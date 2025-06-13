namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// File-access flags for <c>EFI_FILE_PROTOCOL.Open</c>.
    /// These values can be combined with bitwise OR.
    /// </summary>
    internal enum EFI_FILE_MODE : ulong
    {
        /// <summary>
        /// Open an existing file for read-only access.<br/>
        /// The call fails if the file does not exist.
        /// </summary>
        Read = 0x0000_0000_0000_0001,

        /// <summary>
        /// Open an existing file for write access.<br/>
        /// The call fails if the file does not exist.
        /// </summary>
        Write = 0x0000_0000_0000_0002,

        /// <summary>
        /// Create a new file if it does not exist; otherwise truncate and open it.<br/>
        /// When combined with <see cref="Read"/> and/or <see cref="Write"/>,
        /// the new file is opened with the specified access.
        /// </summary>
        Create = 0x8000_0000_0000_0000
    }
}
