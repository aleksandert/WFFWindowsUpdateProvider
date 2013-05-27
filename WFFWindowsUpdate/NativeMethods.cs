using System;
using System.Collections;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

namespace WFFWindowsUpdate
{
    public enum OperationResultCode
    {
        orcNotStarted,
        orcInProgress,
        orcSucceeded,
        orcSucceededWithErrors,
        orcFailed,
        orcAborted
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("7C907864-346C-4AEB-8F3F-57DA289F969F")]
    public interface IImageInformation
    {
        [DispId(0x60020001)]
        string AltText { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        int Height { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        string Source { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        int Width { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("81DDC1B8-9D35-47A6-B471-5B80F519223B"), DefaultMember("Name")]
    public interface ICategory
    {
        [DispId(0)]
        string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        string CategoryID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        ICategoryCollection Children { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        string Description { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        IImageInformation Image { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        int Order { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        ICategory Parent { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [DispId(0x60020007)]
        string Type { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] get; }
        [DispId(0x60020008)]
        UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("3A56BFB8-576C-43F7-9335-FE4838FD7E37")]
    public interface ICategoryCollection : IEnumerable
    {
        [DispId(0)]
        ICategory this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }

    public enum UpdateExceptionContext
    {
        uecGeneral = 1,
        uecWindowsDriver = 2,
        uecWindowsInstaller = 3
    }

    [ComImport, DefaultMember("Message"), TypeLibType((short)0x10c0), Guid("A376DD5E-09D4-427F-AF7C-FED5B6E1C1D6")]
    public interface IUpdateException
    {
        [DispId(0)]
        string Message { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002), ComAliasName("WUApiLib.UpdateExceptionContext")]
        UpdateExceptionContext Context { [return: ComAliasName("WUApiLib.UpdateExceptionContext")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("503626A3-8E14-4729-9355-0FE664BD2321")]
    public interface IUpdateExceptionCollection : IEnumerable
    {
        [DispId(0)]
        IUpdateException this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }

    [ComImport, Guid("D40CFF62-E08C-4498-941A-01E25F0FD33C"), TypeLibType((short)0x10c0)]
    public interface ISearchResult
    {
        [DispId(0x60020001), ComAliasName("WUApiLib.OperationResultCode")]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        ICategoryCollection RootCategories { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        IUpdateExceptionCollection Warnings { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
    }

    public enum ServerSelection
    {
        ssDefault,
        ssManagedServer,
        ssWindowsUpdate,
        ssOthers
    }

    public enum UpdateOperation
    {
        uoInstallation = 1,
        uoUninstallation = 2
    }

    [ComImport, Guid("BE56A644-AF0E-4E0E-A311-C1D8E695CBFF"), TypeLibType((short)0x10c0)]
    public interface IUpdateHistoryEntry
    {
        [DispId(0x60020001), ComAliasName("WUApiLib.UpdateOperation")]
        UpdateOperation Operation { [return: ComAliasName("WUApiLib.UpdateOperation")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002), ComAliasName("WUApiLib.OperationResultCode")]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        DateTime Date { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        IUpdateIdentity UpdateIdentity { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        string Title { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [DispId(0x60020007)]
        string Description { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] get; }
        [DispId(0x60020008)]
        int UnmappedResultCode { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)] get; }
        [DispId(0x60020009)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)] get; }
        [ComAliasName("WUApiLib.ServerSelection"), DispId(0x6002000a)]
        ServerSelection ServerSelection { [return: ComAliasName("WUApiLib.ServerSelection")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)] get; }
        [DispId(0x6002000b)]
        string ServiceID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)] get; }
        [DispId(0x6002000c)]
        StringCollection UninstallationSteps { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000c)] get; }
        [DispId(0x6002000d)]
        string UninstallationNotes { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)] get; }
        [DispId(0x6002000e)]
        string SupportUrl { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000e)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("A7F04F3C-A290-435B-AADF-A116C3357A5C")]
    public interface IUpdateHistoryEntryCollection : IEnumerable
    {
        [DispId(0)]
        IUpdateHistoryEntry this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("7366EA16-7A1A-4EA2-B042-973D3E9CD99B")]
    public interface ISearchJob
    {
        [DispId(0x60020001)]
        object AsyncState { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool IsCompleted { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        void CleanUp();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        void RequestAbort();
    }

    [ComImport, Guid("8F45ABF1-F9AE-4B95-A933-F0F66E5056EA"), TypeLibType((short)0x10c0)]
    public interface IUpdateSearcher
    {
        [DispId(0x60020001)]
        bool CanAutomaticallyUpgradeService { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020003)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] set; }
        [DispId(0x60020004)]
        bool IncludePotentiallySupersededUpdates { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] set; }
        [DispId(0x60020007), ComAliasName("WUApiLib.ServerSelection")]
        ServerSelection ServerSelection { [return: ComAliasName("WUApiLib.ServerSelection")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] get; [param: In, ComAliasName("WUApiLib.ServerSelection")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)]
        ISearchJob BeginSearch([In, MarshalAs(UnmanagedType.BStr)] string criteria, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)]
        ISearchResult EndSearch([In, MarshalAs(UnmanagedType.Interface)] ISearchJob searchJob);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)]
        string EscapeString([In, MarshalAs(UnmanagedType.BStr)] string unescaped);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)]
        IUpdateHistoryEntryCollection QueryHistory([In] int startIndex, [In] int Count);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000c)]
        ISearchResult Search([In, MarshalAs(UnmanagedType.BStr)] string criteria);
        [DispId(0x6002000d)]
        bool Online { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)] set; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000e)]
        int GetTotalHistoryCount();
        [DispId(0x6002000f)]
        string ServiceID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000f)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000f)] set; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("D940F0F8-3CBB-4FD0-993F-471E7F2328AD")]
    public interface IUpdateInstallationResult
    {
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool RebootRequired { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003), ComAliasName("WUApiLib.OperationResultCode")]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
    }

    [ComImport, Guid("A43C56D6-7451-48D4-AF96-B6CD2D0D9B7A"), TypeLibType((short)0x10c0)]
    public interface IInstallationResult
    {
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool RebootRequired { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003), ComAliasName("WUApiLib.OperationResultCode")]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        IUpdateInstallationResult GetUpdateResult([In] int updateIndex);
    }

    [ComImport, Guid("345C8244-43A3-4E32-A368-65F073B76F36"), TypeLibType((short)0x10c0)]
    public interface IInstallationProgress
    {
        [DispId(0x60020001)]
        int CurrentUpdateIndex { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        int CurrentUpdatePercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        int PercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        IUpdateInstallationResult GetUpdateResult([In] int updateIndex);
    }

    [ComImport, Guid("5C209F0B-BAD5-432A-9556-4699BED2638A"), TypeLibType((short)0x10c0)]
    public interface IInstallationJob
    {
        [DispId(0x60020001)]
        object AsyncState { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool IsCompleted { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        void CleanUp();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        IInstallationProgress GetProgress();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        void RequestAbort();
    }

    [ComImport, ComConversionLoss, Guid("7B929C68-CCDC-4226-96B1-8724600B54C2"), TypeLibType((short)0x10c0)]
    public interface IUpdateInstaller
    {
        [DispId(0x60020001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool IsForced { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] set; }
        [DispId(0x60020003), ComAliasName("WUApiLib.wireHWND")]
        IntPtr ParentHwnd { [return: ComAliasName("WUApiLib.wireHWND")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short)1), DispId(0x60020003)] get; [param: In, ComAliasName("WUApiLib.wireHWND")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), TypeLibFunc((short)1), DispId(0x60020003)] set; }
        [DispId(0x60020004)]
        object parentWindow { [return: MarshalAs(UnmanagedType.IUnknown)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, MarshalAs(UnmanagedType.IUnknown)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] set; }
        [DispId(0x60020005)]
        UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        IInstallationJob BeginInstall([In, MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        IInstallationJob BeginUninstall([In, MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)]
        IInstallationResult EndInstall([In, MarshalAs(UnmanagedType.Interface)] IInstallationJob value);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)]
        IInstallationResult EndUninstall([In, MarshalAs(UnmanagedType.Interface)] IInstallationJob value);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)]
        IInstallationResult Install();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)]
        IInstallationResult RunWizard([In, Optional, DefaultParameterValue(""), MarshalAs(UnmanagedType.BStr)] string dialogTitle);
        [DispId(0x6002000c)]
        bool IsBusy { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000c)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)]
        IInstallationResult Uninstall();
        [DispId(0x6002000e)]
        bool AllowSourcePrompts { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000e)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000e)] set; }
        [DispId(0x6002000f)]
        bool RebootRequiredBeforeInstallation { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000f)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("BF99AF76-B575-42AD-8AA4-33CBB5477AF1")]
    public interface IUpdateDownloadResult
    {
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [ComAliasName("WUApiLib.OperationResultCode"), DispId(0x60020002)]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("DAA4FDD0-4727-4DBE-A1E7-745DCA317144")]
    public interface IDownloadResult
    {
        [DispId(0x60020001)]
        int HResult { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002), ComAliasName("WUApiLib.OperationResultCode")]
        OperationResultCode ResultCode { [return: ComAliasName("WUApiLib.OperationResultCode")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        IUpdateDownloadResult GetUpdateResult([In] int updateIndex);
    }

    public enum DownloadPhase
    {
        dphDownloading = 2,
        dphInitializing = 1,
        dphVerifying = 3
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("D31A5BAC-F719-4178-9DBB-5E2CB47FD18A")]
    public interface IDownloadProgress
    {
        [DispId(0x60020001)]
        decimal CurrentUpdateBytesDownloaded { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        decimal CurrentUpdateBytesToDownload { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        int CurrentUpdateIndex { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        int PercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        decimal TotalBytesDownloaded { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        decimal TotalBytesToDownload { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        IUpdateDownloadResult GetUpdateResult([In] int updateIndex);
        [ComAliasName("WUApiLib.DownloadPhase"), DispId(0x60020008)]
        DownloadPhase CurrentUpdateDownloadPhase { [return: ComAliasName("WUApiLib.DownloadPhase")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)] get; }
        [DispId(0x60020009)]
        int CurrentUpdatePercentComplete { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("C574DE85-7358-43F6-AAE8-8697E62D8BA7")]
    public interface IDownloadJob
    {
        [DispId(0x60020001)]
        object AsyncState { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool IsCompleted { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        void CleanUp();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        IDownloadProgress GetProgress();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        void RequestAbort();
    }

    [ComImport, TypeLibType((short)0x10d0), Guid("68F1C6F9-7ECC-4666-A464-247FE12496C3")]
    public interface IUpdateDownloader
    {
        [DispId(0x60020001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool IsForced { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] set; }
        [DispId(0x60020003), ComAliasName("WUApiLib.DownloadPriority")]
        DownloadPriority Priority { [return: ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] set; }
        [DispId(0x60020004)]
        UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        IDownloadJob BeginDownload([In, MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        IDownloadResult Download();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        IDownloadResult EndDownload([In, MarshalAs(UnmanagedType.Interface)] IDownloadJob value);
    }

    [ComImport, ClassInterface((short)0), Guid("5BAF654A-5A07-4264-A255-9FF54C7151E7"), TypeLibType((short)2)]
    public class UpdateDownloaderClass : IUpdateDownloader, UpdateDownloader
    {
        // Methods
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        public virtual extern IDownloadJob BeginDownload([In, MarshalAs(UnmanagedType.IUnknown)] object onProgressChanged, [In, MarshalAs(UnmanagedType.IUnknown)] object onCompleted, [In, MarshalAs(UnmanagedType.Struct)] object state);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        public virtual extern IDownloadResult Download();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        public virtual extern IDownloadResult EndDownload([In, MarshalAs(UnmanagedType.Interface)] IDownloadJob value);

        // Properties
        [DispId(0x60020001)]
        public extern string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        public extern bool IsForced { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] set; }
        [ComAliasName("WUApiLib.DownloadPriority"), DispId(0x60020003)]
        public extern DownloadPriority Priority { [return: ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] set; }
        [DispId(0x60020004)]
        public extern UpdateCollection Updates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] set; }
    }

    [ComImport, CoClass(typeof(UpdateDownloaderClass)), Guid("68F1C6F9-7ECC-4666-A464-247FE12496C3")]
    public interface UpdateDownloader : IUpdateDownloader
    {
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("816858A4-260D-4260-933A-2585F1ABC76B")]
    public interface IUpdateSession
    {
        [DispId(0x60020001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        WebProxy WebProxy { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        IUpdateSearcher CreateUpdateSearcher();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        UpdateDownloader CreateUpdateDownloader();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        IUpdateInstaller CreateUpdateInstaller();
    }

    [ComImport, Guid("46297823-9940-4C09-AED9-CD3EA6D05968"), TypeLibType((short)0x10c0)]
    public interface IUpdateIdentity
    {
        [DispId(0x60020002)]
        int RevisionNumber { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        string UpdateID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
    }

    [ComImport, TypeLibType((short)2), ClassInterface((short)0), Guid("72C97D74-7C3B-40AE-B77D-ABDB22EBA6FB")]
    public class StringCollectionClass : IStringCollection, StringCollection, IEnumerable
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        public virtual extern int Add([In, MarshalAs(UnmanagedType.BStr)] string value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        public virtual extern void Clear();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        public virtual extern StringCollection Copy();
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        public virtual extern IEnumerator GetEnumerator();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        public virtual extern void Insert([In] int index, [In, MarshalAs(UnmanagedType.BStr)] string value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        public virtual extern void RemoveAt([In] int index);

        // Properties
        [DispId(0x60020001)]
        public extern int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0)]
        public extern string this[int index] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] set; }
        [DispId(0x60020002)]
        public extern bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }



    [ComImport, Guid("EFF90582-2DDC-480F-A06D-60F3FBC362C3"), TypeLibType((short)0x10d0)]
    public interface IStringCollection : IEnumerable
    {
        [DispId(0)]
        string this[int index] { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] set; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        int Add([In, MarshalAs(UnmanagedType.BStr)] string value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        void Clear();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        StringCollection Copy();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        void Insert([In] int index, [In, MarshalAs(UnmanagedType.BStr)] string value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        void RemoveAt([In] int index);
    }

    [ComImport, Guid("EFF90582-2DDC-480F-A06D-60F3FBC362C3"), CoClass(typeof(StringCollectionClass))]
    public interface StringCollection : IStringCollection
    {
    }

    public enum InstallationImpact
    {
        iiNormal,
        iiMinor,
        iiRequiresExclusiveHandling
    }

    public enum InstallationRebootBehavior
    {
        irbNeverReboots,
        irbAlwaysRequiresReboot,
        irbCanRequestReboot
    }

    [ComImport, Guid("D9A59339-E245-4DBD-9686-4D5763E39624"), TypeLibType((short)0x10c0)]
    public interface IInstallationBehavior
    {
        [DispId(0x60020001)]
        bool CanRequestUserInput { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [ComAliasName("WUApiLib.InstallationImpact"), DispId(0x60020002)]
        InstallationImpact Impact { [return: ComAliasName("WUApiLib.InstallationImpact")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [ComAliasName("WUApiLib.InstallationRebootBehavior"), DispId(0x60020003)]
        InstallationRebootBehavior RebootBehavior { [return: ComAliasName("WUApiLib.InstallationRebootBehavior")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        bool RequiresNetworkConnectivity { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
    }

    public enum UpdateType
    {
        utDriver = 2,
        utSoftware = 1
    }

    public enum DeploymentAction
    {
        daNone,
        daInstallation,
        daUninstallation,
        daDetection
    }

    public enum DownloadPriority
    {
        dpHigh = 3,
        dpLow = 1,
        dpNormal = 2
    }

    [ComImport, Guid("54A2CB2D-9A0C-48B6-8A50-9ABB69EE2D02"), TypeLibType((short)0x10c0)]
    public interface IUpdateDownloadContent
    {
        [DispId(0x60020001)]
        string DownloadUrl { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("BC5513C8-B3B8-4BF7-A4D4-361C0D8C88BA")]
    public interface IUpdateDownloadContentCollection : IEnumerable
    {
        [DispId(0)]
        IUpdateDownloadContent this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("6A92B07A-D821-4682-B423-5C805022CC4D"), DefaultMember("Title")]
    public interface IUpdate
    {
        [DispId(0)]
        string Title { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        bool AutoSelectOnWebSites { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        UpdateCollection BundledUpdates { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        bool CanRequireSource { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        ICategoryCollection Categories { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        object Deadline { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        bool DeltaCompressedContentAvailable { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [DispId(0x60020007)]
        bool DeltaCompressedContentPreferred { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] get; }
        [DispId(0x60020008)]
        string Description { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)] get; }
        [DispId(0x60020009)]
        bool EulaAccepted { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020009)] get; }
        [DispId(0x6002000a)]
        string EulaText { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)] get; }
        [DispId(0x6002000b)]
        string HandlerID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)] get; }
        [DispId(0x6002000c)]
        IUpdateIdentity Identity { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000c)] get; }
        [DispId(0x6002000d)]
        IImageInformation Image { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)] get; }
        [DispId(0x6002000e)]
        IInstallationBehavior InstallationBehavior { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000e)] get; }
        [DispId(0x6002000f)]
        bool IsBeta { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000f)] get; }
        [DispId(0x60020010)]
        bool IsDownloaded { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020010)] get; }
        [DispId(0x60020011)]
        bool IsHidden { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020011)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020011)] set; }
        [DispId(0x60020012)]
        bool IsInstalled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020012)] get; }
        [DispId(0x60020013)]
        bool IsMandatory { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020013)] get; }
        [DispId(0x60020014)]
        bool IsUninstallable { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020014)] get; }
        [DispId(0x60020015)]
        StringCollection Languages { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020015)] get; }
        [DispId(0x60020016)]
        DateTime LastDeploymentChangeTime { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020016)] get; }
        [DispId(0x60020017)]
        decimal MaxDownloadSize { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020017)] get; }
        [DispId(0x60020018)]
        decimal MinDownloadSize { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020018)] get; }
        [DispId(0x60020019)]
        StringCollection MoreInfoUrls { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020019)] get; }
        [DispId(0x6002001a)]
        string MsrcSeverity { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002001a)] get; }
        [DispId(0x6002001b)]
        int RecommendedCpuSpeed { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002001b)] get; }
        [DispId(0x6002001c)]
        int RecommendedHardDiskSpace { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002001c)] get; }
        [DispId(0x6002001d)]
        int RecommendedMemory { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002001d)] get; }
        [DispId(0x6002001e)]
        string ReleaseNotes { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002001e)] get; }
        [DispId(0x6002001f)]
        StringCollection SecurityBulletinIDs { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002001f)] get; }
        [DispId(0x60020021)]
        StringCollection SupersededUpdateIDs { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020021)] get; }
        [DispId(0x60020022)]
        string SupportUrl { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020022)] get; }
        [ComAliasName("WUApiLib.UpdateType"), DispId(0x60020023)]
        UpdateType Type { [return: ComAliasName("WUApiLib.UpdateType")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020023)] get; }
        [DispId(0x60020024)]
        string UninstallationNotes { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020024)] get; }
        [DispId(0x60020025)]
        IInstallationBehavior UninstallationBehavior { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020025)] get; }
        [DispId(0x60020026)]
        StringCollection UninstallationSteps { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020026)] get; }
        [DispId(0x60020028)]
        StringCollection KBArticleIDs { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020028)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020027)]
        void AcceptEula();
        [ComAliasName("WUApiLib.DeploymentAction"), DispId(0x60020029)]
        DeploymentAction DeploymentAction { [return: ComAliasName("WUApiLib.DeploymentAction")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020029)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002002a)]
        void CopyFromCache([In, MarshalAs(UnmanagedType.BStr)] string path, [In] bool toExtractCabFiles);
        [DispId(0x6002002b), ComAliasName("WUApiLib.DownloadPriority")]
        DownloadPriority DownloadPriority { [return: ComAliasName("WUApiLib.DownloadPriority")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002002b)] get; }
        [DispId(0x6002002c)]
        IUpdateDownloadContentCollection DownloadContents { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002002c)] get; }
    }

    [ComImport, TypeLibType((short)0x10d0), Guid("07F7438C-7709-4CA5-B518-91279288134E")]
    public interface IUpdateCollection : IEnumerable
    {
        [DispId(0)]
        IUpdate this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] set; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        int Add([In, MarshalAs(UnmanagedType.Interface)] IUpdate value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        void Clear();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        UpdateCollection Copy();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        void Insert([In] int index, [In, MarshalAs(UnmanagedType.Interface)] IUpdate value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        void RemoveAt([In] int index);
    }

    [ComImport, CoClass(typeof(UpdateCollectionClass)), Guid("07F7438C-7709-4CA5-B518-91279288134E")]
    public interface UpdateCollection : IUpdateCollection
    {
    }

    [ComImport, Guid("13639463-00DB-4646-803D-528026140D88"), TypeLibType((short)2), ClassInterface((short)0)]
    public class UpdateCollectionClass : IUpdateCollection, UpdateCollection, IEnumerable
    {
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        public virtual extern int Add([In, MarshalAs(UnmanagedType.Interface)] IUpdate value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        public virtual extern void Clear();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        public virtual extern UpdateCollection Copy();
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        public virtual extern IEnumerator GetEnumerator();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        public virtual extern void Insert([In] int index, [In, MarshalAs(UnmanagedType.Interface)] IUpdate value);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)]
        public virtual extern void RemoveAt([In] int index);

        // Properties
        [DispId(0x60020001)]
        public extern int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0)]
        public extern IUpdate this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] set; }
        [DispId(0x60020002)]
        public extern bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
    }

    [ComImport, Guid("91CAF7B0-EB23-49ED-9937-C52D817F46F7"), TypeLibType((short)0x10c0)]
    public interface IUpdateSession2 : IUpdateSession
    {
        [DispId(0x60020001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        WebProxy WebProxy { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        IUpdateSearcher CreateUpdateSearcher();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        UpdateDownloader CreateUpdateDownloader();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        IUpdateInstaller CreateUpdateInstaller();
        [DispId(0x60030001)]
        uint UserLocale { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] set; }
    }

    [ComImport, TypeLibType((short)0x10c0), DefaultMember("Name"), Guid("76B3B17E-AED6-4DA5-85F0-83587F81ABE3")]
    public interface IUpdateService
    {
        [DispId(0)]
        string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        object ContentValidationCert { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        DateTime ExpirationDate { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        bool IsManaged { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        bool IsRegisteredWithAU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        DateTime IssueDate { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        bool OffersWindowsUpdates { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [DispId(0x60020007)]
        StringCollection RedirectUrls { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] get; }
        [DispId(0x60020008)]
        string ServiceID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)] get; }
        [DispId(0x6002000a)]
        bool IsScanPackageService { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)] get; }
        [DispId(0x6002000b)]
        bool CanRegisterWithAU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)] get; }
        [DispId(0x6002000c)]
        string ServiceUrl { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000c)] get; }
        [DispId(0x6002000d)]
        string SetupPrefix { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)] get; }
    }

    [ComImport, TypeLibType((short)0x10c0), Guid("9B0353AA-0E52-44FF-B8B0-1F7FA0437F88")]
    public interface IUpdateServiceCollection : IEnumerable
    {
        [DispId(0)]
        IUpdateService this[int index] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4)]
        IEnumerator GetEnumerator();
        [DispId(0x60020001)]
        int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }

    [ComImport, Guid("23857E3C-02BA-44A3-9423-B1C900805F37"), TypeLibType((short)0x10c0)]
    public interface IUpdateServiceManager
    {
        [DispId(0x60020001)]
        IUpdateServiceCollection Services { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)]
        IUpdateService AddService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        void RegisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        void RemoveService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        void UnregisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        IUpdateService AddScanPackageService([In, MarshalAs(UnmanagedType.BStr)] string serviceName, [In, MarshalAs(UnmanagedType.BStr)] string scanFileLocation, [In, Optional, DefaultParameterValue(0)] int flags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60010007)]
        void SetOption([In, MarshalAs(UnmanagedType.BStr)] string optionName, [In, MarshalAs(UnmanagedType.Struct)] object optionValue);
    }

    [ComImport, Guid("1518B460-6518-4172-940F-C75883B24CEB"), TypeLibType((short)0x10c0), DefaultMember("Name")]
    public interface IUpdateService2 : IUpdateService
    {
        [DispId(0)]
        string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        object ContentValidationCert { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        DateTime ExpirationDate { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        bool IsManaged { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
        [DispId(0x60020004)]
        bool IsRegisteredWithAU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)] get; }
        [DispId(0x60020005)]
        DateTime IssueDate { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)] get; }
        [DispId(0x60020006)]
        bool OffersWindowsUpdates { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)] get; }
        [DispId(0x60020007)]
        StringCollection RedirectUrls { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020007)] get; }
        [DispId(0x60020008)]
        string ServiceID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020008)] get; }
        [DispId(0x6002000a)]
        bool IsScanPackageService { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000a)] get; }
        [DispId(0x6002000b)]
        bool CanRegisterWithAU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000b)] get; }
        [DispId(0x6002000c)]
        string ServiceUrl { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000c)] get; }
        [DispId(0x6002000d)]
        string SetupPrefix { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x6002000d)] get; }
        [DispId(0x60030001)]
        bool IsDefaultAUService { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] get; }
    }

    public enum UpdateServiceRegistrationState
    {
        usrsNotRegistered = 1,
        usrsRegistered = 3,
        usrsRegistrationPending = 2
    }

    [ComImport, DefaultMember("RegistrationState"), TypeLibType((short)0x10c0), Guid("DDE02280-12B3-4E0B-937B-6747F6ACB286")]
    public interface IUpdateServiceRegistration
    {
        [DispId(0), ComAliasName("WUApiLib.UpdateServiceRegistrationState")]
        UpdateServiceRegistrationState RegistrationState { [return: ComAliasName("WUApiLib.UpdateServiceRegistrationState")] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
        [DispId(0x60020001)]
        string ServiceID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [DispId(0x60020002)]
        bool IsPendingRegistrationWithAU { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        IUpdateService2 Service { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; }
    }

    [ComImport, TypeLibType((short)0x10d0), Guid("0BB8531D-7E8D-424F-986C-A0B8F60A3E7B")]
    public interface IUpdateServiceManager2 : IUpdateServiceManager
    {
        [DispId(0x60020001)]
        IUpdateServiceCollection Services { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)]
        IUpdateService AddService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        void RegisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        void RemoveService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        void UnregisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        IUpdateService AddScanPackageService([In, MarshalAs(UnmanagedType.BStr)] string serviceName, [In, MarshalAs(UnmanagedType.BStr)] string scanFileLocation, [In, Optional, DefaultParameterValue(0)] int flags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60010007)]
        void SetOption([In, MarshalAs(UnmanagedType.BStr)] string optionName, [In, MarshalAs(UnmanagedType.Struct)] object optionValue);
        [DispId(0x60030001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030002)]
        IUpdateServiceRegistration QueryServiceRegistration([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030003)]
        IUpdateServiceRegistration AddService2([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In] int flags, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
    }

    [ComImport, CoClass(typeof(UpdateServiceManagerClass)), Guid("0BB8531D-7E8D-424F-986C-A0B8F60A3E7B")]
    public interface UpdateServiceManager : IUpdateServiceManager2
    {
    }

    [ComImport, Guid("F8D253D9-89A4-4DAA-87B6-1168369F0B21"), TypeLibType((short)2), ClassInterface((short)0)]
    public class UpdateServiceManagerClass : IUpdateServiceManager2, UpdateServiceManager
    {
        // Methods
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        public virtual extern IUpdateService AddScanPackageService([In, MarshalAs(UnmanagedType.BStr)] string serviceName, [In, MarshalAs(UnmanagedType.BStr)] string scanFileLocation, [In, Optional, DefaultParameterValue(0)] int flags);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)]
        public virtual extern IUpdateService AddService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030003)]
        public virtual extern IUpdateServiceRegistration AddService2([In, MarshalAs(UnmanagedType.BStr)] string ServiceID, [In] int flags, [In, MarshalAs(UnmanagedType.BStr)] string authorizationCabPath);
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030002)]
        public virtual extern IUpdateServiceRegistration QueryServiceRegistration([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)]
        public virtual extern void RegisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        public virtual extern void RemoveService([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60010007)]
        public virtual extern void SetOption([In, MarshalAs(UnmanagedType.BStr)] string optionName, [In, MarshalAs(UnmanagedType.Struct)] object optionValue);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        public virtual extern void UnregisterServiceWithAU([In, MarshalAs(UnmanagedType.BStr)] string ServiceID);

        // Properties
        [DispId(0x60030001)]
        public extern string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] set; }
        [DispId(0x60020001)]
        public extern IUpdateServiceCollection Services { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; }
    }

    [ComImport, Guid("918EFD1E-B5D8-4C90-8540-AEB9BDC56F9D"), TypeLibType((short)0x10d0)]
    public interface IUpdateSession3 : IUpdateSession2
    {
        [DispId(0x60020001)]
        string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60020003)]
        WebProxy WebProxy { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        IUpdateSearcher CreateUpdateSearcher();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        UpdateDownloader CreateUpdateDownloader();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        IUpdateInstaller CreateUpdateInstaller();
        [DispId(0x60030001)]
        uint UserLocale { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] set; }
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60040001)]
        UpdateServiceManager CreateUpdateServiceManager();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60040002)]
        IUpdateHistoryEntryCollection QueryHistory([In, MarshalAs(UnmanagedType.BStr)] string criteria, [In] int startIndex, [In] int Count);
    }

    [ComImport, CoClass(typeof(UpdateSessionClass)), Guid("918EFD1E-B5D8-4C90-8540-AEB9BDC56F9D")]
    public interface UpdateSession : IUpdateSession3
    {
    }

    [ComImport, TypeLibType((short)2), ClassInterface((short)0), Guid("4CB43D7F-7EEE-4906-8698-60DA1C38F2FE")]
    public class UpdateSessionClass : IUpdateSession3, UpdateSession
    {
        // Methods
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020005)]
        public virtual extern UpdateDownloader CreateUpdateDownloader();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020006)]
        public virtual extern IUpdateInstaller CreateUpdateInstaller();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020004)]
        public virtual extern IUpdateSearcher CreateUpdateSearcher();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60040001)]
        public virtual extern UpdateServiceManager CreateUpdateServiceManager();
        [return: MarshalAs(UnmanagedType.Interface)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60040002)]
        public virtual extern IUpdateHistoryEntryCollection QueryHistory([In, MarshalAs(UnmanagedType.BStr)] string criteria, [In] int startIndex, [In] int Count);

        // Properties
        [DispId(0x60020001)]
        public extern string ClientApplicationID { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020001)] set; }
        [DispId(0x60020002)]
        public extern bool ReadOnly { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020002)] get; }
        [DispId(0x60030001)]
        public extern uint UserLocale { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60030001)] set; }
        [DispId(0x60020003)]
        public extern WebProxy WebProxy { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] get; [param: In, MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x60020003)] set; }
    }
}
