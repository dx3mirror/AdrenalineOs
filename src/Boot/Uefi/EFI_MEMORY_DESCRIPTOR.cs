using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Uefi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct EFI_MEMORY_DESCRIPTOR
    {
        public uint Type;
        public uint Pad;              
        public ulong PhysicalStart;
        public ulong VirtualStart;
        public ulong NumberOfPages;
        public ulong Attribute;
    }
}
