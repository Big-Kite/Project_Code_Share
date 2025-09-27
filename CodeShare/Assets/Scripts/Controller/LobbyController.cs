using UnityEngine;
using UnityEngine.UI;

public class LobbyController : Singleton<LobbyController>
{
    [SerializeField] LobbyBase[] menus;
    [SerializeField] Button[] buttons;

    Color unSelected = new Color(0.11f, 0.13f, 0.25f);
    Color selected = new Color(0.33f, 0.38f, 0.62f);
    LobbyMenu curMenu = LobbyMenu.Home;

    void Awake()
    {
        if(menus.Length != buttons.Length)
        {
            Debug.LogError("The number is not the same! (menu, button)");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            LobbyMenu index = (LobbyMenu)i;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnClickButton(index));
            buttons[i].image.color = unSelected;

            menus[i].gameObject.SetActive(false);
        }
    }
    void Start()
    {
        buttons[(int)curMenu].image.color = selected;
        menus[(int)curMenu].gameObject.SetActive(true);

        PlayerData.Instance.StoredEquipItem();
    }
    void OnClickButton(LobbyMenu _menu)
    {
        if (curMenu == _menu)
            return;

        SoundManager.Instance.PlaySfx(SFX.ButtonTouch);

        buttons[(int)curMenu].image.color = unSelected;
        menus[(int)curMenu].gameObject.SetActive(false);

        curMenu = _menu;
        buttons[(int)curMenu].image.color = selected;
        menus[(int)curMenu].gameObject.SetActive(true);
    }
}
