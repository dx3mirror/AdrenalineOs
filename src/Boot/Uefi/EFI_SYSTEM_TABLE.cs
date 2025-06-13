using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Uefi
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct EFI_SYSTEM_TABLE
    {
        public fixed byte _pad1[48];
        public void* ConIn;
        public void* ConOut;
        public void* StdErr;
        public EFI_RUNTIME_SERVICES* RuntimeServices;
        public EFI_BOOT_SERVICES* BootServices;
    }
}
