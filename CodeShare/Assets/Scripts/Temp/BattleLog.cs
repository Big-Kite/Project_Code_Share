using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour
{
    [SerializeField] Image panel;
    [SerializeField] TextMeshProUGUI logText;

    Color panelColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    Color textColor = Color.white;

    IEnumerator Start()
    {
        yield return YieldCache.WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
    public void SetColor()
    {
        panel.color = panelColor;
        logText.color = textColor;
    }
    public void SetText(string _text)
    {
        logText.text = _text;
    }
}
