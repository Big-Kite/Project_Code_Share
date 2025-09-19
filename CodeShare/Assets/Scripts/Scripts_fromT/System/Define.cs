using UnityEngine;

public enum LobbyMenu
{
    Home,
    Equip,
    Stat,
    Shop,
}
public enum BattleEffect
{
    Floating,
    Arrow,
    SwordHit,
    ArrowHit,
    Shoot,
    EnergyBall,
    EnergyHit,
    ShieldHit,
    IceColumn,
    Stun,
}
public enum FloatingType
{
    Damage,
    Heal,
    Protect,
}
public enum ItemMainType
{
    Coin = 1,
    Material,
    EquipItem
}
public enum CoinItemSubType
{
    Gold = 1,
    Dia,
    Event
}
public enum EquipItemSubType
{
    Weapon = 1,
    Shield,
    Helmet,
    Armor,
    Gauntlet,
    Boots
}
public enum PopupType
{
    ItemInfo,

}
public enum ItemGrade
{
    Common = 1,
    Advanced,
    Rare,
    Elite,
    Epic,
    Legend
}
public enum UnitState
{
    Idle,   // 대기
    Chase,  // 탐색
    Fight,  // 전투
    Dead    // 죽음
}
public enum DebuffType
{
    UnableAct,
    UnableHeal,
    UnableRegenMana,
    UnableSkill,
    PeriodicDamage
}
public static class Define
{
    public const string LobbyScene = "Lobby";
    public const string FieldScene = "Field";
    public const string BattleScene = "Battle";

    public const float MaxMeleeDist = 3.0f;
    public const float MinAtkSpeed = 3.0f;
    public const float MaxAtkSpeed = 0.0f;
    public const float DefaultInterval = 0.2f;

    public const int DefaultItemSlotCount = 20;

    public static readonly int[,] ProjectileSet = new int[3, 3]
    { // 발사체, 시작이펙트, 끝이펙트
        { 0, 0, 0 }, // 0번은 안쓰임
        { (int)BattleEffect.Arrow, (int)BattleEffect.Shoot, (int)BattleEffect.ArrowHit },
        { (int)BattleEffect.EnergyBall, (int)BattleEffect.Shoot, (int)BattleEffect.EnergyHit },
    };

    public static readonly Color[] GradeColor = new Color[(int)ItemGrade.Legend]
    {
        new Color(0.44f, 0.55f, 0.59f),
        new Color(0.0f, 0.83f, 0.35f),
        new Color(0.0f, 0.43f, 1.0f),
        new Color(0.0f, 0.33f, 0.26f),
        new Color(0.0f, 0.27f, 0.87f),
        new Color(1.0f, 0.88f, 0.0f)
    };
}
