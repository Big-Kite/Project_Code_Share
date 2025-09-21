using UnityEngine;

public class LineIndicator : Indicator
{
    float skillLength = 0.0f;  // ½ºÅ³ ÃÖ´ë °Å¸®
    float skillWidth = 0.8f; // ¶óÀÎÀÇ ±½±â (ÀÓ½Ã)

    public override void Init(Skill _skill, Vector3 _pos, Vector3 _dir, float _range, float _delay)
    {
        skill = _skill;
        skillLength = _range;
        chargeTime = _delay;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir.normalized);

        transform.position = _pos + (_dir.normalized * (_range * 0.5f));

        SetupLine(transform, skillLength, skillWidth);
        SetupLine(charging, 0.0f, 1.0f);
        charging.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        if (!isCharging)
            return;

        currentTime += Time.deltaTime;
        float t = Mathf.Clamp01(currentTime / chargeTime);
        float currentLength = 1.0f * t;
        charging.localScale = new Vector3(1.0f, currentLength, 1.0f);
        charging.localPosition = new Vector3(0.0f, (currentLength / 2f) - 0.5f, 0f);  // Áß¾Ó ÇÇ¹þ º¸Á¤

        if (t >= 1f)
        {
            isCharging = false;
            //SetupLine(charging, 0.0f, 1.0f);
            skill.CompleteIndicator();
        }
    }
    void SetupLine(Transform line, float length, float _width)
    {
        line.localScale = new Vector3(_width, length, 1f);
        //line.localPosition = new Vector3(line.localPosition.x, line.localPosition.y + (length / 2f), 0f);  // Áß¾Ó ÇÇ¹þ º¸Á¤
    }
}
