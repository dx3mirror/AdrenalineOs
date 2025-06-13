using System.Runtime.InteropServices;

namespace AdrenalineOs.Boot.Drivers
{
    /// <summary>
    /// Exposes a native-callable <c>puts</c> shim that forwards characters
    /// to the managed <see cref="Serial"/> driver, allowing BIOS code or
    /// freestanding C/assembly to print strings over the UART without
    /// needing to know the managed infrastructure.
    /// </summary>
    internal static unsafe class SerialExport
    {
        /// <summary>
        /// Sends a zero-terminated ASCII byte string to the serial port.
        /// </summary>
        /// <remarks>
        /// Marked <see cref="UnmanagedCallersOnlyAttribute"/> so the CLR
        /// exports it as a raw symbol named <c>"puts"</c>; native code may
        /// call it with the platform default C calling convention.
        /// </remarks>
        /// <param name="str">Pointer to a null-terminated 8-bit string.</param>
        [UnmanagedCallersOnly(EntryPoint = "puts")]
        public static void Puts(byte* str)
        {
            while (*str != 0)
                Serial.PutChar((char)(*str++));
        }
    }
}
