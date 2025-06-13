using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// Interface to a handle opened by the UEFI Simple File System protocol
    /// (<c>EFI_FILE_PROTOCOL</c>).  
    /// All functions use the Microsoft x64 ABI (<c>extern "C"</c> / <see langword="unmanaged"/>) and
    /// return an <c>EFI_STATUS</c> (<see cref="ulong"/>).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct EFI_FILE_PROTOCOL
    {
        /// <summary>
        /// Protocol revision (should be <c>0x00010000</c> or newer).
        /// </summary>
        public ulong Revision;

        /// <summary>
        /// Opens or creates a child file/directory relative to this node.
        /// </summary>
        /// <param>The current handle (implicit *<c>this</c>*).</param>
        /// <param>Receives the opened child handle.</param>
        /// <param>UTF-16 path relative to <paramref/>.</param>
        /// <param>Combination of <see cref="EFI_FILE_MODE"/> flags.</param>
        /// <param>File attributes when <c>Create</c> is used.</param>
        /// <returns><c>EFI_SUCCESS</c> or error status.</returns>
        public delegate* unmanaged<
            EFI_FILE_PROTOCOL* /*This*/,
            EFI_FILE_PROTOCOL** /*NewHandle*/,
            char* /*FileName*/,
            EFI_FILE_MODE /*OpenMode*/,
            ulong /*Attributes*/,
            ulong /*EFI_STATUS*/> Open;

        /// <summary>
        /// Closes the handle and frees any associated resources.
        /// </summary>
        public delegate* unmanaged<
            EFI_FILE_PROTOCOL* /*This*/,
            ulong /*EFI_STATUS*/> Close;

        /// <summary>
        /// Reads up to <paramref /> bytes from the current file
        /// position into <paramref /> and updates the position.
        /// </summary>
        public delegate* unmanaged<
            EFI_FILE_PROTOCOL* /*This*/,
            ulong* /*BufferSize IN/OUT*/,
            void* /*Buffer*/,
            ulong /*EFI_STATUS*/> Read;

        /// <summary>
        /// Moves the file pointer to the specified byte offset.
        /// </summary>
        public delegate* unmanaged<
            EFI_FILE_PROTOCOL* /*This*/,
            ulong /*Position*/,
            ulong /*EFI_STATUS*/> SetPosition;

        /// <summary>
        /// Returns the current byte offset of the file pointer.
        /// </summary>
        public delegate* unmanaged<
            EFI_FILE_PROTOCOL* /*This*/,
            ulong* /*Position*/,
            ulong /*EFI_STATUS*/> GetPosition;

        public delegate* unmanaged<
            EFI_FILE_PROTOCOL* /*This*/,
            EFI_GUID* /*InformationType*/,
            ulong* /*BufferSize IN/OUT*/,
            void* /*Buffer*/,
            ulong /*EFI_STATUS*/> GetInfo;
    }
}
