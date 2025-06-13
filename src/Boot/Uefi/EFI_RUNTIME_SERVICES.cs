namespace AdrenalineOs.Boot.Uefi
{
    internal unsafe struct EFI_RUNTIME_SERVICES
    {
        private fixed byte _pad[40]; 
        public delegate* unmanaged<EFI_TIME*, void*, ulong> GetTime;
    }
}
