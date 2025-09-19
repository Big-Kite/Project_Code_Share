using UnityEngine;

public static class EncryptedPrefs
{
    public static void SaveEncrypted<T>(string key, T data)
    {
        string json = JsonUtility.ToJson(data);
        string encrypted = Encryptor.Encrypt(json);
        string hash = Encryptor.GetHash(json);

        PlayerPrefs.SetString(key, encrypted);
        PlayerPrefs.SetString($"{key}_hash", hash);
        PlayerPrefs.Save();
    }

    public static T LoadEncrypted<T>(string key, T defaultValue)
    {
        string encrypted = PlayerPrefs.GetString(key, "");
        string savedHash = PlayerPrefs.GetString($"{key}_hash", "");

        if (string.IsNullOrEmpty(encrypted)) return defaultValue;

        string decrypted = Encryptor.Decrypt(encrypted);
        string computedHash = Encryptor.GetHash(decrypted);

        if (savedHash != computedHash)
        {
            Debug.LogWarning($"[EncryptedPrefs] {key} 해시 불일치! 조작 감지됨.");
            return defaultValue;
        }

        return JsonUtility.FromJson<T>(decrypted);
    }
}
