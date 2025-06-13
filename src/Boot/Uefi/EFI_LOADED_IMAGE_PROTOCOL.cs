namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// Descriptor supplied by UEFI for the PE/COFF image that is currently
    /// loaded and executing.
    /// </summary>
    internal unsafe struct EFI_LOADED_IMAGE_PROTOCOL
    {
        /// <summary>Protocol version.</summary>
        public uint Revision;

        /// <summary>
        /// Handle of the image that loaded this one, or <c>NULL</c> if this is
        /// the root image.
        /// </summary>
        public void* ParentHandle;

        /// <summary>Pointer to the active <c>EFI_SYSTEM_TABLE</c>.</summary>
        public void* SystemTable;

        /// <summary>
        /// Firmware handle for the device the image was read from
        /// (used to locate other protocols such as the file-system).
        /// </summary>
        public void* DeviceHandle;
    }
}
