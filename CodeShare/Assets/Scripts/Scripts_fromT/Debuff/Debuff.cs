using System.Collections;
using UnityEngine;

public abstract class Debuff
{
    protected BattleUnit master = null;
    protected float tick = 0.0f;

    public bool OnDebuff { get; protected set; } = false;

    public void Gain(BattleUnit _unit)
    {
        master = _unit;
    }
    protected abstract void Update(float _delta);
    public abstract void Cure();
    public void AddTick(float _tick)
    {
        tick += _tick;
    }
    public void TickTock(float _delta)
    {
        tick -= _delta;
        Update(tick);
    }
    public bool isEnd()
    {
        return tick < 0.0f;
    }
    public void FlashPingPong(Color _color)
    {
        master.StartCoroutine(CoFlashPingPong(_color));
    }
    protected IEnumerator CoFlashPingPong(Color _color)
    {
        float min = 0.3f;
        float max = 0.8f;
        float frequency = 1.5f;

        while (!isEnd())
        {
            float value = Mathf.PingPong(Time.time * frequency, max - min) + min;

            if (!master.Flash)
            {
                foreach (var mat in master.Mats)
                {
                    mat.SetColor("_HitEffectColor", _color);
                    mat.SetFloat("_HitEffectBlend", value);
                }
            }
            yield return null;
        }
        master.RestoreMaterial();
    }
}
public class DebuffUnableAct : Debuff
{
    protected override void Update(float _delta)
    {
        if (!isEnd())
            OnDebuff = true;
        else
            OnDebuff = false;
    }
    public override void Cure()
    {
        OnDebuff = false;
    }
}
public class DebuffUnableHeal : Debuff
{
    protected override void Update(float _delta)
    {

    }
    public override void Cure()
    {

    }
}
public class DebuffUnableRegenMana : Debuff
{
    protected override void Update(float _delta)
    {

    }
    public override void Cure()
    {

    }
}
public class DebuffUnableSkill : Debuff
{
    protected override void Update(float _delta)
    {

    }
    public override void Cure()
    {

    }
}
public class DebuffPeriodicDamage : Debuff
{
    protected override void Update(float _delta)
    {

    }
    public override void Cure()
    {

    }
}