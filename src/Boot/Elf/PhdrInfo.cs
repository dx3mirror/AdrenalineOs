namespace AdrenalineOs.Boot.Elf
{
    /// <summary>
    /// Immutable view returned by the ELF loader that identifies the program-header
    /// table and entry point of a 64-bit ELF image.
    /// <para>
    ///     • <see cref="Table"/>  – pointer to the first <see cref="Elf64_Phdr"/>.<br/>
    ///     • <see cref="Count"/>  – number of program-header entries.<br/>
    ///     • <see cref="Entry"/>  – virtual address at which execution should begin.
    /// </para>
    /// </summary>
    internal unsafe struct PhdrInfo
    {
        /// <summary>
        /// Pointer to the program-header array (<c>Elf64_Phdr[count]</c>).
        /// </summary>
        public Elf64_Phdr* Table;

        /// <summary>
        /// Number of entries in the program-header table.
        /// </summary>
        public ushort Count;

        /// <summary>
        /// ELF image entry-point virtual address.
        /// </summary>
        public ulong Entry;
    }
}
