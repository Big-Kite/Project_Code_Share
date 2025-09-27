using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffController : MonoBehaviour
{
    Dictionary<DebuffType, Debuff> debuffs = new Dictionary<DebuffType, Debuff>();
    HashSet<DebuffType> toRemove = new HashSet<DebuffType>();

    void Update()
    {
        toRemove.Clear();

        var tick = Time.deltaTime;
        foreach (var debuff in debuffs)
        {
            if(debuff.Value != null)
            {
                Debuff debuffValue = debuff.Value;
                debuffValue.TickTock(tick);

                if (debuffValue.isEnd())
                {
                    debuffValue.Cure();
                    toRemove.Add(debuff.Key);
                }
            }
        }

        foreach (var debuffKey in toRemove)
        {
            debuffs.Remove(debuffKey);
        }
    }
    public bool IsOnDebuff(DebuffType _type)
    {
        if (debuffs.TryGetValue(_type, out var debuff))
        {
            return debuff.OnDebuff;
        }
        return false;
    }
    public void AddDebuff(BattleUnit _unit, DebuffType _type, float _tick, Color _color)
    {
        if(debuffs.TryGetValue(_type, out var debuff))
        {
            debuff.AddTick(_tick);
        }
        else
        {
            Debuff newDebuff = null;
            switch(_type)
            {
                case DebuffType.UnableAct:
                    {
                        newDebuff = new DebuffUnableAct();
                    }
                    break;
                case DebuffType.UnableHeal:
                    {
                        newDebuff = new DebuffUnableHeal();
                    }
                    break;
                case DebuffType.UnableRegenMana:
                    {
                        newDebuff = new DebuffUnableRegenMana();
                    }
                    break;
                case DebuffType.UnableSkill:
                    {
                        newDebuff = new DebuffUnableSkill();
                    }
                    break;
                case DebuffType.PeriodicDamage:
                    {
                        newDebuff = new DebuffPeriodicDamage();
                    }
                    break;
            }
            newDebuff.Gain(_unit);
            newDebuff.AddTick(_tick);
            if(_color != Color.black)
                newDebuff.FlashPingPong(_color);

            debuffs.Add(_type, newDebuff);
        }
    }
}
