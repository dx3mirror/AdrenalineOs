namespace AdrenalineOs.Boot.Elf
{
    /// <summary>
    /// Minimal 64-bit ELF file header (<c>Elf64_Ehdr</c>) as laid out on disk.
    /// <para>
    /// Only the fields required by the AdrenalineOS bootloader are kept:
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <term><see cref="Ident"/></term>
    ///     <description>
    ///       16-byte identification array beginning with the magic
    ///       <c>0x7F 'E' 'L' 'F'</c>, followed by class, data model,
    ///       version, ABI, etc.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Type"/></term>
    ///     <description>
    ///       Object-file type (e.g. <c>2</c> = <c>ET_EXEC</c>,
    ///       <c>3</c> = <c>ET_DYN</c>).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Entry"/></term>
    ///     <description>
    ///       Virtual address of the first instruction to run after the image
    ///       has been loaded.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="PhOff"/></term>
    ///     <description>
    ///       File offset (in bytes) to the program-header table.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="PhNum"/></term>
    ///     <description>
    ///       Number of <c>Elf64_Phdr</c> entries located at
    ///       <see cref="PhOff"/>.
    ///     </description>
    ///   </item>
    /// </list>
    /// This structure is unpacked directly from the binary, so it must be
    /// accessed with <c>unsafe</c> code and interpreted as little-endian on
    /// x86-64 platforms.
    /// </summary>
    internal unsafe struct Elf64_Ehdr
    {
        /// <summary>ELF magic and identification bytes (size = 16).</summary>
        public fixed byte Ident[16];

        /// <summary>Object-file type (ET_EXEC, ET_DYN, …).</summary>
        public ushort Type;

        /// <summary>Entry-point virtual address.</summary>
        public ulong Entry;

        /// <summary>Byte offset to the program-header table.</summary>
        public ulong PhOff;

        /// <summary>Number of entries in the program-header table.</summary>
        public ushort PhNum;
    }
}
