namespace AdrenalineOs.Boot.Uefi
{
    internal unsafe struct EFI_SIMPLE_FILE_SYSTEM_PROTOCOL
    {
        public ulong Revision;
        public delegate* unmanaged<
            EFI_SIMPLE_FILE_SYSTEM_PROTOCOL*, EFI_FILE_PROTOCOL**, ulong> OpenVolume;
    }
}
