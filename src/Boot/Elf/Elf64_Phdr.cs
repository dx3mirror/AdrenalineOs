namespace AdrenalineOs.Boot.Elf
{
    /// <summary>
    /// 64-bit ELF program-header entry (<c>Elf64_Phdr</c>) as it appears in the
    /// file’s program-header table.
    /// <para>
    /// Each entry describes a contiguous segment of the image that the
    /// loader must map into memory.
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <term><see cref="Type"/></term>
    ///     <description>
    ///       Segment type = <c>PT_LOAD</c>,
    ///       <c>2</c> = <c>PT_DYNAMIC</c>).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Flags"/></term>
    ///     <description>
    ///       Access flags (<c>PF_X</c>, <c>PF_W</c>, <c>PF_R</c>).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Offset"/></term>
    ///     <description>
    ///       Byte offset in the file where the segment starts.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Vaddr"/></term>
    ///     <description>
    ///       Virtual address at which to map the segment.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Filesz"/></term>
    ///     <description>
    ///       Number of bytes to copy from file to memory.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Memsz"/></term>
    ///     <description>
    ///       Total size of the segment in memory (may exceed
    ///       <see cref="Filesz"/> for BSS-style zero fill).
    ///     </description>
    ///   </item>
    /// </list>
    /// The bootloader iterates over the <c>PhNum</c> entries in the
    /// program-header table (starting at <c>PhOff</c> in the ELF header) and
    /// loads each <c>PT_LOAD</c> segment into memory accordingly.
    /// </summary>
    internal unsafe struct Elf64_Phdr
    {
        public uint Type;    // Segment type
        public uint Flags;   // Access flags
        public ulong Offset;  // File offset
        public ulong Vaddr;   // Virtual address
        public ulong Filesz;  // Size in file
        public ulong Memsz;   // Size in memory
    }
}
