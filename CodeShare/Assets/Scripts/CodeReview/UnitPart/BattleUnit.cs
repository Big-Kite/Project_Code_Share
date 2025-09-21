using System.Collections;
using UnityEditor;
using UnityEngine;

public abstract class BattleUnit : MonoBehaviour
{
    // 배틀씬에 스폰되는 플레이어와 NPC 모두 유닛 개별 FSM으로 돌아가는 오브젝트입니다.
    // 그래서 베이스 클래스로 만든 클래스로, FSM뿐 아니라 체력, 디버프, 애니메이션 실행, 탐색 등 공통적인 컨트롤과 추상 함수를 가지고 있습니다.
    [SerializeField] protected Transform physicalLayer;
    [SerializeField] Transform front;

    protected UnitState state = UnitState.Idle;

    public Transform Front { get { return front; } }
    public HpSystem HpComponent { get; protected set; } = null;
    public BattleUnit Target { get; protected set; } = null;
    public Material[] Mats { get; private set; } = null;

    protected Rigidbody2D body = null;
    protected Animator anim = null;
    protected BattleFSMController fsmCtrl = null;
    DebuffController debuffCtrl = null;

    public bool Flash { get; private set; } = false;

    public float AtkDist { get; protected set; } = 0.0f;
    float defaultInterval = Define.DefaultInterval;
    float atkInterval = 0.0f;
    float atkSpeed = 0.0f;

    protected bool skillCast = false;
    protected bool manualOperate = false; // 수동 조작

    protected Shield curShield = null;

    public abstract void Init(int _dataKey = 0);
    public abstract void TakeDamage(int _damage);
    public abstract void CompleteAttack();
    protected abstract void FindTarget();
    protected abstract void Attack();

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (debuffCtrl.IsOnDebuff(DebuffType.UnableAct))
            return;

        fsmCtrl?.Update();
    }
    void LateUpdate()
    {
        InBound();
    }
    void OnDestroy()
    {
        fsmCtrl.Dispose();
    }
    void InBound()
    {
        Bounds bounds = UnitPlacement.Instance.PlayerMoveBounds;
        if (bounds.Contains(body.transform.position))
            return;

        float clampedX = Mathf.Clamp(body.transform.position.x, bounds.min.x, bounds.max.x);
        float clampedY = Mathf.Clamp(body.transform.position.y, bounds.min.y, bounds.max.y);
        var fixedPos = new Vector3(clampedX, clampedY, 0.0f);

        body.transform.position = fixedPos;
    }
    protected void BattleUnitInit(bool _isMonster, float _atkSpeed)
    {
        SetMaterial();

        atkSpeed = _atkSpeed;
        atkInterval = atkSpeed;

        anim = GetComponentInChildren<Animator>();
        HpComponent = GetComponentInChildren<HpSystem>();

        fsmCtrl = new BattleFSMController(this, anim, _isMonster);
        ChangeState(UnitState.Idle); // 처음 초기화 유휴
        fsmCtrl.ChangeState(fsmCtrl.idleState);

        debuffCtrl = gameObject.AddComponent<DebuffController>();
    }
    void SetMaterial()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);
        Mats = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = EffectController.Instance.GetMaterial(0);
            Mats[i] = renderers[i].material;
        }
    }
    public void UnitStateUpdate(float _delta)
    {
        if (debuffCtrl.IsOnDebuff(DebuffType.UnableAct))
            return;

        if (skillCast || manualOperate)
            return;

        defaultInterval -= _delta;
        if(defaultInterval < 0.0f)
        {
            switch (state)
            {
                case UnitState.Idle:
                    {
                        if (Target != null)
                        {
                            ChangeState(UnitState.Chase); // 유휴상태에서 타겟을 탐색했다면 쫒기 시작
                            fsmCtrl.ChangeState(fsmCtrl.chaseState);
                        }
                        else
                            FindTarget();
                    }
                    break; // 딱히 할 건 없고, 체이스를 위한 유닛 스캔을 미리 해두기
                case UnitState.Chase:
                    {
                        fsmCtrl.ChangeState(fsmCtrl.chaseState);
                    }
                    break; // 스캔된 유닛한테 쫒아가기(원거리는 생략)
                case UnitState.Fight:
                    {
                        atkInterval -= _delta;
                        if (atkInterval < 0.0f)
                        {
                            Attack();
                            atkInterval = atkSpeed;
                        }
                        else
                            return;
                    }
                    break; // 데이터 공속에 맞춰서 지속 공격 (유닛간 체력이 다 할때까지)
                case UnitState.Dead: break; // 죽은 상태
            }
            defaultInterval = Define.DefaultInterval;
        }
    }
    public void KeepGoing()
    {
        manualOperate = false;
        defaultInterval = Define.DefaultInterval;
    }
    public void SkillCast(int _motionNo)
    {
        fsmCtrl.SetCastMotion(_motionNo);
        fsmCtrl.ChangeState(fsmCtrl.castState);

        skillCast = true;
    }
    public void CompleteSkillCast()
    {
        skillCast = false;

        ChangeState(UnitState.Idle); // 스킬동작을 완료했다면 다시 유휴상태 돌입
        fsmCtrl.ChangeState(fsmCtrl.idleState);
    }
    protected void ChangeState(UnitState _state)
    {
        if (skillCast)
            return;

        state = _state;
    }
    public void TargetOutOfRange()
    {
        ChangeState(UnitState.Idle); // 공격범위안에 타겟이 없다면(존재가없던지, 멀리있던지) 유휴상태 돌입
    }
    public void ReachedToTarget()
    {
        ChangeState(UnitState.Fight); // 목표물을 범위안에 도달했다면 공격 시작
    }
    public bool IsDead()
    {
        return HpComponent.IsDead();
    }
    public void HitFlash()
    {
        if (Flash)
            return;

        StartCoroutine(CoFlash(Color.red));
    }
    public void ShockFlash()
    {
        if (Flash)
            return;

        StartCoroutine(CoFlash(Color.blue));
    }
    IEnumerator CoFlash(Color _color)
    {
        Flash = true;
        foreach (var mat in Mats)
        {
            mat.SetColor("_HitEffectColor", _color);
            mat.SetFloat("_HitEffectBlend", 0.6f);
        }
        yield return new WaitForSeconds(0.2f);

        RestoreMaterial();
        Flash = false;
    }
    public void Dead()
    {
        StartCoroutine(CoDead());
    }
    IEnumerator CoDead()
    {
        float routineTime = 0.0f;
        float duration = 3.0f;

        while (routineTime < duration)
        {
            routineTime += Time.deltaTime;
            float clampTime = Mathf.Clamp01(routineTime / duration);
            foreach (var mat in Mats)
            {
                mat.SetFloat("_FadeAmount", clampTime);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
    public void RestoreMaterial()
    {
        foreach (var mat in Mats)
        {
            mat.SetFloat("_HitEffectBlend", 0.0f);
        }
    }
    protected IEnumerator CoShake(float _time, float _magnitude)
    {
        var shakeTransform = anim.transform.parent;

        float routineTime = 0f;
        Vector3 originalPos = shakeTransform.localPosition;

        float seedX = Random.Range(0f, 100f);
        float seedY = Random.Range(0f, 100f);

        while (routineTime < _time)
        {
            routineTime += Time.deltaTime;

            float x = Mathf.PerlinNoise(seedX, Time.time * 10f) * 2f - 1f;
            float y = Mathf.PerlinNoise(seedY, Time.time * 10f) * 2f - 1f;

            shakeTransform.localPosition = new Vector3(originalPos.x + x * _magnitude, originalPos.y + y * _magnitude, originalPos.z);

            yield return null;
        }
        shakeTransform.localPosition = Vector3.zero;
    }
    public void TakeHeal(int _heal)
    {
        if (debuffCtrl.IsOnDebuff(DebuffType.UnableHeal))
            _heal = 0;

        HpComponent.TakeHeal(_heal);
        EffectController.Instance.ShowFloatingUI(FloatingType.Heal, transform.position, _heal);
    }
    public void AttachShield(Shield _shield)
    {
        curShield = _shield;
    }
    public void DetachShield()
    {
        curShield = null;
    }
    public Vector3 GetDirection()
    {
        return fsmCtrl.GetDirection();
    }
    public void TakeDebuff(DebuffType _type, float _tick, Color? _color = null)
    {
        debuffCtrl.AddDebuff(this, _type, _tick, _color ?? Color.black);
    }
    public Bounds GetTotalBounds()
    {
        var totalBounds = new Bounds(transform.position, Vector3.zero);

        var renderers = transform.GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0)
            return totalBounds;

        totalBounds = renderers[0].bounds;
        foreach (var r in renderers)
        {
            totalBounds.Encapsulate(r.bounds); // 다른 자식 스프라이트까지 포함
        }
        return totalBounds;
    }








#if QA
    void OnDrawGizmos()
    {
        Bounds _cachedBounds = GetTotalBounds();
        bool _hasBounds = _cachedBounds.size.sqrMagnitude > 0f;

        if (!_hasBounds) return;

        // 3D WireCube(월드 AABB)
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_cachedBounds.center, _cachedBounds.size);

        // 2D 외곽선(XY 평면에 네 변)
        Gizmos.color = Color.cyan;
        Vector3 min = _cachedBounds.min;
        Vector3 max = _cachedBounds.max;

        // z는 가운데로 고정(2D 프로젝트 기준)
        float z = _cachedBounds.center.z;
        Vector3 a = new Vector3(min.x, min.y, z);
        Vector3 b = new Vector3(max.x, min.y, z);
        Vector3 c = new Vector3(max.x, max.y, z);
        Vector3 d = new Vector3(min.x, max.y, z);

        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
        Gizmos.DrawLine(d, a);

        // 너비/높이 라벨
        if (true)
        {
            var size = _cachedBounds.size;
            Handles.color = Color.cyan;
            Handles.Label(_cachedBounds.center + new Vector3(0, size.y * 0.55f, 0),
                $"W: {size.x:F3}  H: {size.y:F3}");
        }
    }
#endif
}
