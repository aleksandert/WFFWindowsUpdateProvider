using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.Principal;

namespace WFFWindowsUpdate
{
    internal class Impersonator : IDisposable
    {
        private Impersonator()
        {
        }

        public static IDisposable ImpersonateProcessIdentity()
        {
            return ImpersonateUser(IntPtr.Zero);
        }

        public static IDisposable ImpersonateUser(IntPtr userHandle)
        {
            bool bSucceeded = false;
            Impersonator impersonator = new Impersonator();
            try
            {
                impersonator._impersonationContext = WindowsIdentity.Impersonate(userHandle);
                impersonator._impersonatedUser = WindowsIdentity.GetCurrent().Name;
                if (userHandle == IntPtr.Zero)
                {
                    Tracer.TraceInformation(DeploymentTraceSource.Impersonation,
                        Resources.ImpersonatingProcessIdentity,
                        impersonator._impersonatedUser);
                }
                else
                {
                    Tracer.TraceInformation(DeploymentTraceSource.Impersonation,
                        Resources.ImpersonatingUser,
                        impersonator._impersonatedUser);
                }
                bSucceeded = true;
                return impersonator;
            }
            catch (UnauthorizedAccessException e)
            {
                throw new DeploymentAuthorizationException(Resources.ClientErrorForAuthSettings, Resources.TaskDelegationToProcessFailed, e);

            }
            catch (SecurityException e)
            {
                throw new DeploymentAuthorizationException(Resources.ClientErrorForAuthSettings, Resources.TaskDelegationToProcessFailed, e);
            }
            finally
            {
                if (!bSucceeded)
                {
                    impersonator.Dispose();
                }
            }
        }
    }

    internal static class ProcessIdentity
    {
        private static WindowsIdentity GetProcessIdentity()
        {
            using (Impersonator.ImpersonateProcessIdentity())
            {
                return WindowsIdentity.GetCurrent();
            }
        }

        static internal bool RunningAsProcess()
        {
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();

            return (currentIdentity.User.Equals(sm_processIdentity.User));
        }

        static WindowsIdentity sm_processIdentity = GetProcessIdentity();
    }

    internal class ProcessInfoWrapper : IDisposable
    {
        private ProcessInfoWrapper(NativeMethods.STARTUPINFO startupInfo,
            NativeMethods.PROCESS_INFORMATION processInfo,
            SafeFileHandle standardInputPipeHandle,
            SafeFileHandle standardOutputReadPipeHandle,
            SafeFileHandle standardErrorReadPipeHandle)
        {
            Debug.Assert(startupInfo != null);
            Debug.Assert(processInfo != null);
            _startupInfo = startupInfo;
            _processInfo = processInfo;

            _hThreadWrapper = new SafeIntPtrHandle(_processInfo.hThread);
            _hProcessWrapper = new SafeIntPtrHandle(_processInfo.hProcess);

            if (standardInputPipeHandle != null)
            {
                _standardInputStream = new FileStream(standardInputPipeHandle, FileAccess.Write, 4096, false);
            }
            if (standardOutputReadPipeHandle != null)
            {
                _standardOutputStreamReader = GetStreamReader(standardOutputReadPipeHandle);
            }
            if (standardErrorReadPipeHandle != null)
            {
                _standardErrorStreamReader = GetStreamReader(standardErrorReadPipeHandle);
            }

            Debug.Assert(_processInfo.dwProcessId != 0);
            _process = Process.GetProcessById(_processInfo.dwProcessId);
            _process.EnableRaisingEvents = true;
        }

        private static StreamReader GetStreamReader(SafeFileHandle fileHandle)
        {
            Debug.Assert(fileHandle != null && !fileHandle.IsInvalid);
            FileStream stream = new FileStream(fileHandle, FileAccess.Read, 4096, false);
            StreamReader reader = new StreamReader(stream, true);
            return reader;
        }

        public bool HasExited
        {
            get
            {
                return _process.HasExited;
            }
        }

        public bool WaitForExit(int milliseconds)
        {
            return _process.WaitForExit(milliseconds);
        }

        public int? EnsureExit()
        {
            Debug.Assert(_process != null);

            int? exitCode = null;
            try
            {
                if (!_process.HasExited)
                {
                    if (!_process.CloseMainWindow())
                    {
                        // i think this needs to be changed to kill process tree.
                        _process.Kill();
                    }
                    else
                    {
                        exitCode = _process.ExitCode;
                    }
                }
                else
                {
                    exitCode = _process.ExitCode;
                }
            }
            catch (InvalidOperationException)
            {
                Debug.Assert(false);
                // process has exited
            }

            if (exitCode.HasValue)
            {
                this.Dispose();
            }
            return exitCode;
        }

        public void Resume()
        {
            if (this._hThreadWrapper != null && !this._hThreadWrapper.IsInvalid)
            {
                int threadSuspendCount = NativeMethods.ResumeThread(this._hThreadWrapper);
                if (threadSuspendCount == -1)
                {
                    throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                Debug.Assert(threadSuspendCount == 1);
            }
            else
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }
        }

        public void BeginOutputReadLine(UserCallBack callback)
        {
            Debug.Assert(_standardOutputStreamReader != null);
            Debug.Assert(_standardOutputStreamReaderAsync == null);
            _standardOutputStreamReaderAsync = new AsyncStreamReader(_standardOutputStreamReader, callback);
            _standardOutputStreamReaderAsync.BeginRead();
        }

        public void BeginErrorReadLine(UserCallBack callback)
        {
            Debug.Assert(_standardErrorStreamReader != null);
            Debug.Assert(_standardErrorStreamReaderAsync == null);
            _standardErrorStreamReaderAsync = new AsyncStreamReader(_standardErrorStreamReader, callback);
            _standardErrorStreamReaderAsync.BeginRead();
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public void Dispose()
        {
            if (_standardInputStream != null)
            {
                _standardInputStream.Dispose();
                _standardInputStream = null;
            }

            if (_startupInfo != null)
            {
                _startupInfo.Dispose();
                _startupInfo = null;
            }

            if (_hProcessWrapper != null)
            {
                _hProcessWrapper.Dispose();
                _hProcessWrapper = null;
            }

            if (_hThreadWrapper != null)
            {
                _hThreadWrapper.Dispose();
                _hThreadWrapper = null;
            }

            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }

            GC.SuppressFinalize(this);
        }

        private static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
            public class STARTUPINFO : IDisposable
            {
                public int cb;
                public IntPtr lpReserved = IntPtr.Zero;
                public IntPtr lpDesktop = IntPtr.Zero;
                public IntPtr lpTitle = IntPtr.Zero;
                public int dwX;
                public int dwY;
                public int dwXSize;
                public int dwYSize;
                public int dwXCountChars;
                public int dwYCountChars;
                public int dwFillAttribute;
                public int dwFlags;
                public short wShowWindow;
                public short cbReserved2;
                public IntPtr lpReserved2 = IntPtr.Zero;
                public SafeFileHandle hStdInput = new SafeFileHandle(IntPtr.Zero, false);
                public SafeFileHandle hStdOutput = new SafeFileHandle(IntPtr.Zero, false);
                public SafeFileHandle hStdError = new SafeFileHandle(IntPtr.Zero, false);

                public STARTUPINFO()
                {
                    cb = Marshal.SizeOf(this);
                }

                [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
                public void Dispose()
                {
                    // close the handles created for child process
                    if (hStdInput != null)
                    {
                        hStdInput.Dispose();
                        hStdInput = null;
                    }

                    if (hStdOutput != null)
                    {
                        hStdOutput.Dispose();
                        hStdOutput = null;
                    }

                    if (hStdError != null)
                    {
                        hStdError.Dispose();
                        hStdError = null;
                    }

                    GC.SuppressFinalize(this);
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public class PROCESS_INFORMATION
            {
                public IntPtr hProcess;
                public IntPtr hThread;
                public int dwProcessId;
                public int dwThreadId;
            }

            [StructLayout(LayoutKind.Sequential)]
            public class SECURITY_ATTRIBUTES
            {
                public Int32 Length;
                public IntPtr lpSecurityDescriptor;
                public bool bInheritHandle;

                public SECURITY_ATTRIBUTES()
                {
                    this.Length = Marshal.SizeOf(this);
                }
            }

            public enum SECURITY_IMPERSONATION_LEVEL
            {
                SecurityAnonymous,
                SecurityIdentification,
                SecurityImpersonation,
                SecurityDelegation
            }

            public enum TOKEN_TYPE
            {
                TokenPrimary = 1,
                TokenImpersonation
            }

            public const int GENERIC_ALL_ACCESS = 0x10000000;

            [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CreateProcessAsUser(
                SafeHandle token,
                string lpApplicationName,
                string lpCommandLine,
                SECURITY_ATTRIBUTES lpProcessAttributes,
                SECURITY_ATTRIBUTES lpThreadAttributes,
                [MarshalAs(UnmanagedType.Bool)] bool bInheritHandles,
                int dwCreationFlags,
                IntPtr lpEnvironment,
                string lpCurrentDirectory,
                STARTUPINFO lpStartupInfo,
                PROCESS_INFORMATION lpProcessInformation
                );

            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool CreateProcess(
                string lpApplicationName,
                string lpCommandLine,
                SECURITY_ATTRIBUTES lpProcessAttributes,
                SECURITY_ATTRIBUTES lpThreadAttributes,
                [MarshalAs(UnmanagedType.Bool)] bool bInheritHandles,
                int dwCreationFlags,
                IntPtr lpEnvironment,
                string lpCurrentDirectory,
                STARTUPINFO lpStartupInfo,
                PROCESS_INFORMATION lpProcessInformation
                );


            [DllImport("advapi32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public extern static bool DuplicateTokenEx(
                SafeHandle hToken,
                int access,
                SECURITY_ATTRIBUTES tokenAttributes,
                int impersonationLevel,
                int tokenType,
                out SafeIntPtrHandle hNewToken
                );

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CreatePipe(
                out SafeFileHandle hReadPipe,
                out SafeFileHandle hWritePipe,
                SECURITY_ATTRIBUTES lpPipeAttributes,
                int nSize);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetCurrentProcess();

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DuplicateHandle(
                SafeHandle hSourceProcessHandle,
                SafeHandle hSourceHandle,
                SafeHandle hTargetProcess,
                out SafeFileHandle targetHandle,
                int dwDesiredAccess,
                [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
                int dwOptions);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int ResumeThread(SafeHandle hThread);

            public const int DUPLICATE_SAME_ACCESS = 2;

            public const int CREATE_NO_WINDOW = 0x08000000;
            public const int CREATE_SUSPENDED = 0x00000004;
            public const int CREATE_UNICODE_ENVIRONMENT = 0x00000400;

            public const int STARTF_USESTDHANDLES = 0x00000100;
            public const int STD_INPUT_HANDLE = -10;
            public const int STD_OUTPUT_HANDLE = -11;
            public const int STD_ERROR_HANDLE = -12;
        }

        private static void CreatePipeWithSecurityAttributes(out SafeFileHandle hReadPipe,
            out SafeFileHandle hWritePipe,
            NativeMethods.SECURITY_ATTRIBUTES lpPipeAttributes,
            int nSize)
        {
            bool ret = NativeMethods.CreatePipe(out hReadPipe, out hWritePipe, lpPipeAttributes, nSize);
            if (!ret || hReadPipe.IsInvalid || hWritePipe.IsInvalid)
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        // Using synchronous Anonymous pipes for process input/output redirection means we would end up 
        // wasting a worker threadpool thread per pipe instance. Overlapped pipe IO is desirable, since 
        // it will take advantage of the NT IO completion port infrastructure. But we can't really use 
        // Overlapped I/O for process input/output as it would break Console apps (managed Console class 
        // methods such as WriteLine as well as native CRT functions like printf) which are making an
        // assumption that the console standard handles (obtained via GetStdHandle()) are opened
        // for synchronous I/O and hence they can work fine with ReadFile/WriteFile synchrnously!
        private static void CreatePipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
        {
            NativeMethods.SECURITY_ATTRIBUTES securityAttributesParent = new NativeMethods.SECURITY_ATTRIBUTES();
            securityAttributesParent.bInheritHandle = true;

            SafeFileHandle hTmp = null;
            try
            {
                if (parentInputs)
                {
                    CreatePipeWithSecurityAttributes(out childHandle, out hTmp, securityAttributesParent, 0);
                }
                else
                {
                    CreatePipeWithSecurityAttributes(out hTmp,
                        out childHandle,
                        securityAttributesParent,
                        0);
                }
                // Duplicate the parent handle to be non-inheritable so that the child process 
                // doesn't have access. This is done for correctness sake, exact reason is unclear.
                // One potential theory is that child process can do something brain dead like 
                // closing the parent end of the pipe and there by getting into a blocking situation
                // as parent will not be draining the pipe at the other end anymore. 
                if (!NativeMethods.DuplicateHandle(new SafeFileHandle(NativeMethods.GetCurrentProcess(), true),
                    hTmp,
                    new SafeFileHandle(NativeMethods.GetCurrentProcess(), true),
                    out parentHandle,
                    0,
                    false,
                    NativeMethods.DUPLICATE_SAME_ACCESS))
                {
                    throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }
            finally
            {
                if (hTmp != null && !hTmp.IsInvalid)
                {
                    hTmp.Close();
                }
            }
        }

        private static SafeIntPtrHandle GetDuplicateToken(SafeHandle userToken,
            NativeMethods.SECURITY_ATTRIBUTES securityAttributes)
        {
            SafeIntPtrHandle hDupedToken;
            if (!NativeMethods.DuplicateTokenEx(
                  userToken,
                  NativeMethods.GENERIC_ALL_ACCESS,
                  securityAttributes,
                  (int)NativeMethods.SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                  (int)NativeMethods.TOKEN_TYPE.TokenPrimary,
                  out hDupedToken))
            {
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            else
            {
                return hDupedToken;
            }
        }

        public static ProcessInfoWrapper CreateSuspendedProcessAsUser(SafeHandle userToken,
            string processPath,
            string arguments,
            bool redirected)
        {
            Debug.Assert(!string.IsNullOrEmpty(processPath));

            SafeFileHandle standardOutputReadPipeHandle = null;
            SafeFileHandle standardErrorReadPipeHandle = null;
            SafeFileHandle standardInputPipeHandle = null;
            NativeMethods.PROCESS_INFORMATION processInfo = new NativeMethods.PROCESS_INFORMATION();
            NativeMethods.SECURITY_ATTRIBUTES securityAttributes = new NativeMethods.SECURITY_ATTRIBUTES();
            NativeMethods.STARTUPINFO startupInfo = new NativeMethods.STARTUPINFO();
            SafeIntPtrHandle hDupedToken = GetDuplicateToken(userToken, securityAttributes);

            if (redirected)
            {
                startupInfo.dwFlags = NativeMethods.STARTF_USESTDHANDLES;
                // you have to redirect all or none of the i/o handles
                CreatePipe(out standardInputPipeHandle, out startupInfo.hStdInput, true);
                CreatePipe(out standardOutputReadPipeHandle, out startupInfo.hStdOutput, false);
                CreatePipe(out standardErrorReadPipeHandle, out startupInfo.hStdError, false);
            }

            int processCreationFlags =
                NativeMethods.CREATE_SUSPENDED |
                NativeMethods.CREATE_NO_WINDOW |
                NativeMethods.CREATE_UNICODE_ENVIRONMENT;

            bool retVal;

            if (ProcessIdentity.RunningAsProcess())
            {
                retVal = NativeMethods.CreateProcess(
                                                processPath,
                                                arguments,
                                                null,
                                                null,
                                                true,
                                                processCreationFlags,
                                                IntPtr.Zero,
                                                null,
                                                startupInfo,
                                                processInfo);
            }
            else
            {
                retVal = NativeMethods.CreateProcessAsUser(
                                                hDupedToken,
                                                processPath,
                                                arguments,
                                                null,
                                                null,
                                                true,
                                                processCreationFlags,
                                                IntPtr.Zero,
                                                null,
                                                startupInfo,
                                                processInfo);
            }

            if (!retVal)
            {
                int errorCode = Marshal.GetHRForLastWin32Error();
                startupInfo.Dispose();
                if (standardInputPipeHandle != null)
                {
                    standardInputPipeHandle.Dispose();
                }
                if (standardOutputReadPipeHandle != null)
                {
                    standardOutputReadPipeHandle.Dispose();
                }
                if (standardErrorReadPipeHandle != null)
                {
                    standardErrorReadPipeHandle.Dispose();
                }
                throw Marshal.GetExceptionForHR(errorCode);
            }
            else
            {
                return new ProcessInfoWrapper(startupInfo,
                    processInfo,
                    standardInputPipeHandle,
                    standardOutputReadPipeHandle,
                    standardErrorReadPipeHandle);
            }
        }

        private SafeIntPtrHandle _hProcessWrapper;
        private SafeIntPtrHandle _hThreadWrapper;
        private Process _process;
        private NativeMethods.PROCESS_INFORMATION _processInfo;
        private NativeMethods.STARTUPINFO _startupInfo;
        private Stream _standardInputStream;

        // these last 4 items are not disposed by this class becasue the async reader itself handles
        // closing the underlying stream after the final read.
        // if we disposed them here, we'd be pulling the rug from under the async reader
        private StreamReader _standardOutputStreamReader;
        private StreamReader _standardErrorStreamReader;
        private AsyncStreamReader _standardOutputStreamReaderAsync;
        private AsyncStreamReader _standardErrorStreamReaderAsync;
    }
}
