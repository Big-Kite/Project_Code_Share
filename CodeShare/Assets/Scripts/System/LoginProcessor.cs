using UnityEngine;

public class LoginProcessor : Singleton<LoginProcessor>
{
    bool isLogIn = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void LogInTry()
    {
        // �α��� ó��
        // ����Ǹ�
        LogInSuccess();
    }
    void LogInSuccess()
    {
        isLogIn = true;
    }
    public bool DidLogIn()
    {
        return isLogIn;
    }
}
