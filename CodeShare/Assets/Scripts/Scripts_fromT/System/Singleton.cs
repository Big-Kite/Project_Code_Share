using System.Linq;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    public static T Instance
    {
        get
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
                return null;
#endif
            if (instance == null)
            {
                instance = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
                if (instance == null)
                {
                    GameObject newSingleton = new GameObject();
                    instance = newSingleton.AddComponent<T>();
                    newSingleton.name = $"{typeof(T).ToString()}(singleton)";
                }
            }
            if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                Debug.LogError("[Singleton] More than 1 singleton!!");

            return instance;
        }
    }

    protected virtual void OnDestroy()
    { // �����Ǹ� �ȵǴ� ������Ʈ�� don't destroy �ؾ߰���?
        if (instance == this)
            instance = null;
    }

    public static bool HasInstance()
    {
        if (instance == null)
        {
            if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 0)
                return true;
            else
                return false;
        }
        else
            return true;
    }
}