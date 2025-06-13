namespace AdrenalineOs.Boot.Uefi
{
    /// <summary>
    /// Placement hint for EFI page allocations.
    /// </summary>
    internal enum EFI_ALLOCATE_TYPE : uint
    {
        /// <summary>Allocate anywhere.</summary>
        AnyPages = 0,

        /// <summary>Allocate at or below the given maximum address.</summary>
        MaxAddress = 1,

        /// <summary>Allocate exactly at the given address.</summary>
        Address = 2,
    }
}
