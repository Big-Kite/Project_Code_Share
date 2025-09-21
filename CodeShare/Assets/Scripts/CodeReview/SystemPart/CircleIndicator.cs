using UnityEngine;

public class CircleIndicator : Indicator
{
    float skillRadius = 0.0f;     // 최대 범위

    public override void Init(Skill _skill, Vector3 _pos, Vector3 _dir, float _range, float _delay)
    {
        skill = _skill;
        transform.position = _pos;
        skillRadius = _range;
        chargeTime = _delay;

        transform.localScale = new Vector3(1.0f, 0.5f, 1.0f) * skillRadius;

        charging.localScale = Vector3.zero;
        charging.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        if (!isCharging)
            return;

        currentTime += Time.deltaTime;
        float t = Mathf.Clamp01(currentTime / chargeTime);
        float currentRadius = 1.0f * t;
        charging.localScale = Vector3.one * currentRadius;

        if (t >= 1.0f)
        {
            isCharging = false;
            charging.localScale = Vector3.one;
            skill.CompleteIndicator();
        }
    }
}
