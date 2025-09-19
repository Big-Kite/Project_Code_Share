using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    bool isOpen = false; // 오픈 상태값
    bool closing = false; // 닫히는 중인지 상태값

    bool isSave = false; // close에서 삭제 여부
    float destroyTime = 0.1f;

    GameObject m_background;
    Color backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.85f);

    public void Open() // with background
    {
        if (isOpen)
            return;
        
        isOpen = true;
        closing = false;

        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, backgroundColor);
        bgTex.Apply();

        m_background = new GameObject("PopupBackground");
        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1.0f);

        var image = m_background.AddComponent<Image>();
        image.material = new Material(image.material);
        image.material.mainTexture = bgTex;
        image.sprite = sprite;

        var newColor = image.color;
        image.color = newColor;
        image.canvasRenderer.SetAlpha(0.0f);
        image.CrossFadeAlpha(1.0f, 0.4f, false);

        var canvas = GetComponentInParent<Canvas>();
        m_background.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        m_background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
        m_background.transform.SetParent(canvas.transform, false);
        m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }

    public void Close(bool _isSave = false)
    {
        if (closing)
            return;

        closing = true;
        isSave = _isSave;

        var animator = GetComponent<Animator>();
        if (animator && animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            animator.Play("Close");
            //SoundManager.Instance.PlaySfx(SFX.CancelButton_AU1);
        }

        if (m_background)
        {
            var image = m_background.GetComponent<Image>();
            if (image != null)
                image.CrossFadeAlpha(0.0f, 0.2f, false);
        }

        if (gameObject.activeSelf)
            StartCoroutine(CoDestroyPopup());
    }

    IEnumerator CoDestroyPopup()
    {
        yield return YieldCache.WaitForSeconds(destroyTime);

        isOpen = false;
        gameObject.SetActive(false);

        if (m_background)
            Destroy(m_background);

        if (isSave == false)
            Destroy(gameObject);
    }
}