using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectController : Singleton<UnitSelectController>
{
    [SerializeField] GameObject buttonPrefab;

    [SerializeField] Button[] buttons;

    Color unPressed = Color.white;
    Color pressed = new Color(0.78f, 0.24f, 0.24f);

    public void OnCilckUnitSelect(int _index)
    {
        //foreach (var button in buttons)
        //{
        //    button.image.color = unPressed;
        //}
        //buttons[_index].image.color = pressed;
        //
        //BattleManager.Instance.SetSelectUnit(_index);
    }
    public void SetUnitButton(List<BattleUnit> _mons)
    {
        //buttons = new Button[_mons.Count];
        //
        //int unitIndex = 0;
        //foreach (var mon in _mons)
        //{
        //    GameObject unitButtonInst = Instantiate(buttonPrefab, transform);
        //    Button unitButton = unitButtonInst.GetComponent<Button>();
        //
        //    int curIndex = unitIndex;
        //    unitButton.onClick.RemoveAllListeners();
        //    unitButton.onClick.AddListener(() => OnCilckUnitSelect(curIndex));
        //
        //    buttons[unitIndex++] = unitButton;
        //}
        //OnCilckUnitSelect(0);
    }
}
