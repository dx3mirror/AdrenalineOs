namespace AdrenalineOs.Boot.Uefi
{
    internal static class UefiGuids
    {
        public static readonly EFI_GUID LoadedImage = new()
        {
            Data1 = 0x5B1B31A1,
            Data2 = 0x9562,
            Data3 = 0x11D2,
            Data4_0 = 0x8E,
            Data4_1 = 0x3F,
            Data4_2 = 0x00,
            Data4_3 = 0xA0,
            Data4_4 = 0xC9,
            Data4_5 = 0x69,
            Data4_6 = 0x72,
            Data4_7 = 0x3B
        };

        public static readonly EFI_GUID SimpleFileSystem = new()
        {
            Data1 = 0x0964E5B2,
            Data2 = 0x6459,
            Data3 = 0x11D2,
            Data4_0 = 0x8E,
            Data4_1 = 0x39,
            Data4_2 = 0x00,
            Data4_3 = 0xA0,
            Data4_4 = 0xC9,
            Data4_5 = 0x69,
            Data4_6 = 0x72,
            Data4_7 = 0x3B
        };

        public static readonly EFI_GUID FileInfo = new()
        {
            Data1 = 0x09576E92,
            Data2 = 0x6D3F,
            Data3 = 0x11D2,
            Data4_0 = 0x8E,
            Data4_1 = 0x39,
            Data4_2 = 0x00,
            Data4_3 = 0xA0,
            Data4_4 = 0xC9,
            Data4_5 = 0x69,
            Data4_6 = 0x72,
            Data4_7 = 0x3B
        };
    }
}
