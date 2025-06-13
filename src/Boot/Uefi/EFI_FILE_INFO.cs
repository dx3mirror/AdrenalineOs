using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// Metadata returned by EFI_FILE_PROTOCOL.GetInfo (GUID <c>EFI_FILE_INFO</c>).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EFI_FILE_INFO
    {
        /// <summary>Total size of this structure, including <see cref="FileName"/>.</summary>
        public ulong Size;

        /// <summary>Logical length of the file in bytes.</summary>
        public ulong FileSize;

        /// <summary>Allocated space on the volume (may be larger than <see cref="FileSize"/>).</summary>
        public ulong PhysicalSize;

        private fixed byte CreateTime[24];       // EFI_TIME (creation)
        private fixed byte LastAccessTime[24];   // EFI_TIME (last read)
        private fixed byte ModificationTime[24]; // EFI_TIME (last write)

        /// <summary>EFI_FILE_ATTRIBUTE_* bit-mask.</summary>
        public ulong Attribute;

        /// <summary>Null-terminated UTF-16 file name (flexible array).</summary>
        public fixed char FileName[1];
    }
}
