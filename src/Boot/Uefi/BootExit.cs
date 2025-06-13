using AdrenalineOs.Boot.Memory;

namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// Captures the final UEFI memory map into a freshly-allocated buffer and
    /// transitions firmware control to the kernel by calling
    /// <c>ExitBootServices</c>.
    ///
    /// <para>Algorithm</para>
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       Query the required buffer size and descriptor stride with an
    ///       initial <c>GetMemoryMap</c> call (size parameter set to 0).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Inflate the size by two descriptor slots — UEFI recommends a
    ///       margin because the map may grow between calls — then allocate
    ///       whole pages via <see cref="PageAllocator.AllocMany"/>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Retrieve the authoritative map, receiving a monotonic
    ///       changed.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Invoke <c>ExitBootServices</c>; from this point on all firmware
    ///       services are unavailable and the system is under the OS’s
    ///       control.
    ///     </description>
    ///   </item>
    /// </list>
    /// The tuple returned provides:
    /// <c>(pointer to map, total size in bytes, descriptor size)</c>.
    /// </summary>
    internal unsafe static class BootExit
    {
        public static (IntPtr map, ulong mapSize, ulong descSize)
            CollectMapAndExit(EFI_SYSTEM_TABLE* st, void* imageHandle)
        {
            nuint sz = 0;
            nuint descSize;                       

            st->BootServices->GetMemoryMap(&sz, null, null, &descSize, null);

            sz += 2 * descSize;                   
            nuint pages = (sz + 0xFFF) >> 12;
            void* raw = PageAllocator.AllocMany(pages);

            nuint key;
            st->BootServices->GetMemoryMap(&sz,
                                           (EFI_MEMORY_DESCRIPTOR*)raw,
                                           &key,
                                           &descSize,
                                           null);

            st->BootServices->ExitBootServices(imageHandle, key);

            return ((IntPtr)raw, sz, descSize);
        }
    }
}
