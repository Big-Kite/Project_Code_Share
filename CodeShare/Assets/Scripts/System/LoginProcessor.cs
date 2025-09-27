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
        // 로그인 처리
        // 통과되면
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
