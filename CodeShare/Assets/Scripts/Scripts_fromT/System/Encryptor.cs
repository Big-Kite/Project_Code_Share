using System;
using System.Security.Cryptography;
using System.Text;

public static class Encryptor
{
    private static readonly string key = "YourSecretKey123!"; // 외부에서 쉽게 추측 어려운 값으로 설정

    public static string Encrypt(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText + key);
        return Convert.ToBase64String(bytes);
    }

    public static string Decrypt(string encryptedText)
    {
        byte[] bytes = Convert.FromBase64String(encryptedText);
        string combined = Encoding.UTF8.GetString(bytes);
        return combined.Replace(key, "");
    }

    public static string GetHash(string input)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + key);
            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
