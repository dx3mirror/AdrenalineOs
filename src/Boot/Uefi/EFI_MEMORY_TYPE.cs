namespace AdrenalineOs.Boot.Uefi
{
    internal enum EFI_MEMORY_TYPE : uint
    {
        Reserved = 0,
        LoaderCode = 1,
        LoaderData = 2,
        BootServicesCode = 3,
        BootServicesData = 4,
        RuntimeServicesCode = 5,
        RuntimeServicesData = 6,
        ConventionalMemory = 7,
    }
}
