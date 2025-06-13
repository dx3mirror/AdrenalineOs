using AdrenalineOs.Boot.MemMap;

namespace AdrenalineOs.Boot.Heap;

/// <summary>
/// Tiny fixed-block kernel allocator backed by 4 KiB pages.
/// <para>
/// Blocks are bucketed into <c>Classes = 10</c> size classes:
/// • Requests ≤ 4 KiB are rounded up to the next power-of-two bucket and
///   satisfied from an intra-page free list.<br/>
/// • Larger requests fall through to the page allocator
///   (<see cref="BootMem.AllocFrame"/>).
/// </para>
/// </summary>
internal static unsafe class KHeap
{
    private const int MinShift = 3;          // smallest bucket 2^3  = 8 B
    private const int MaxShift = 12;         // largest  bucket 2^12 = 4096 B
    private const int Classes = MaxShift - MinShift + 1;

    /// <summary>
    /// Array of <c>Classes</c> singly-linked free lists; each node’s first
    /// machine word stores the next pointer.
    /// </summary>
    private static void** _freeLists;

    /// <summary>Initialises the bucket heads and reserves storage for them.</summary>
    public static void Init()
    {
        _freeLists = (void**)BootMem.AllocFrame(); // 4 KiB → plenty for 10 ptrs
        for (int i = 0; i < Classes; i++)
            _freeLists[i] = null;
    }

    /// <summary>
    /// Allocates <paramref name="size"/> bytes.
    /// </summary>
    /// <remarks>
    /// Returns <c>null</c> for <c>size == 0</c>.  
    /// For requests &gt; 4096 B the call delegates directly to the page
    /// allocator and hands out whole frames.
    /// </remarks>
    public static void* Alloc(nuint size)
    {
        if (size == 0) return null;

        int cls = SizeToClass(size);
        if (cls < 0)
            return BootMem.AllocFrame();               // huge request

        if (_freeLists[cls] == null)
            CarveNewPage(cls);                         // populate bucket

        void* block = _freeLists[cls];
        _freeLists[cls] = *(void**)_freeLists[cls];     // pop
        return block;
    }

    /// <summary>
    /// Frees a block previously returned by <see cref="Alloc"/>.
    /// </summary>
    public static void Free(void* ptr, nuint size)
    {
        int cls = SizeToClass(size);
        if (cls < 0) { BootMem.FreeFrame(ptr); return; }

        *(void**)ptr = _freeLists[cls];            // push
        _freeLists[cls] = ptr;
    }

    /// <summary>
    /// Maps a size to its bucket index; returns −1 when the size exceeds
    /// the largest bucket.
    /// </summary>
    private static int SizeToClass(nuint sz)
    {
        uint s = (uint)(sz - 1);
        int p = 0;
        while (s > 0) { s >>= 1; p++; }                // ceil(log₂(sz))
        int cls = p - MinShift;
        return (cls >= 0 && cls < Classes) ? cls : -1;
    }

    /// <summary>Splits a fresh 4 KiB frame into blocks of the given class.</summary>
    private static void CarveNewPage(int cls)
    {
        nuint blk = (nuint)1 << (cls + MinShift);     // block size
        byte* page = (byte*)BootMem.AllocFrame();

        for (nuint off = 0; off + blk <= 4096; off += blk)
        {
            *(void**)(page + off) = _freeLists[cls];
            _freeLists[cls] = page + off;
        }
    }
}
