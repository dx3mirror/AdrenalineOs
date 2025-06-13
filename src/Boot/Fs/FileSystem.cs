using AdrenalineOs.Boot.Uefi;

namespace AdrenalineOs.Boot.Fs
{
    /// <summary>
    /// Thin convenience layer around the EFI Simple File System protocol that
    /// locates and opens the AdrenalineOS kernel image
    /// (<c>\EFI\ADRENALINE\KERNEL.ELF</c>) on the same device that
    /// the bootloader itself was loaded from.
    /// </summary>
    internal static unsafe class FileSystem
    {
        /// <summary>
        /// Opens the kernel ELF file and returns an <see cref="EFI_FILE_PROTOCOL"/>
        /// handle ready for sequential reading.
        /// </summary>
        /// <param name="image">
        /// Handle of the running PE/COFF bootloader image (as passed to
        /// <c>efi_main</c>).
        /// </param>
        /// <param name="st">
        /// Pointer to the EFI system table supplied by the firmware.
        /// </param>
        /// <param name="fileSize">
        /// On success receives the file’s exact size in bytes.
        /// </param>
        /// <returns>
        /// An open, read-only file handle for <c>KERNEL.ELF</c>; never <c>null</c>
        /// if the call succeeds.
        /// </returns>
        public static EFI_FILE_PROTOCOL* OpenKernel(
            void* image,
            EFI_SYSTEM_TABLE* st,
            out ulong fileSize)
        {
            // --- Resolve EFI protocols --------------------------------------------------
            EFI_LOADED_IMAGE_PROTOCOL* li;
            var gLi = UefiGuids.LoadedImage;
            st->BootServices->HandleProtocol(image, &gLi, (void**)&li);

            var gSfs = UefiGuids.SimpleFileSystem;
            EFI_SIMPLE_FILE_SYSTEM_PROTOCOL* fs;
            st->BootServices->HandleProtocol(li->DeviceHandle, &gSfs, (void**)&fs);

            // --- Navigate to \EFI\ADRENALINE\KERNEL.ELF -------------------------------
            EFI_FILE_PROTOCOL* root;
            fs->OpenVolume(fs, &root);

            fixed (char* path = @"\EFI\ADRENALINE\KERNEL.ELF")
                root->Open(root, &root, path, EFI_FILE_MODE.Read, 0);

            // --- Query file metadata ----------------------------------------------------
            EFI_FILE_INFO* info = FileInfo(root, st);
            fileSize = info->FileSize;

            return root;
        }

        /// <summary>
        /// Allocates a buffer, fills it with <see cref="EFI_FILE_INFO"/> for
        /// <paramref name="file"/>, and returns a typed pointer to the data.
        /// </summary>
        private static EFI_FILE_INFO* FileInfo(
            EFI_FILE_PROTOCOL* file,
            EFI_SYSTEM_TABLE* st)
        {
            ulong sz = 0;
            var gInfo = UefiGuids.FileInfo;

            // First call obtains required buffer size.
            file->GetInfo(file, &gInfo, &sz, null);

            // Allocate from boot-services pool.
            void* buf;
            st->BootServices->AllocatePool(
                EFI_MEMORY_TYPE.BootServicesData,
                (nuint)sz,
                &buf);

            // Second call retrieves the structure.
            file->GetInfo(file, &gInfo, &sz, buf);
            return (EFI_FILE_INFO*)buf;
        }
    }
}
