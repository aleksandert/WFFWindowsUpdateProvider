using System.Diagnostics;
using Microsoft.Web.Farm;
using System;
using System.IO;
using System.Security.Principal;
using System.Security;

namespace WFFWindowsUpdate
{
    internal class InstallUpdatesRemoteMethod : IServerRemoteMethod
    {
        private class Impersonator : IDisposable
        {
            private Impersonator()
            {
            }

            public static IDisposable ImpersonateProcessIdentity(ServerRemoteMethodContext methodContext)
            {
                return ImpersonateUser(IntPtr.Zero, methodContext);
            }

            public static IDisposable ImpersonateUser(IntPtr userHandle, ServerRemoteMethodContext methodContext)
            {
                bool bSucceeded = false;
                Impersonator impersonator = new Impersonator();
                try
                {
                    impersonator._impersonationContext = WindowsIdentity.Impersonate(userHandle);
                    impersonator._impersonatedUser = WindowsIdentity.GetCurrent().Name;
                    impersonator._methodContext = methodContext;
                    if (userHandle == IntPtr.Zero)
                    {

                        methodContext.TraceMessage(new TraceMessage(TraceLevel.Verbose,
                            string.Format(Resources.TraceMessageImpersonatingProcessIdentity,
                            impersonator._impersonatedUser)));
                    }
                    else
                    {
                        methodContext.TraceMessage(new TraceMessage(TraceLevel.Verbose,
                            string.Format(Resources.TraceMessageImpersonatingUser,
                            impersonator._impersonatedUser)));
                    }
                    bSucceeded = true;
                    return impersonator;
                }
                finally
                {
                    if (!bSucceeded)
                    {
                        impersonator.Dispose();
                    }
                }
            }

            public void Dispose()
            {
                if (_impersonationContext != null)
                {
                    Debug.Assert(!string.IsNullOrEmpty(_impersonatedUser));
                    _methodContext.TraceMessage(new TraceMessage(
                        TraceLevel.Verbose,
                        string.Format(Resources.TraceMessageRevertingFromImpersonation,
                        _impersonatedUser)));
                    _impersonationContext.Undo();
                    _impersonationContext = null;
                }

                GC.SuppressFinalize(this);
            }

            private WindowsImpersonationContext _impersonationContext;
            private string _impersonatedUser;
            private ServerRemoteMethodContext _methodContext;
        }

        public object RunRemote(ServerRemoteMethodContext methodContext, object[] parameters)
        {
            _methodContext = methodContext;

            using (Impersonator.ImpersonateProcessIdentity(_methodContext))
            {
                IUpdateSession updateSession = new UpdateSession();
                IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();

                TraceMessage(TraceLevel.Verbose, Resources.TraceMessageSearchingForUpdates);

                // search for updates and report back the number of updates
                ISearchResult searchResult = updateSearcher.Search("IsInstalled=0 and Type='Software'");
                TraceMessage(TraceLevel.Verbose, string.Format(Resources.TraceMessageNumberOfUpdates, searchResult.Updates.Count));

                if (searchResult.Updates.Count == 0)
                {
                    return false;
                }

                UpdateCollection updatesToDownload = new UpdateCollection();

                foreach (IUpdate item in searchResult.Updates)
                {
                    if (item.InstallationBehavior.CanRequestUserInput == true)
                    {
                        TraceMessage(TraceLevel.Warning, string.Format("Update requires user input. Skipping '{0}'", item.Title));
                    }
                    else
                    {
                        if (item.EulaAccepted == false)
                        {
                            TraceMessage(TraceLevel.Info, string.Format("Accepting EULA for update '{0}'.", item.Title));
                            item.AcceptEula();
                        }

                        if (!item.IsDownloaded)
                        {
                            updatesToDownload.Add(item);
                        }
                    }
                }

                TraceMessage(TraceLevel.Verbose, string.Format(Resources.TraceMessageDownloadingUpdates, searchResult.Updates.Count));

                IUpdateDownloader downloader = updateSession.CreateUpdateDownloader();
                
                downloader.Updates = updatesToDownload;
                IDownloadResult downloadResult = downloader.Download();

                UpdateCollection updatesToInstall = new UpdateCollection();

                for (int i = 0; i < searchResult.Updates.Count; i++)
                {
                    var update = searchResult.Updates[i];
                    if (update.IsDownloaded)
                    {
                        updatesToInstall.Add(update);
                    }
                }

                foreach (IUpdate item in searchResult.Updates)
                {
                    updatesToInstall.Add(item);
                }

                TraceMessage(TraceLevel.Verbose, string.Format(Resources.TraceMessageInstallingUpdates, searchResult.Updates.Count));
                IUpdateInstaller installer = updateSession.CreateUpdateInstaller();
                installer.Updates = updatesToInstall;
                IInstallationResult installationResult = installer.Install();

                TraceMessage(TraceLevel.Verbose, string.Format(Resources.TraceMessageInstallationResultCode, installationResult.ResultCode));

                TraceMessage(TraceLevel.Verbose, installationResult.RebootRequired ? Resources.TraceMessageRebootRequired : Resources.TraceMessageRebootNotRequired);
                return installationResult.RebootRequired;
            }
        }

        private void TraceMessage(TraceLevel traceLevel, string message)
        {
            _methodContext.TraceMessage(new TraceMessage(TraceLevel.Verbose, message));
        }

        private ServerRemoteMethodContext _methodContext;
    }
}
