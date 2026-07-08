namespace StudentAttendanceSysttem.Helpers
{
    /// <summary>
    /// Hardware abstraction interface for fingerprint scanners.
    /// Implement for ZKTeco, R307, AS608, or any other biometric device.
    /// </summary>
    public interface IFingerprintReader
    {
        /// <summary>Initializes and connects to the fingerprint device.</summary>
        bool Initialize();

        /// <summary>Captures a fingerprint and returns its unique template ID, or -1 on failure.</summary>
        int CaptureFingerprint(int timeoutMs = 8000);

        /// <summary>Enrolls a new fingerprint and returns the assigned template ID.</summary>
        int Enroll(int studentId);

        /// <summary>Verifies that the currently scanned finger matches the stored template for the given ID.</summary>
        bool Verify(int templateId);

        /// <summary>Identifies the scanned finger against all stored templates. Returns matched template ID or -1.</summary>
        int Identify();

        /// <summary>Deletes the stored fingerprint template for the given ID.</summary>
        bool DeleteTemplate(int templateId);

        /// <summary>True if the device is connected and ready.</summary>
        bool IsConnected { get; }

        /// <summary>Total number of stored templates on the device.</summary>
        int TemplateCount { get; }

        void Dispose();
    }
}
