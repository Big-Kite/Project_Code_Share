using System.Linq;
using UnityEngine;

public class NpcInBattle : BattleUnit
{
    [SerializeField] GameObject dropItem;

    NpcData npcData = null;

    public override void Init(int _dataKey = 0)
    {
        npcData = DataRef.GetNpcDB.GetNpcData(_dataKey).data;
        Instantiate(DataRef.GetNpcDB.GetNpcData(_dataKey).prefab, physicalLayer);

        BattleUnitInit(true, npcData.AtkSpeed);

        AtkDist = npcData.AtkDist;
        HpComponent.Init(npcData.Hp, npcData.Mp);
    }
    public override void TakeDamage(int _damage)
    {
        if (HpComponent.IsDead())
            return;

        HitFlash();
        EffectController.Instance.ShowFloatingUI(FloatingType.Damage, transform.position, _damage);

        HpComponent.TakeDamage(_damage);
        if (HpComponent.IsDead())
        {
            Dead();
            DropItem();
            BattleManager.Instance.CheckWinLose();
        }
        else
            StartCoroutine(CoShake(0.1f, 0.1f));
    }
    public override void CompleteAttack()
    {
        if (AtkDist <= Define.MaxMeleeDist) // 근거리
        {
            EffectController.Instance.ShowBattleEffect(BattleEffect.SwordHit, Target.Front.transform.position, 0.5f, _randRotate: true);
            Target.TakeDamage(1);
        }
        else                            // 원거리
        {
            Vector3 dir = Target.Front.position - Front.position;
            dir.Normalize();

            EffectController.Instance.ShowBattleEffect((BattleEffect)(Define.ProjectileSet[npcData.ProjectileType, 1]), Front.transform.position, 0.5f, _randRotate: true);

            GameObject projectileInst = EffectController.Instance.GetBattleProjectile((BattleEffect)(Define.ProjectileSet[npcData.ProjectileType, 0]));
            projectileInst.transform.position = Front.position;

            Projectile projectile = projectileInst.GetComponent<Projectile>();
            projectile.Init(npcData.ProjectileType, dir, 5.0f);
        }
    }
    protected override void FindTarget()
    {
        Target = BattleManager.Instance.Player;
    }
    protected override void Attack()
    {
        if (npcData.SkillNo != 0)
        {
            if (HpComponent.UseMana(SkillManager.Instance.GetNeedMp(npcData.SkillNo)))
            {
                SkillManager.Instance.ActivateSkill(this, npcData.SkillNo);
                return;
            }
        }
        fsmCtrl.Attack();
    }
    void DropItem()
    {
        var dropItemInst = Instantiate(dropItem, transform.position, Quaternion.identity);
        dropItemInst.GetComponent<DropItem>().Init(npcData.Key);
    }
}
