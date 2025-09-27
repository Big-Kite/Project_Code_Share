using System;
using UnityEngine;

public class BattleFSMController : IDisposable
{
    IBattleState currentState;

    BattleUnit master;
    Animator anim;

    public BattleIdleState idleState;
    public BattleChaseState chaseState;
    BattleAttackState attackState;
    public BattleCastState castState;
    public BattleDeadState deadState;

    public BattleFSMController(BattleUnit _owner, Animator _anim, bool _isMonster)
    { // 생성자
        master = _owner;
        currentState = null;
        anim = _anim;

        idleState = new BattleIdleState(this, anim, _isMonster);
        chaseState = new BattleChaseState(this, anim, _isMonster);
        attackState = new BattleAttackState(this, anim, _isMonster);
        castState = new BattleCastState(this, anim, _isMonster);
        deadState = new BattleDeadState(this, anim, _isMonster);
    }
    public void Dispose()
    { // 소멸자 (IDisposable 인터페이스에서 제공, 해당 클래스를 가진 오브젝트 디스트로이에서 실행하면 됨.)
        master = null;
        currentState = null;
        anim = null;

        idleState = null;
        chaseState = null;
        attackState = null;
        castState = null;
        deadState = null;
    }
    public BattleUnit GetTarget()
    {
        return master.Target;
    }
    public float GetAttackDist()
    {
        return master.AtkDist;
    }
    public void ReachedToTarget()
    {
        master.ReachedToTarget();
    }
    public void CompleteAttack()
    {
        master.CompleteAttack();
    }
    public void TargetOutOfRange()
    {
        master.TargetOutOfRange();
    }
    public void SetCastMotion(int _no)
    {
        castState.SetCastMotion(_no);
    }
    public void ChangeState(IBattleState _newState)
    { // 상태를 변경할 때 현재 상태의 Exit을 호출한 뒤, 새 상태의 Enter를 호출합니다.
        if (currentState == _newState)
            return;

        currentState?.Exit();

        currentState = _newState;
        currentState?.Enter();
    }
    public void Attack()
    {
        currentState = attackState;
        currentState?.Enter();
    }
    public void Update()
    { // 매 프레임 현재 상태의 Update를 호출합니다.
        currentState?.Update();
    }
    public Vector3 GetDirection()
    {
        return currentState?.GetDirection() ?? Vector3.right;
    }
}
