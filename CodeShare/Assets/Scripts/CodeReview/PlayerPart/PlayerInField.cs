using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInField : Singleton<PlayerInField>
{
    [SerializeField] ParticleSystem dust;
    bool dustPlay = false;

    Transform bodyTransform;
    Rigidbody2D body;
    Animator anim;

    Vector2 inputVec = Vector2.zero;
    float speed = 8.0f;

    List<Transform> inArea = new List<Transform>();

    void Awake()
    {
        bodyTransform = transform.Find("Ch");
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        GameObject child = new GameObject("SquareSprite");
        child.transform.SetParent(transform);
        child.transform.localScale = new Vector3(1.5f, 1.0f, 1.0f);
        child.transform.localPosition = Vector3.zero;

        // SpriteRenderer 추가 및 기본 스프라이트 설정
        SpriteRenderer sr = child.AddComponent<SpriteRenderer>();
        Texture2D tex = new Texture2D(16, 16);
        tex.Apply();
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);
        // 레이어 설정
        int layerIndex = LayerMask.NameToLayer("MiniMap");
        child.layer = layerIndex;

        PlayerData.Instance.StoredEquipItem();
        StageManager.Instance.LoadPlayerPos(transform);
    }
    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (SceneTransition.Instance.Inaccessible)
            return;

        if (_collision.CompareTag("FieldMonster"))
        {
            var mon = _collision.GetComponent<NpcInField>();
            if (mon == null)
                return;

            inArea.Add(_collision.transform);
            FieldManager.Instance.SetTarget(mon);
        }
    }
    void OnTriggerExit2D(Collider2D _collision)
    {
        if (SceneTransition.Instance.Inaccessible)
            return;

        if (_collision.CompareTag("FieldMonster"))
        {
            inArea.Remove(_collision.transform);
        }
        if (inArea.Count <= 0)
        {
            FieldManager.Instance.SetTargetNull();
        }
    }
    void OnMove(InputValue _value)
    { // update PlayerInput component
        inputVec = _value.Get<Vector2>();
    }
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        Vector2 targetPos = body.position + nextVec;

        // 이동 제한 적용
        Bounds bounds = FieldManager.Instance.MoveAreaBounds;
        float clampedX = Mathf.Clamp(targetPos.x, bounds.min.x, bounds.max.x);
        float clampedY = Mathf.Clamp(targetPos.y, bounds.min.y, bounds.max.y);
        targetPos = new Vector2(clampedX, clampedY);

        body.MovePosition(targetPos);
    }
    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.magnitude < 0.1f)
        {
            if (dustPlay)
            {
                dustPlay = false;
                dust?.Stop();
            }
            return;
        }

        if (!dustPlay)
        {
            dustPlay = true;
            dust?.Play();
        }

        Filper.InputFilp(bodyTransform, inputVec);
    }
}

//void Update()
//{ 이동방식 레거시
//    inputVec.x = Input.GetAxisRaw("Horizontal");
//    inputVec.y = Input.GetAxisRaw("Vertical");
//    inputVec = inputVec.normalized;
//
//    body.AddForce(inputVec); // 힘 부여
//    body.linearVelocity = inputVec; // 속도 제어
//    body.MovePosition(body.position + inputVec); // 위치 이동
//}
