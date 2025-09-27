using System.Linq;
using UnityEditor;
using UnityEngine;

public class NpcInField : MonoBehaviour
{
    Rigidbody2D body;
    Animator anim;

    Vector2 inputVec;
    Vector2 targetPos;

    const float stopDistance = 0.1f;
    const float speed = 1.0f;

    int noByStage = 0;
    int dataKey = 0;

    void Awake()
    {
        body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0.0f;

        anim = GetComponentInChildren<Animator>();

        targetPos = body.position;
    }
    void FixedUpdate()
    {
        float distance = Vector2.Distance(body.position, targetPos);

        // ��ǥ�� �����ϸ� ���ο� ��ġ ���
        if (distance < stopDistance)
        {
            inputVec = Random.insideUnitCircle/*.normalized*/ * 3.0f;
            Vector2 newTarget = body.position + inputVec;

            // �̵� ���� ����
            Bounds bounds = FieldManager.Instance.MoveAreaBounds;
            float clampedX = Mathf.Clamp(newTarget.x, bounds.min.x, bounds.max.x);
            float clampedY = Mathf.Clamp(newTarget.y, bounds.min.y, bounds.max.y);
            targetPos = new Vector2(clampedX, clampedY);
        }

        body.MovePosition(Vector2.MoveTowards(body.position, targetPos, speed * Time.fixedDeltaTime));
    }
    void LateUpdate()
    {
        anim.SetFloat("Speed", (targetPos - (Vector2)transform.position).magnitude);
        if ((targetPos - (Vector2)transform.position).magnitude < 0.1f)
            return;

        Filper.InputFilp(transform, targetPos - (Vector2)transform.position);
    }
    public void SetIdentity(int _no, int _key)
    {
        noByStage = _no;
        dataKey = _key;
    }
    public void GetIdentity(out int _no, out int _key)
    {
        _no = noByStage;
        _key = dataKey;
    }
    public Bounds GetTotalBounds()
    {
        var totalBounds = new Bounds(transform.position, Vector3.zero);

        var renderers = transform.GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0)
            return totalBounds;

        int filterLayer = LayerMask.NameToLayer("MiniMap"); // ���̾� ����
        var filtered = renderers.Where(r => r.gameObject.layer != filterLayer).ToArray();

        totalBounds = filtered[0].bounds;
        foreach (var r in filtered)
        {
            totalBounds.Encapsulate(r.bounds); // �ٸ� �ڽ� ��������Ʈ���� ����
        }
        return totalBounds;
    }






























#if QA
    void OnDrawGizmos()
    {
        Bounds _cachedBounds = GetTotalBounds();
        bool _hasBounds = _cachedBounds.size.sqrMagnitude > 0f;

        if (!_hasBounds) return;

        // 3D WireCube(���� AABB)
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_cachedBounds.center, _cachedBounds.size);

        // 2D �ܰ���(XY ��鿡 �� ��)
        Gizmos.color = Color.cyan;
        Vector3 min = _cachedBounds.min;
        Vector3 max = _cachedBounds.max;

        // z�� ����� ����(2D ������Ʈ ����)
        float z = _cachedBounds.center.z;
        Vector3 a = new Vector3(min.x, min.y, z);
        Vector3 b = new Vector3(max.x, min.y, z);
        Vector3 c = new Vector3(max.x, max.y, z);
        Vector3 d = new Vector3(min.x, max.y, z);

        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
        Gizmos.DrawLine(d, a);

        // �ʺ�/���� ��
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
