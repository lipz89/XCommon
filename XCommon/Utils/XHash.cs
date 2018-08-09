using System;
using System.Security.Cryptography;

namespace XCommon.Utils
{
    /// <summary>
    /// Salted password hashing with PBKDF2-SHA1.
    /// Author: havoc AT defuse.ca
    /// www: http://crackstation.net/hashing-security.htm
    /// Compatibility: .NET 3.0 and later.
    /// </summary>
    public static class XHash
    {
        // The following constants may be changed without breaking existing hashes.
        private const int SALT_BYTE_SIZE = 24;
        private const int HASH_BYTE_SIZE = 24;
        private const int PBKDF2_ITERATIONS = 5000;
        private const int PBKDF2_AUTHORIZE_ITERATIONS = 50;

        private const int SALT_LENGTH = 32;
        private const int PBKDF2_LENGTH = 32;

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="input">The password to hash.</param>
        /// <returns>The hash of the password.</returns>
        public static string Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            byte[] hash = Pbkdf2(input, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return Convert.ToBase64String(salt) + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="publicKey"></param>
        /// <returns>The hash of the password.</returns>
        public static string CreateAuthorize(string tick, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(tick) || string.IsNullOrWhiteSpace(publicKey))
            {
                return null;
            }
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            var input = tick + publicKey;
            // Hash the password and encode the parameters
            byte[] hash = Pbkdf2(input, salt, PBKDF2_AUTHORIZE_ITERATIONS, HASH_BYTE_SIZE);
            return Convert.ToBase64String(salt) + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="publicKey">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool ValidateAuthorize(string tick, string publicKey, string correctHash)
        {
            if (string.IsNullOrWhiteSpace(tick) ||
                string.IsNullOrWhiteSpace(publicKey) ||
                string.IsNullOrWhiteSpace(correctHash))
            {
                return false;
            }
            var input = tick + publicKey;
            return Validate(input, correctHash, PBKDF2_AUTHORIZE_ITERATIONS);
        }
        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="input">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool Validate(string input, string correctHash)
        {
            if (string.IsNullOrWhiteSpace(input) ||
                string.IsNullOrWhiteSpace(correctHash))
            {
                return false;
            }
            return Validate(input, correctHash, PBKDF2_ITERATIONS);
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="input">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <param name="iterations"></param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        private static bool Validate(string input, string correctHash, int iterations)
        {
            if (correctHash.Length != SALT_LENGTH + PBKDF2_LENGTH)
            {
                return false;
            }
            // Extract the parameters from the hash
            var index = 0;
            byte[] salt = Convert.FromBase64String(correctHash.Substring(index, SALT_LENGTH));
            index += SALT_LENGTH;
            byte[] hash = Convert.FromBase64String(correctHash.Substring(index, PBKDF2_LENGTH));

            byte[] testHash = Pbkdf2(input, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
