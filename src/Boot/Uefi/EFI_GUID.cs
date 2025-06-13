using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// 128-bit Globally Unique Identifier (GUID) in UEFI byte layout:
    /// {Data1-Data2-Data3-Data4[0-7]} ⇢ 8-4-4-4-12.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct EFI_GUID
    {
        /// <summary>Most-significant 32 bits.</summary>
        public uint Data1;

        /// <summary>Next 16 bits.</summary>
        public ushort Data2;

        /// <summary>Next 16 bits.</summary>
        public ushort Data3;

        /// <summary>Final 64-bit field, byte 0.</summary>
        public byte Data4_0;
        /// <summary>Final 64-bit field, byte 1.</summary>
        public byte Data4_1;
        /// <summary>Final 64-bit field, byte 2.</summary>
        public byte Data4_2;
        /// <summary>Final 64-bit field, byte 3.</summary>
        public byte Data4_3;
        /// <summary>Final 64-bit field, byte 4.</summary>
        public byte Data4_4;
        /// <summary>Final 64-bit field, byte 5.</summary>
        public byte Data4_5;
        /// <summary>Final 64-bit field, byte 6.</summary>
        public byte Data4_6;
        /// <summary>Final 64-bit field, byte 7.</summary>
        public byte Data4_7;
    }
}
