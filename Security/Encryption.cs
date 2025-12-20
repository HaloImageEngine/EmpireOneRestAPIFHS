using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EmpireOneRestAPIITJ.Security
{
    /// <summary>
    /// Password hashing utilities for .NET Framework 4.8.
    /// - Uses PBKDF2 (Rfc2898DeriveBytes) with HMAC-SHA256.
    /// - Format: v1$<iterations>$<base64(salt)>$<base64(hash)>
    /// NOTE: Passwords are hashed (one-way). Decryption is not supported.
    /// </summary>
    public static class Encryption
    {
        // Tunables (safe defaults for web apps; increase Iterations as needed)
        private const int SaltSize = 16;       // 128-bit salt
        private const int KeySize = 32;       // 256-bit derived key
        private const int Iterations = 10000;  // PBKDF2 iterations
        private const string SchemeVersion = "v1";

        /// <summary>
        /// Hashes a password using PBKDF2 (HMAC-SHA256) with a random salt.
        /// Returns: v1$iterations$saltBase64$hashBase64
        /// </summary>
        public static string EncryptPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            // Generate cryptographic salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive key
            byte[] key = Pbkdf2(password, salt, Iterations, KeySize);

            return string.Join("$",
                SchemeVersion,
                Iterations.ToString(),
                Convert.ToBase64String(salt),
                Convert.ToBase64String(key));
        }

        /// <summary>
        /// Passwords are stored as one-way hashes and cannot be decrypted.
        /// This method throws by design to prevent misuse.
        /// </summary>
        public static string DecryptPassword(string hash)
        {
            throw new NotSupportedException("Passwords are hashed with one-way PBKDF2 and cannot be decrypted.");
        }

        /// <summary>
        /// Verifies a plaintext password against a stored hash string.
        /// </summary>
        /// <param name="password">User-provided plaintext password.</param>
        /// <param name="storedHash">Stored string from EncryptPassword.</param>
        /// <returns>true if match; otherwise false.</returns>
        public static bool PasswordMatch(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            var parts = storedHash.Split('$');
            if (parts.Length != 4) return false;

            // versioning
            var version = parts[0];
            if (!string.Equals(version, SchemeVersion, StringComparison.Ordinal))
                return false; // unknown scheme; optionally support migrations here

            if (!int.TryParse(parts[1], out int iterations)) return false;

            byte[] salt, expectedKey;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expectedKey = Convert.FromBase64String(parts[3]);
            }
            catch
            {
                return false;
            }

            byte[] actualKey = Pbkdf2(password, salt, iterations, expectedKey.Length);

            return FixedTimeEquals(actualKey, expectedKey);
        }

        // ---- Helpers ----

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int length)
        {
            // Rfc2898DeriveBytes with HashAlgorithmName.SHA256 is available in .NET Framework 4.7+ (OK for 4.8)
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password: password,
                salt: salt,
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(length);
            }
        }

        // Constant-time comparison (avoid timing attacks)
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }
}
