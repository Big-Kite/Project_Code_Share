using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : Singleton<FieldManager>
{
    [SerializeField] GameObject tombstonePrefab;
    [SerializeField] GameObject npcNameTagPrefab;
    [SerializeField] GameObject targetMarkPrefab;
    GameObject targetMark = null;

    public BoxCollider2D MoveArea { get; private set; } = null; // 추적 맵 공간
    public Vector2 MoveAreaMin { get; private set; } = Vector2.zero; // 맵(지형)의 최소/최대 경계
    public Vector2 MoveAreaMax { get; private set; } = Vector2.zero;
    public Bounds MoveAreaBounds { get; private set; } = new Bounds(); // 캐릭터 무브 바운드

    List<NpcInField> spawnedNpcs = new List<NpcInField>();

    void Awake()
    {
        BoxCollider2D[] candidates = FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None);
        foreach (BoxCollider2D candidate in candidates)
        {
            if (candidate.gameObject.activeSelf && candidate.gameObject.CompareTag("MoveArea"))
            {
                MoveArea = candidate;
                break;
            }
        }
        if (MoveArea != null)
        {
            MoveAreaMin = MoveArea.bounds.min;
            MoveAreaMax = MoveArea.bounds.max;

            Bounds fixedBounds = MoveArea.bounds;

            Vector3 min = fixedBounds.min * 0.95f;
            Vector3 max = fixedBounds.max * 0.95f;
            fixedBounds.min = min;
            fixedBounds.max = max;

            MoveAreaBounds = fixedBounds;
        }
    }
    IEnumerator Start()
    {
        yield return YieldCache.WaitForSeconds(2.0f);

        spawnedNpcs.Clear();

        if (StageManager.Instance.Loaded())
            CreateNpcs_Continue();
        else
            CreateNpcs_New();

        SoundManager.Instance.SetBgmVolume(0.2f);
        SoundManager.Instance.PlayBgm(BGM.BGM_Field);
    }
    void CreateNpcs_Continue()
    {
        var stageMons = StageManager.Instance.GetLoadedStageMonster();
        for (int index = 0; index < stageMons.Count; index++)
        {
            Vector2 spawnPos = Random.insideUnitCircle * 20.0f;
            float clampedX = Mathf.Clamp(spawnPos.x, MoveAreaBounds.min.x, MoveAreaBounds.max.x);
            float clampedY = Mathf.Clamp(spawnPos.y, MoveAreaBounds.min.y, MoveAreaBounds.max.y);
            spawnPos = new Vector2(clampedX, clampedY);

            // 데이터 가져오기
            var npcGroupData = stageMons[index].npcGroupData;
            if (stageMons[index].hunted)
            {
                spawnPos = stageMons[index].fieldPos;
                var tombstone = Instantiate(tombstonePrefab, (Vector3)spawnPos, Quaternion.identity);
                
                var dir = PlayerInField.Instance.transform.position - (Vector3)spawnPos;
                Filper.Filp(tombstone.transform, dir.x > 0);
            }
            else
            {
                // 오브젝트 등록
                var representNpcDataSO = DataRef.GetNpcDB.GetNpcData(npcGroupData.Units[0]);
                var representNpcGO = Instantiate(representNpcDataSO.prefab, (Vector3)spawnPos, Quaternion.identity);
                var representNpc = representNpcGO.AddComponent<NpcInField>();
                representNpc.SetIdentity(stageMons[index].NoByStage, npcGroupData.Key);
                spawnedNpcs.Add(representNpc);

                bool boss = stageMons[index].NoByStage == 1;
                CreateMiniMapDisplay(representNpcGO, boss);
                CreateTagAndCollision(representNpcGO, boss, representNpcDataSO);
                CreateUnitHudCanvas(representNpcGO.transform);
            }
        }
    }
    void CreateNpcs_New()
    {
        var curStageSO = StageManager.Instance.GetCurStageDataSO();
        for (int index = 0; index < curStageSO.data.SpawnNormal; index++)
        {
            Vector2 spawnPos = Random.insideUnitCircle * 20.0f;
            float clampedX = Mathf.Clamp(spawnPos.x, MoveAreaBounds.min.x, MoveAreaBounds.max.x);
            float clampedY = Mathf.Clamp(spawnPos.y, MoveAreaBounds.min.y, MoveAreaBounds.max.y);
            spawnPos = new Vector2(clampedX, clampedY);

            // 데이터 등록
            int npcGroupDataKey = index == 0 ? curStageSO.data.Boss : Random.Range(curStageSO.data.GroupMin, curStageSO.data.GroupMax + 1);
            var npcGroupData = DataRef.GetNpcDB.GetNpcGroupData(npcGroupDataKey).data;
            StageManager.Instance.AddSpawnedNpcGroupData(index + 1, npcGroupData);

            // 오브젝트 등록
            var representNpcDataSO = DataRef.GetNpcDB.GetNpcData(npcGroupData.Units[0]);
            var representNpcGO = Instantiate(representNpcDataSO.prefab, (Vector3)spawnPos, Quaternion.identity);
            var representNpc = representNpcGO.AddComponent<NpcInField>();
            representNpc.SetIdentity(index + 1, npcGroupDataKey);
            spawnedNpcs.Add(representNpc);

            bool boss = index == 0;
            CreateMiniMapDisplay(representNpcGO, boss);
            CreateTagAndCollision(representNpcGO, boss, representNpcDataSO);
            CreateUnitHudCanvas(representNpcGO.transform);
        }
        StageManager.Instance.CompleteLoad();
    }
    void CreateTagAndCollision(GameObject _object, bool _boss, NpcDataSO _dataSO)
    {
        _object.tag = "FieldMonster";

        var boxCollision = _object.AddComponent<BoxCollider2D>();
        boxCollision.isTrigger = true;
        boxCollision.size = new Vector2(2.0f, 2.0f);

        var nameTagGO = Instantiate(npcNameTagPrefab, Vector3.zero, Quaternion.identity);
        nameTagGO.GetComponent<NpcNameTag>().SetOwner(_object, _dataSO);

        if (_boss)
            nameTagGO.GetComponent<NpcNameTag>().SetBossColor();
    }
    void CreateMiniMapDisplay(GameObject _object, bool _boss)
    {
        GameObject minimapDisplayGO = new GameObject("MiniMapDisplay"); // 미니맵 디스플레이
        minimapDisplayGO.transform.SetParent(_object.transform);
        minimapDisplayGO.transform.localScale = new Vector3(1.5f, 1.0f, 1.0f);
        minimapDisplayGO.transform.localPosition = Vector3.zero;

        SpriteRenderer spriteRenderer = minimapDisplayGO.AddComponent<SpriteRenderer>();
        Texture2D texture = new Texture2D(16, 16);
        texture.Apply();
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);
        spriteRenderer.color = _boss ? Color.red : Color.yellow;

        int layerIndex = LayerMask.NameToLayer("MiniMap"); // 레이어 설정
        minimapDisplayGO.layer = layerIndex;
    }
    void CreateUnitHudCanvas(Transform _transform, string _name = "UnitHUD")
    {
        var hudGO = new GameObject(_name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        hudGO.layer = LayerMask.NameToLayer("Default");   // 스샷과 동일

        var rt = hudGO.GetComponent<RectTransform>(); // 부모 설정
        rt.SetParent(_transform, worldPositionStays: false);
        rt.anchorMin = Vector2.zero;     // Min (0,0)
        rt.anchorMax = Vector2.zero;     // Max (0,0)
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition3D = Vector3.zero;
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;
        rt.sizeDelta = new Vector2(1f, 1f); // Width/Height=1

        var canvas = hudGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;     // World Space
        canvas.worldCamera = null;                     // Event Camera = None
        canvas.sortingLayerID = SortingLayer.NameToID("Default");
        canvas.sortingOrder = 20;                    // Order in Layer = 20
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;   // Additional Shader Channels

        var scaler = hudGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.dynamicPixelsPerUnit = 1f;   // Dynamic Pixels Per Unit = 1
        scaler.referencePixelsPerUnit = 100f; // Reference Pixels Per Unit = 100

        var raycaster = hudGO.GetComponent<GraphicRaycaster>();
        raycaster.ignoreReversedGraphics = true;
        raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
        raycaster.blockingMask = ~0; // Everything
    }
    public void SetTarget(NpcInField _target)
    {
        if (_target == null)
        {
            if (targetMark != null)
                targetMark.SetActive(false);

            return;
        }

        var hud = _target.transform.Find("UnitHUD");
        
        targetMark ??= Instantiate(targetMarkPrefab, hud);

        targetMark.transform.SetParent(hud);
        targetMark.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        targetMark.GetComponent<RectTransform>().sizeDelta = _target.GetTotalBounds().size * 1.2f;
        targetMark.gameObject.SetActive(true);

        _target.GetIdentity(out int no, out int key);
        StageManager.Instance.SetTarget(no, key, _target.transform.position);
    }
    public void SetTargetNull()
    {
        SetTarget(null);
        StageManager.Instance.SetTargetNull();
    }
}













/*    bool IsTouchOverUI()
    {
        if (TutorialManager.Instance.IsActive == false)
            return false;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN    
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }
    void Update()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            //if (IsTouchOverUI())
            //    return;
        
            //Vector3 touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            //joystick.anchoredPosition = touchPos;
            //joystick.localScale = Vector3.one;
            //RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, Mathf.Infinity, touchLayer);
            //if (hit && hit.transform.CompareTag("Block"))
            //{
            //    canSecond = true;
            //    firstBlock = hit.transform.GetComponentInParent<ActiveBlock>();
            //    firstBlock.PlayChoice();
            //
            //    beginPos = firstBlock.transform.position;
            //    swapDir = Vector2.zero; // 스왑 방향 초기화
            //    SoundManager.Instance.PlaySfx(SFX.BlockSelect_AU1); //
            //}
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //joystick.localScale = Vector3.zero;
        }
#else
        if (Input.touchCount > 0)
        {
            if (IsTouchOverUI())
                return;
            
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        Vector3 touchPos = puzCam.ScreenToWorldPoint(touch.position);
                        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, Mathf.Infinity, touchLayer);
                        if (hit && hit.transform.CompareTag("Block"))
                        {
                            canSecond = true;
                            firstBlock = hit.transform.GetComponentInParent<ActiveBlock>();
                            firstBlock.PlayChoice();
            
                            beginPos = firstBlock.transform.position;
                            swapDir = Vector2.zero; // 스왑 방향 초기화
                            SoundManager.Instance.PlaySfx(SFX.BlockSelect_AU1); //
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    {
                        if (firstBlock != null && canSecond)
                        {
                            Vector2 movedtouchPos = puzCam.ScreenToWorldPoint(touch.position);
                            Vector2 direction = movedtouchPos - beginPos; // 스왑 방향 계산
                            if (direction.magnitude > 0.6f) // 충분히 이동했을 때만 방향 판단
                            {
                                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                    swapDir = direction.x > 0 ? Vector2.right : Vector2.left; // 좌우
                                else
                                    swapDir = direction.y > 0 ? Vector2.up : Vector2.down; // 상하
            
                                userTouch = true;
            
                                firstBlock.StopAction();
                                TrySwap(swapDir); // 스왑 조건 달성했으면 스왑 시도
                            }
                        }
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    {
                        if (firstBlock != null && canSecond)
                        {
                            canSecond = false;
                            firstBlock.StopAction();
                        }
                    }
                    break;
            }
        }
#endif
    }
*/
