using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D body;
    int type = 0;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    public void Init(int _type, Vector3 _dir, float _speed)
    {
        type = _type;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        body.linearVelocity = _dir * _speed;
    }
    void OnTriggerEnter2D(Collider2D _collision)
    {
        var battleUnit = _collision.gameObject.GetComponent<PlayerInBattle>();
        if (battleUnit != null)
        {
            EffectController.Instance.ShowBattleEffect((BattleEffect)(Define.ProjectileSet[type, 2]), battleUnit.Front.transform.position, 0.5f, _randRotate: true);
            battleUnit.TakeDamage(1);

            gameObject.SetActive(false);
        }
    }
}
