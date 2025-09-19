using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] GameObject playerUnitPrefab;
    [SerializeField] GameObject monsterUnitPrefab;
    [SerializeField] Button PlayingButton;
    [SerializeField] GameObject targetMarkPrefab;
    GameObject targetMark = null;

    List<BattleUnit> allUnits = null;

    List<BattleUnit> npcs = new List<BattleUnit>();
    public List<BattleUnit> Npcs { get { return npcs; } }

    BattleUnit player = null;
    public BattleUnit Player {  get { return player; } }

    bool battleStart = false;

    void Awake()
    {
        Application.targetFrameRate = 120;
        battleStart = false;

        //PlayingButton.onClick.RemoveAllListeners();
        //PlayingButton.onClick.AddListener(OnClickPlayingButton);
    }
    IEnumerator Start()
    {
        battleStart = false;

        PopupManager.Instance.OpenLoading();
        CreateAndRegistBattleUnit();
        yield return YieldCache.WaitForSeconds(1.0f);

        UnitPlacement.Instance.SetUnitPlace(player, npcs);
        PopupManager.Instance.CloseLoading();

        yield return new WaitUntil(() => SceneTransition.Instance.Inaccessible == false);
        battleStart = true;
    }
    void Update()
    {
        if (!battleStart)
            return;

        UpdateUnits(Time.deltaTime);
    }
    void OnClickPlayingButton()
    {
        player.KeepGoing();
    }
    void UpdateUnits(float _delta)
    {
        foreach (BattleUnit unit in allUnits)
        {
            if (unit == null)
            {
                Debug.LogError("BattleCharacter is null!");
                continue;
            }
            if (unit.IsDead())// || unit is NpcInBattle)
                continue;

            unit.UnitStateUpdate(_delta);
        }
    }
    void CreateAndRegistBattleUnit()
    {
        allUnits = null;

        var npcDataKey = StageManager.Instance.GetCurNpcGroupDataKey();
        var npcGroupDataUnits = DataRef.GetNpcDB.GetNpcGroupData(npcDataKey).data.Units;
        for (int i = 0; i < npcGroupDataUnits.Length; i++)
        {
            if (npcGroupDataUnits[i] == 0)
                continue;

            NpcInBattle monsterUnit = Instantiate(monsterUnitPrefab, Vector3.zero, Quaternion.identity).GetComponent<NpcInBattle>();
            monsterUnit.Init(npcGroupDataUnits[i]);
        }

        BattleUnit[] units = FindObjectsByType<BattleUnit>(FindObjectsSortMode.None); // 배틀 캐릭터 전체 조사
        allUnits = new List<BattleUnit>(units);
        foreach (BattleUnit unit in allUnits) // 각 멤버에 등록
        {
            if (unit == null)
            {
                Debug.LogError("BattleCharacter is null!");
                continue;
            }
            switch (unit)
            {
                case PlayerInBattle playerUnit:
                    {
                        player = playerUnit;
                    }
                    break;
                case NpcInBattle npcUnit:
                    {
                        npcs.Add(npcUnit);
                    }
                    break;
            }
        }

        TotalHpSystem.Instance.Init(npcs);
    }
    public void CheckWinLose()
    {
        if (player.IsDead())
        {
            // 플레이어 패배
            StartCoroutine(CoBattleEnd(false));
        }
        else
        {
            bool allNpcsDead = true;
            foreach (var mon in npcs)
            {
                if (mon == null) continue;
                if (mon.IsDead() == false)
                { // 몬스터 무리 순회해서 한마리라도 살았으면 false 리턴
                    allNpcsDead = false;
                    break;
                }
            }

            if (allNpcsDead)
            {
                // 플레이어 승리
                StageManager.Instance.BattleWin();
                StartCoroutine(CoBattleEnd(true));
            }
        }
    }
    IEnumerator CoBattleEnd(bool _win)
    {
        PopupManager.Instance.OpenLoading();
        yield return YieldCache.WaitForSeconds(2.0f);

        PopupManager.Instance.OpenBattleResult();
        BattleResult.Instance.Init(_win);
        PopupManager.Instance.CloseLoading();
    }
    public void SetTarget(BattleUnit _target)
    {
        if (_target == null)
        {
            if (targetMark != null)
                targetMark.SetActive(false);

            return;
        }

        var hud = _target.transform.Find("UnitHUD");
        if (targetMark == null)
            targetMark = Instantiate(targetMarkPrefab, hud);

        targetMark.transform.SetParent(hud);
        targetMark.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        targetMark.GetComponent<RectTransform>().sizeDelta = _target.GetTotalBounds().size * (1 / _target.transform.localScale.x) * 1.2f;

        targetMark.gameObject.SetActive(true);
    }
}
