namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// Subset of EFI Boot Services used by the AdrenalineOS loader.
    /// Only the members referenced by the loader are exposed; all other
    /// fields are represented by padding placeholders to preserve layout.
    /// </summary>
    internal unsafe struct EFI_BOOT_SERVICES
    {
        private fixed byte _pad[56]; // EFI_TABLE header (unused)

        /// <summary>
        /// Starts, resets, or disables the watchdog timer.
        /// </summary>
        public delegate* unmanaged<
            ulong /*timeout*/,
            ulong /*watchdogCode*/,
            ulong /*dataSize*/,
            char* /*data*/,
            ulong /*EFI_STATUS*/> SetWatchdogTimer;

        private fixed byte _pad0[24 + 16]; // RaiseTPL / RestoreTPL (unused)

        /// <summary>
        /// Allocates one or more contiguous 4 KiB pages.
        /// </summary>
        public delegate* unmanaged<
            EFI_ALLOCATE_TYPE /*type*/,
            EFI_MEMORY_TYPE   /*memType*/,
            nuint             /*pages*/,
            void**            /*memory*/,
            ulong             /*EFI_STATUS*/> AllocatePages;

        /// <summary>
        /// Frees pages previously obtained from <see cref="AllocatePages"/>.
        /// </summary>
        public delegate* unmanaged<
            void*  /*memory*/,
            nuint  /*pages*/,
            ulong  /*EFI_STATUS*/> FreePages;

        private fixed byte _pad1[0x38]; // LocateHandle et al. (unused)

        /// <summary>
        /// Retrieves an interface from a handle that supports the given GUID.
        /// </summary>
        public delegate* unmanaged<
            void*     /*handle*/,
            EFI_GUID* /*protocol*/,
            void**    /*interface*/,
            ulong     /*EFI_STATUS*/> HandleProtocol;

        /// <summary>
        /// Allocates a pool buffer of the specified type and size.
        /// </summary>
        public delegate* unmanaged<
            EFI_MEMORY_TYPE /*memType*/,
            nuint           /*size*/,
            void**          /*buffer*/,
            ulong           /*EFI_STATUS*/> AllocatePool;

        /// <summary>
        /// Returns a buffer allocated with <see cref="AllocatePool"/> to the firmware.
        /// </summary>
        public delegate* unmanaged<
            void* /*buffer*/,
            ulong /*EFI_STATUS*/> FreePool;

        /// <summary>
        /// Retrieves the current memory map.
        /// </summary>
        public delegate* unmanaged<
            nuint*               /*memoryMapSize*/,
            EFI_MEMORY_DESCRIPTOR* /*memoryMap*/,
            nuint*               /*mapKey*/,
            nuint*               /*descriptorSize*/,
            uint*                /*descriptorVersion*/,
            ulong                /*EFI_STATUS*/> GetMemoryMap;

        /// <summary>
        /// Terminates boot services and hands control to the operating system.
        /// </summary>
        public delegate* unmanaged<
            void* /*imageHandle*/,
            nuint /*mapKey*/,
            ulong /*EFI_STATUS*/> ExitBootServices;
    }
}
