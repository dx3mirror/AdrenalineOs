using System.Runtime.CompilerServices;

namespace AdrenalineOs.Boot.CpuExternal
{
    /// <summary>
    /// Thin managed wrapper around the <c>RDMSR</c> and <c>WRMSR</c> 
    /// instructions, enabling direct access to 64-bit Model-Specific 
    /// Registers (MSRs).
    /// <para>
    ///     • <see cref="Read"/>  – executes <c>RDMSR</c>, returning the 
    ///       64-bit value in <c>EDX:EAX</c>.<br/>
    ///     • <see cref="Write"/> – executes <c>WRMSR</c>, writing the 
    ///       provided 64-bit value from <c>EDX:EAX</c>.
    /// </para>
    /// Use only at the highest privilege level (CPL 0); the caller must ensure
    /// the specified MSR index is valid for the executing CPU.
    /// </summary>
    internal static class Msr
    {
        /// <summary>
        /// Reads the 64-bit value of the MSR identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The MSR index to read (placed in <c>ECX</c>).</param>
        /// <returns>The 64-bit contents of the requested MSR.</returns>
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern ulong Read(uint id);

        /// <summary>
        /// Writes <paramref name="value"/> to the MSR identified by 
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The MSR index to write (placed in <c>ECX</c>).</param>
        /// <param name="value">The 64-bit value to store (split into <c>EDX:EAX</c>).</param>
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern void Write(uint id, ulong value);
    }
}
