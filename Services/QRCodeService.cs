using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace StudentAttendanceSysttem.Services
{
    /// <summary>
    /// Generates QR code images for student attendance cards.
    /// Uses QRCoder library. Each QR code encodes the student's unique student number.
    /// </summary>
    public class QRCodeService
    {
        private const int PixelsPerModule = 6;

        /// <summary>Generates a QR code bitmap for the given data string.</summary>
        public Bitmap GenerateQRCode(string data, int pixelsPerModule = PixelsPerModule)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("QR data cannot be empty.", nameof(data));

            using var generator = new QRCodeGenerator();
            var qrData = generator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrData);

            // QRCoder 1.6.x GetGraphic(int pixelsPerModule)
            return qrCode.GetGraphic(pixelsPerModule);
        }

        /// <summary>Generates and saves QR code as PNG. Returns the file path.</summary>
        public string GenerateAndSave(string studentNumber, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string filePath = Path.Combine(outputFolder, $"qr_{studentNumber}.png");
            using var bitmap = GenerateQRCode(studentNumber);
            bitmap.Save(filePath, ImageFormat.Png);
            return filePath;
        }

        /// <summary>Returns Base64-encoded PNG of the QR code.</summary>
        public string GenerateAsBase64(string data)
        {
            using var bitmap = GenerateQRCode(data);
            using var ms     = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
