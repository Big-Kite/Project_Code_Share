using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    Color damageCol = new Color(1.0f, 0.28f, 0.0f);
    Color healCol = new Color(0.15f, 1.0f, 0.0f);
    Color protectCol = new Color(0.61f, 0.61f, 0.61f);
    
    public void SetFloatingUI(FloatingType _type, Vector3 _pos, int _value)
    {
        transform.position = _pos;

        float time = 0.5f, xValue = 1.0f;
        switch(_type)
        {
            case FloatingType.Damage:
                {
                    text.color = damageCol;
                }
                break;
            case FloatingType.Heal:
                {
                    text.color = healCol;
                    time = 1.0f;
                    xValue = 0.0f;
                }
                break;
            case FloatingType.Protect:
                {
                    text.color = protectCol;
                    time = 0.7f;
                }
                break;
        }
        text.text = $"{_value}";

        StartCoroutine(CoHide(time, xValue));
    }
    IEnumerator CoHide(float _time, float _xValue)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f) * _xValue, 1.2f, 0.0f);

        float routineTime = 0.0f;
        while (routineTime < _time)
        {
            routineTime += Time.deltaTime;
            float clampTime = Mathf.Clamp01(routineTime / _time);

            float xPos = Mathf.Lerp(startPos.x, endPos.x, clampTime);
            float yPos = Mathf.Lerp(startPos.y , endPos.y, clampTime) + (0.5f * Mathf.Sin(Mathf.PI * clampTime));

            transform.position = new Vector3(xPos, yPos, 0.0f);
            yield return null;
        }

        yield return YieldCache.WaitForSeconds(_time);
        gameObject.SetActive(false);
    }
}
