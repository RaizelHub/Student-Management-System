namespace StudentAttendanceSysttem.Helpers
{
    /// <summary>
    /// Hardware abstraction interface for any RFID reader device.
    /// Implement this interface for specific hardware (RC522, ESP32, USB RFID).
    /// </summary>
    public interface IRfidReader
    {
        /// <summary>Initializes and opens the RFID reader device.</summary>
        bool Initialize();

        /// <summary>Reads a card UID. Blocks until a card is scanned or timeout occurs.</summary>
        /// <param name="timeoutMs">Timeout in milliseconds. -1 = wait indefinitely.</param>
        /// <returns>The card UID string, or null if no card was read within the timeout.</returns>
        string? ReadCard(int timeoutMs = 5000);

        /// <summary>Starts continuous reading in the background. Raises <see cref="CardScanned"/> on each scan.</summary>
        void StartListening();

        /// <summary>Stops background listening.</summary>
        void StopListening();

        /// <summary>Raised when a card is scanned during background listening.</summary>
        event EventHandler<string> CardScanned;

        /// <summary>True if the reader device is connected and initialized.</summary>
        bool IsConnected { get; }

        /// <summary>Releases reader resources.</summary>
        void Dispose();
    }
}
