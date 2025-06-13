using AdrenalineOs.Boot.Memory;
using AdrenalineOs.Boot.Uefi;

namespace AdrenalineOs.Boot.Elf
{
    /// <summary>
    /// Allocates fresh physical pages and stages every <c>PT_LOAD</c> segment
    /// of a 64-bit ELF image into memory.
    /// <para>
    /// Steps per segment:
    /// </para>
    /// <list type="number">
    ///   <item>
    ///     <description>Compute the number of 4 KiB pages required:
    ///     <c>pages = (Memsz + 0xFFF) &gt;&gt; 12</c>.</description>
    ///   </item>
    ///   <item>
    ///     <description>Request that many pages from
    ///     <see cref="PageAllocator.AllocMany"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>Seek to <c>ph[i].Offset</c> in the <see cref="EFI_FILE_PROTOCOL"/>
    ///     and copy <c>ph[i].Filesz</c> bytes into the newly-allocated buffer.</description>
    ///   </item>
    /// </list>
    /// Uninitialised space (<c>Memsz &gt; Filesz</c>) is left as
    /// zero-filled pages supplied by the allocator.  
    /// The method returns the first address after the highest staged byte so
    /// the caller can know the extent of the loaded image.
    /// </summary>
    internal static unsafe class PageStager
    {
        /// <summary>
        /// Loads all <c>PT_LOAD</c> segments described by the program-header
        /// array <paramref name="ph"/>, copying data from <paramref name="src"/>.
        /// </summary>
        /// <param name="ph">Pointer to the first <see cref="Elf64_Phdr"/>.</param>
        /// <param name="count">Number of entries in the table.</param>
        /// <param name="src">
        /// Open EFI file handle positioned at the start of the ELF image.
        /// </param>
        /// <returns>
        /// Address immediately past the highest byte of the last segment
        /// loaded (exclusive upper bound of the staged image).
        /// </returns>
        public static void* Load(Elf64_Phdr* ph, ushort count, EFI_FILE_PROTOCOL* src)
        {
            void* highest = null;

            for (int i = 0; i < count; i++)
            {
                if (ph[i].Type != 1 /* PT_LOAD */)
                    continue;

                // Round up to the next 4 KiB page boundary.
                ulong pages = (ph[i].Memsz + 0xFFF) >> 12;

                // Allocate physical pages for the segment.
                void* dest = PageAllocator.AllocMany(pages);

                // Copy the initialised part of the segment from the file.
                src->SetPosition(src, ph[i].Offset);
                src->Read(src, &ph[i].Filesz, dest);

                // Track the highest mapped byte.
                highest = (byte*)dest + ph[i].Memsz;
            }

            return highest; // exclusive end of the staged image
        }
    }
}
