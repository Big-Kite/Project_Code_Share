using UnityEngine;

[System.Serializable]
public class UserInfoData
{
    public string playerName = "Player";
    public string createdAt = System.DateTime.Now.ToString();
    public string lastLogin = System.DateTime.Now.ToString();
}

[System.Serializable]
public class StatusData
{
    public int level = 1;
    public int exp = 0;
    public int maxHp = 100;
}

[System.Serializable]
public class CurrencyData
{
    public int gold = 100;
    public int gems = 10;
    public int point = 0;
}

public class Backuper
{
    private const string UserInfoKey = "UserInfoData";
    private const string StatusKey = "StatusData";
    private const string CurrencyKey = "CurrencyData";

    public UserInfoData userInfo { get; private set; }
    public StatusData status { get; private set; }
    public CurrencyData currency { get; private set; }

    public void LoadAll()
    {
        userInfo = EncryptedPrefs.LoadEncrypted(UserInfoKey, new UserInfoData());
        status = EncryptedPrefs.LoadEncrypted(StatusKey, new StatusData());
        currency = EncryptedPrefs.LoadEncrypted(CurrencyKey, new CurrencyData());

        Debug.Log("[Backuper] 모든 데이터 불러오기 완료");
    }

    public void SaveAll()
    {
        EncryptedPrefs.SaveEncrypted(UserInfoKey, userInfo);
        EncryptedPrefs.SaveEncrypted(StatusKey, status);
        EncryptedPrefs.SaveEncrypted(CurrencyKey, currency);

        Debug.Log("[Backuper] 모든 데이터 저장 완료");
    }

    // 개별 저장도 가능
    public void SaveUserInfo() => EncryptedPrefs.SaveEncrypted(UserInfoKey, userInfo);
    public void SaveStatus() => EncryptedPrefs.SaveEncrypted(StatusKey, status);
    public void SaveCurrency() => EncryptedPrefs.SaveEncrypted(CurrencyKey, currency);
}
