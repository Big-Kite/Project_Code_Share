using UnityEngine;

public class OverlayScreen : MonoBehaviour
{ // 아직 사용하진 않지만 언젠가를 대비해서.. (딤드 위 파티클 위 ui를 위해)
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