using UnityEngine;

public interface IBattleState
{
    public void Enter(); // �ش� ���·� �� �� ȣ��
    public void Update(); // ���� ���� �� �� ������ ȣ��
    public void Exit(); // ���¸� ������ �� ȣ��
    public Vector3 GetDirection();
}

// �� ������Ʈ�� �������̽��� ��ӹ޾� �������� �����մϴ�.
// ���� ���̸� ����Ű�鼭 ������, ������Ʈ, �������� �� ���¿� �°� ������ ®���ϴ�.
// �� �������̽��� �ٷ� ��ӹޱ⿡ ������ ���� ���̽��θ� ����� �Ѵܰ� �� �Ʒ� �ڽ����� ������Ʈ�� �����Ǿ��ֽ��ϴ�.

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
        return body.localScale.x < 0 ? Vector3.right : Vector3.left; // ĳ���͵� �������� ������ ������
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
// ��� ����(Idle)
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
// �̵� ����(Move)
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

        // �̵� ���� ��� �� ���� �ӵ��� �̵�
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
// ���� ����(Attack)
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
// ĳ��Ʈ ����(Cast)
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

        // �������� �� ĳ��Ʈ��� �Լ��� ���� ���������Ѵ�!
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
// ��� ����(Dead)
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
