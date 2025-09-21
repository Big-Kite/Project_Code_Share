using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // ����ϴ� ��ų���� �����ؾ��� ������Ʈ���� Ǯ�� ��ƵӴϴ�.
    // ��ų�� ��ü�� ��Ͻ��� �÷��̾� �� �ƴ϶� NPC�� �� �� �ֵ��� ���������� ��������ϴ�.
    // ���۷��̼� �Լ����� ��ų ������ ¥��, �ʿ信 ���� �ε������� �����̸� ��ٷ��� �ϴ� ��ų�� �ε������Ϳ��� �Ϸ� ȣ���� �޾� ������ �����մϴ�.
    protected Pool pool = null;
    protected BattleUnit master = null;

    protected virtual void Awake()
    {
        pool = GetComponent<Pool>();
    }
    protected virtual void OnDisable()
    {
        master = null;
    }
    public abstract void OperationSkill(BattleUnit _unit);
    public abstract int GetNeedMp();
    public virtual void CompleteIndicator() { }
}
