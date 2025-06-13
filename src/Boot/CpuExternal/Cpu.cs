using System.Runtime.CompilerServices;

namespace AdrenalineOs.Boot.CpuExternal;

/// <summary>
/// Low-level helper for manipulating x86-64 control registers.
/// 
///     • CR3 ← pml4Phys   – installs the caller-supplied PML4 as the
///       top-level page-map, defining virtual→physical translations.
///
///     • CR4.PAE = 1       – turns on Physical-Address Extension,
///       a prerequisite for 4-level paging on long-mode capable CPUs.
///
///     • CR0.{PG, WP, PE} = 1 – enables paging, write-protect and
///       protected mode, completing the transition to paged protected mode.
///
/// After <see cref="EnablePaging"/> returns, the MMU is live and the
/// processor executes with the provided page tables.  Interrupts
/// should remain disabled during the switch (use <see cref="Sti"/>
/// afterwards if you intend to re-enable them).
/// </summary>
internal unsafe static class Cpu
{
    /// <summary>
    /// Activates 4-level paging.
    /// Steps:
    /// 1. Load the physical address of a valid, 4 KiB-aligned PML4 into CR3.
    /// 2. Set CR4.PAE.
    /// 3. Set CR0.PG | CR0.WP | CR0.PE.
    /// </summary>
    /// <param name="pml4Phys">
    /// Physical address of the top-level page-map (PML4).
    /// </param>
    public static void EnablePaging(void* pml4Phys)
    {
        WriteCr3((ulong)pml4Phys);
        SetCr4Pae();
        SetCr0PgWpPe();
    }

    // ── Raw register helpers provided by external assembly stubs ──────────────
    [MethodImpl(MethodImplOptions.ForwardRef)] private static extern void WriteCr3(ulong value);
    [MethodImpl(MethodImplOptions.ForwardRef)] private static extern void SetCr4Pae();
    [MethodImpl(MethodImplOptions.ForwardRef)] private static extern void SetCr0PgWpPe();
    [MethodImpl(MethodImplOptions.ForwardRef)] public static extern void Sti(); // enable interrupts
}

