using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInBattle : BattleUnit
{
    //플레이어가 컨트롤하는 로직이 씬마다 다르기 때문에 씬에 맞는 로직별 플레이어 컨트롤러입니다.
    [SerializeField] LayerMask targetLayer;
    RaycastHit2D[] targets = null;

    Vector2 inputVec = Vector2.zero;
    float speed = 11.0f;

    void Start()
    {
        Init();
        PlayerData.Instance.StoredEquipItem();
    }
    void OnMove(InputValue _value) // Updating PlayerInput component
    {
        inputVec = _value.Get<Vector2>();
    }
    void Update()
    {
        if (skillCast)
            return;

        if (inputVec.magnitude > 0.0f)
        {
            ManualOperateOn();
            ManualOperateMove();
        }
        else if (inputVec.magnitude <= 0.0f)
        {
            ManualOperateStop();
        }
        FindTarget();
    }
    void ManualOperateOn()
    {
        if (manualOperate)
            return;

        manualOperate = true;
        ChangeState(UnitState.Idle); // 수동조작이 켜지면 일단 유휴상태를 만들자
        fsmCtrl.ChangeState(fsmCtrl.idleState);
    }
    void ManualOperateMove()
    {
        if (!manualOperate)
            return;

        Vector2 nextVec = inputVec * speed * Time.deltaTime;
        Vector2 targetPos = body.position + nextVec;

        body.MovePosition(targetPos);

        anim.SetFloat("Speed", 1.0f);
        Filper.InputFilp(physicalLayer, inputVec);
    }
    void ManualOperateStop()
    {
        if (!manualOperate)
            return;

        anim.SetFloat("Speed", 0.0f);
        //KeepGoing();
    }
    public override void Init(int _dataKey = 0)
    {
        BattleUnitInit(false, PlayerData.Instance.AtkSpeed);

        AtkDist = 2.0f;
        HpComponent.Init(PlayerData.Instance.Hp, PlayerData.Instance.Mp);
    }
    public override void TakeDamage(int _damage)
    {
        if (curShield?.TakeDamage(ref _damage) ?? false)
            return;

        HitFlash();
        EffectController.Instance.ShowFloatingUI(FloatingType.Damage, transform.position, _damage);

        HpComponent.TakeDamage(_damage);
        StartCoroutine(CoShake(0.1f, 0.1f));
        EffectController.Instance.ShowShake(0.03f, 0.1f);

        if (HpComponent.IsDead())
            BattleManager.Instance.CheckWinLose();
    }
    public override void CompleteAttack()
    {
        EffectController.Instance.ShowBattleEffect(BattleEffect.SwordHit, Target.Front.transform.position, 0.5f, _randRotate: true);

        Target.TakeDamage(1);
        if(Target.IsDead())
        {
            Target = null;
            ChangeState(UnitState.Idle); // 공격 후 대상이 죽었다면 목표가 사라진 것으로 다시 유휴상태 돌입
        }
    }
    protected override void FindTarget()
    {
        if (state >= UnitState.Fight)
            return;

        targets = Physics2D.CircleCastAll(transform.position, 10.0f, Vector2.zero, 0, targetLayer);
        GetNearest();
    }
    protected override void Attack()
    {
        fsmCtrl.Attack();
    }
    void GetNearest()
    {
        BattleUnit near = null;
        float diff = 100.0f;

        foreach(var tempTarget in targets)
        {
            var battleUnit = tempTarget.transform.GetComponent<BattleUnit>();
            if (battleUnit == null || battleUnit.IsDead())
                continue;

            float curDiff = Vector3.Distance(transform.position, tempTarget.transform.position);

            if(curDiff < diff)
            {
                diff = curDiff;
                near = battleUnit;
            }
        }

        if (near != null && near != Target)
        {
            BattleManager.Instance.SetTarget(near);
        }
        Target = near;
    }
}
