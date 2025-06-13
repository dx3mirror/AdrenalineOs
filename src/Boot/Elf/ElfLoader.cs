namespace AdrenalineOs.Boot.Elf
{
    /// <summary>
    /// Minimal helper that converts an in-memory ELF image into a
    /// <see cref="PhdrInfo"/> view, exposing the program-header table
    /// needed by the rest of the loader.
    /// </summary>
    internal static unsafe class ElfLoader
    {
        /// <summary>
        /// Parses the ELF header at <paramref name="file"/> and returns the
        /// location and size of the program-header array plus the image’s
        /// entry point.
        /// </summary>
        /// <param name="file">
        /// Pointer to the start of a 64-bit ELF file mapped in memory.
        /// </param>
        /// <returns>
        /// A <see cref="PhdrInfo"/> with:
        /// <list type="bullet">
        ///   <item><description><c>Table</c> – first <see cref="Elf64_Phdr"/>.</description></item>
        ///   <item><description><c>Count</c> – number of program-header entries.</description></item>
        ///   <item><description><c>Entry</c> – virtual entry-point address.</description></item>
        /// </list>
        /// </returns>
        public static PhdrInfo Parse(void* file)
        {
            var hdr = (Elf64_Ehdr*)file;
            return new PhdrInfo
            {
                Table = (Elf64_Phdr*)((byte*)file + hdr->PhOff),
                Count = hdr->PhNum,
                Entry = hdr->Entry
            };
        }
    }
}
