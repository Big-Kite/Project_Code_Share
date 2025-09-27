using UnityEngine;

public class OverlayScreen : MonoBehaviour
{ // ���� ������� ������ �������� ����ؼ�.. (���� �� ��ƼŬ �� ui�� ����)
    GameObject backCanvas = null;

    protected virtual void OnEnable()
    {
        if (backCanvas != null)
        {
            backCanvas.SetActive(true);
            return;
        }

        foreach (Transform child in transform.root)
        {
            if (child.name == "OSBack")
            {
                backCanvas = child.gameObject;
                backCanvas.SetActive(true);
                break;
            }
        }
    }

    protected virtual void OnDisable()
    {
        if (backCanvas != null)
        {
            backCanvas.SetActive(false);
            return;
        }

        foreach (Transform child in transform.root)
        {
            if (child.name == "OSBack")
            {
                backCanvas = child.gameObject;
                backCanvas.SetActive(false);
                break;
            }
        }
    }
}