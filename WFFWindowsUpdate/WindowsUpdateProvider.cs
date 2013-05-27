using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Web.Farm;

namespace WFFWindowsUpdate
{
    [ServerOperationProvider]
    class WindowsUpdateProvider : ServerOperationProvider
    {
        private enum OperationState
        {
            StoppingServer,
            InstallingUpdates,
            Rebooting
        }

        private class SimpleAsyncResult : IAsyncResult, IDisposable
        {
            public SimpleAsyncResult(ServerOperationContext operationContext, AsyncCallback callback, object state)
            {
                _callback = callback;
                _state = state;
                _operationContext = operationContext;
            }

            public void BeginInstall()
            {
                _operationContext.SetCurrentState(OperationState.StoppingServer);

                try
                {
                    RunOperationOptions options = _operationContext.Server.CreateRunOperationOptions(StopOperationProvider.ProviderName);

                    _operationContext.Server.BeginRunOperation(_operationContext,
                        options,
                        StopCompleteCallback,
                        null);
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                    SetComplete();
                }
            }

            public void EndInstall()
            {
                if (_lastException != null)
                {
                    throw _lastException;
                }
            }

            private void StopCompleteCallback(IAsyncResult result)
            {
                try
                {
                    if (result != null)
                    {
                        _operationContext.Server.EndRunOperation(result);
                    }

                    _operationContext.SetCurrentState(OperationState.InstallingUpdates);

                    _operationContext.Server.BeginRunRemote(_operationContext,
                        typeof(InstallUpdatesRemoteMethod),
                        new object[] { },
                        InstallUpdatesCompleteCallback,
                        null);
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                    SetComplete();
                }
            }

            private void InstallUpdatesCompleteCallback(IAsyncResult result)
            {
                try
                {
                    bool rebootRequired = (bool)_operationContext.Server.EndRunRemote(result);
                    // installResult==null means no updates were found
                    if (rebootRequired)
                    {
                        RunOperationOptions options = _operationContext.Server.CreateRunOperationOptions(RebootOperationProvider.ProviderName);
                        options.Parameters[0].Value = Resources.RebootRequiredForInstall;

                        _operationContext.Server.BeginRunOperation(_operationContext,
                            options,
                            RebootCompleteCallback,
                            null);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                }

                SetComplete();
            }

            private void RebootCompleteCallback(IAsyncResult result)
            {
                try
                {
                    _operationContext.Server.EndRunOperation(result);

                    RunOperationOptions options = _operationContext.Server.CreateRunOperationOptions(StopWebServicesOperationProvider.ProviderName);

                    _operationContext.Server.BeginRunOperation(_operationContext,
                        options,
                        StopWebServicesCompleteCallback, null);
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                    SetComplete();
                }
            }

            private void StopWebServicesCompleteCallback(IAsyncResult result)
            {
                try
                {
                    _operationContext.Server.EndRunOperation(result);
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                }

                SetComplete();
            }

            private void SetComplete()
            {
                _isComplete = true;
                _waitHandle.Set();

                if (_callback != null)
                {
                    _callback(this);
                }
            }

            public void Dispose()
            {
                if (_waitHandle != null)
                {
                    _waitHandle.Close();
                    _waitHandle = null;
                }

                GC.SuppressFinalize(this);
            }
        
            public object AsyncState
            {
                get { return _state; }
            }

            public System.Threading.WaitHandle AsyncWaitHandle
            {
                get { return _waitHandle; }
            }

            public bool CompletedSynchronously
            {
                get { return false; }
            }

            public bool IsCompleted
            {
                get { return _isComplete; }
            }

            private AsyncCallback _callback;
            private object _state;
            private bool _isComplete;
            private ServerOperationContext _operationContext;
            private ManualResetEvent _waitHandle = new ManualResetEvent(false);
            private Exception _lastException;
        }

        public override string Name
        {
            get { return ProviderName; }
        }

        public override OperationCategory Category
        {
            get { return OperationCategory.Provisioning; }
        }

        public override string FriendlyName
        {
            get { return Resources.WindowsUpdateProviderFriendlyName; }
        }

        public override string Description
        {
            get { return Resources.WindowsUpdateProviderDescription; }
        }

        public override IAsyncResult BeginRunOperation(ServerOperationContext operationContext, AsyncCallback callback, object state)
        {
            SimpleAsyncResult asyncResult = new SimpleAsyncResult(operationContext, callback, state);
            asyncResult.BeginInstall();
            return asyncResult;
        }

        public override object EndRunOperation(IAsyncResult result)
        {
            using (SimpleAsyncResult asyncResult = result as SimpleAsyncResult)
            {
                if (asyncResult != null)
                {
                    asyncResult.EndInstall();
                    return null;
                }
                else
                {
                    throw new ArgumentException(Resources.WrongAsyncResult, "result");
                }
            }
        }

        private const string ProviderName = "SMWindowsUpdateProvider";
        private const string UpdatesParameterName = "SMUpdates";
    }
}
