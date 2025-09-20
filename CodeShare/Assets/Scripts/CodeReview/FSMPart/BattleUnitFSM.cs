using UnityEngine;

public interface IBattleState
{
    public void Enter(); // 해당 상태로 들어갈 때 호출
    public void Update(); // 상태 유지 중 매 프레임 호출
    public void Exit(); // 상태를 종료할 때 호출
    public Vector3 GetDirection();
}

// 각 스테이트는 인터페이스를 상속받아 다형성을 유지합니다.
// 상태 전이를 일으키면서 시작점, 업데이트, 마무리를 각 상태에 맞게 로직을 짰습니다.
// 또 인터페이스를 바로 상속받기에 제한이 많아 베이스부모를 만들어 한단계 더 아래 자식으로 스테이트가 구성되어있습니다.

public abstract class BattleBaseState : IBattleState
{
    protected BattleFSMController fsmCtrl = null;
    protected Animator anim = null;
    protected Transform body = null;

    protected string aniName = string.Empty;
    protected bool isMonster = false;

    public BattleBaseState(BattleFSMController _fsmCtrl, Animator _anim, bool _isMonster)
    {
        fsmCtrl = _fsmCtrl;
        anim = _anim;
        isMonster = _isMonster;

        body = anim.transform.root.Find("Unit");
    }
    public Vector3 GetDirection()
    {
        return body.localScale.x < 0 ? Vector3.right : Vector3.left; // 캐릭터들 디자인이 왼쪽이 정방향
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
// 대기 상태(Idle)
public class BattleIdleState : BattleBaseState
{
    bool entered = false;

    public BattleIdleState(BattleFSMController _fsmCtrl, Animator _anim, bool _isMonster) : base(_fsmCtrl, _anim, _isMonster)
    {
        aniName = "IDLE";
    }
    public override void Enter()
    {
        anim.Play(aniName, 0, 0.0f);

        if (!entered)
            Filper.Filp(body, !isMonster);
        entered = true;
    }
    public override void Update() { }
    public override void Exit() { }
}
// 이동 상태(Move)
public class BattleChaseState : BattleBaseState
{
    Rigidbody2D moveBody = null;
    float moveSpeed = 0.0f;

    public BattleChaseState(BattleFSMController _fsmCtrl, Animator _anim, bool _isMonster) : base(_fsmCtrl, _anim, _isMonster)
    {
        aniName = "MOVE";
        moveBody = body.GetComponentInParent<Rigidbody2D>();
        moveSpeed = isMonster ? 2.0f : 4.0f;
    }
    public override void Enter()
    {
        if (fsmCtrl.GetTarget() == null)
        {
            fsmCtrl.TargetOutOfRange();
            return;
        }

        anim.Play(aniName, 0, 0.0f);

        Vector2 dirVec = (fsmCtrl.GetTarget().transform.position - body.position);
        Filper.Filp(body, dirVec.x > 0.0f);
    }
    public override void Update()
    {
        if (fsmCtrl.GetTarget() == null)
        {
            fsmCtrl.TargetOutOfRange();
            return;
        }

        Vector2 dirVec = (fsmCtrl.GetTarget().transform.position - body.position);
        Filper.Filp(body, dirVec.x > 0.0f);

        // 이동 방향 계산 및 고정 속도로 이동
        Vector2 newPos = Vector3.MoveTowards(body.position, fsmCtrl.GetTarget().transform.position, moveSpeed * Time.fixedDeltaTime);
        moveBody.MovePosition(newPos);

        if (dirVec.magnitude < fsmCtrl.GetAttackDist())
        {
            if(fsmCtrl.GetAttackDist() < Define.MaxMeleeDist && Mathf.Abs(dirVec.y) > 0.3f)
            {
                Vector2 testPos = new Vector2(body.position.x, fsmCtrl.GetTarget().transform.position.y);
                Vector2 newTestPos = Vector3.MoveTowards(body.position, testPos, moveSpeed * Time.fixedDeltaTime);
                moveBody.MovePosition(newTestPos);
                return;
            }
            fsmCtrl.ReachedToTarget();
            fsmCtrl.ChangeState(fsmCtrl.idleState);
        }
    }
    public override void Exit() { }
}
// 공격 상태(Attack)
public class BattleAttackState : BattleBaseState
{
    public BattleAttackState(BattleFSMController _fsmCtrl, Animator _anim, bool _isMonster) : base(_fsmCtrl, _anim, _isMonster)
    {
        aniName = "ATTACK";
    }
    public override void Enter()
    {
        Vector2 dirVec = (fsmCtrl.GetTarget().transform.position - body.position);
        if (dirVec.magnitude > fsmCtrl.GetAttackDist())
        {
            fsmCtrl.TargetOutOfRange();
            return;
        }

        Filper.Filp(body, dirVec.x > 0.0f);

        anim.Play(aniName, 0, 0.0f);
        fsmCtrl.CompleteAttack();
    }
    public override void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(aniName) && stateInfo.normalizedTime > 0.95f)
            fsmCtrl.ChangeState(fsmCtrl.idleState);
    }
    public override void Exit() { }
}
// 캐스트 상태(Cast)
public class BattleCastState : BattleBaseState
{
    public BattleCastState(BattleFSMController _fsmCtrl, Animator _anim, bool _isMonster) : base(_fsmCtrl, _anim, _isMonster)
    {
        aniName = "CAST1";
    }
    public override void Enter()
    {
        if (fsmCtrl.GetTarget() == null)
            return;

        // 엔터진입 전 캐스트모션 함수가 먼저 정해져야한다!
        anim.Play(aniName, 0, 0.0f);
    }
    public override void Update() { }
    public override void Exit() { }
    public void SetCastMotion(int _no)
    {
        if (_no == 0)
            aniName = "SKILL";
        else
            aniName = $"CAST{_no}";
    }
}
// 사망 상태(Dead)
public class BattleDeadState : BattleBaseState
{
    public BattleDeadState(BattleFSMController _fsmCtrl, Animator _anim, bool _isMonster) : base(_fsmCtrl, _anim, _isMonster)
    {
        aniName = "DEATH";
    }
    public override void Enter()
    {
        anim.Play(aniName, 0, 0.0f);
    }
    public override void Update() { }
    public override void Exit() { }
}
