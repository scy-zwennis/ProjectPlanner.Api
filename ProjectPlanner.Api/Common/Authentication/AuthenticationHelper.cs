using ProjectPlanner.Api.Common.Constants;
using ProjectPlanner.Api.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ProjectPlanner.Api.Common.Authentication
{
    public static class AuthenticationHelper
    {
        public static byte[] GenerateRandomByteArray(int maximumLength)
        {
            var bytes = new byte[maximumLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(bytes);
            }

            return bytes;
        }

        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static byte[] StringToByteArray(string text)
        {
           return Encoding.UTF8.GetBytes(text);
        }
    }
}
