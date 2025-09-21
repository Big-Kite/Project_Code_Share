using System.Collections;
using UnityEngine;

public class EffectController : Singleton<EffectController>
{
    // 코루틴을 이용하여 이펙트오브젝트를 풀링으로 가져와 연출시간동안 활성화 시킨 후 비활성으로 복구시키는 형태입니다.

    [SerializeField] Camera cam;
    [SerializeField] Pool effectPool;
    [SerializeField] Material[] materials;

    Transform camBody = null;

    //float screenX = 0.0f;
    //float screenY = 0.0f;

    void Start()
    {
        SetCam();
    }
    public void SetCam()
    {
        camBody = cam.transform;

        //screenY = cam.orthographicSize * 2;
        //screenX = screenY * cam.aspect;
    }
    public Material GetMaterial(int _index)
    {
        return materials[_index];
    }
    public void ShowShake(float _time, float _magnitude)
    {
        StartCoroutine(CoCamShake(_time, _magnitude));
    }
    IEnumerator CoCamShake(float _time, float _magnitude)
    {
        float routineTime = 0f;
        Vector3 originalPos = camBody.position;

        float seedX = Random.Range(0f, 100f);
        float seedY = Random.Range(0f, 100f);

        while (routineTime < _time)
        {
            routineTime += Time.deltaTime;

            float x = Mathf.PerlinNoise(seedX, Time.time * 10f) * 2f - 1f;
            float y = Mathf.PerlinNoise(seedY, Time.time * 10f) * 2f - 1f;

            camBody.position = new Vector3(originalPos.x + x * _magnitude, originalPos.y + y * _magnitude, originalPos.z);

            yield return null;
        }
        camBody.position = new Vector3(0.0f, 0.0f, -10.0f); ;
    }
    public void ShowFloatingUI(FloatingType _type, Vector3 _pos, int _damage)
    {
        var floatingUI = effectPool.GetComponentDirect<FloatingUI>((int)BattleEffect.Floating);
        floatingUI.SetFloatingUI(_type, _pos, _damage);
    }
    public void ShowBattleEffect(BattleEffect _effect, Vector3 _pos, float _duration, bool _randRotate = false)
    {
        StartCoroutine(CoShowEffect((int)_effect, _pos, _duration, _randRotate));
    }
    IEnumerator CoShowEffect(int _index, Vector3 _pos, float _duration, bool _randRotate)
    {
        GameObject effectInst = effectPool.GetObject(_index);
        if(_randRotate)
        {
            effectInst.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
        effectInst.transform.position = _pos;

        yield return YieldCache.WaitForSeconds(_duration);
        effectInst.gameObject.SetActive(false);
    }
    public void ShowAttachBattleEffect(BattleEffect _effect, BattleUnit _attachUnit, Vector3 _pos, float _duration, bool _randRotate = false)
    {
        StartCoroutine(CoAttachShowEffect((int)_effect, _attachUnit, _pos, _duration, _randRotate));
    }
    IEnumerator CoAttachShowEffect(int _index, BattleUnit _attachUnit, Vector3 _pos, float _duration, bool _randRotate)
    {
        GameObject effectInst = effectPool.GetObject(_index);
        effectInst.transform.SetParent(_attachUnit.transform);

        if (_randRotate)
        {
            effectInst.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
        effectInst.transform.position = _pos;

        yield return YieldCache.WaitForSeconds(_duration);
        effectInst.gameObject.SetActive(false);
    }
    public GameObject GetBattleProjectile(BattleEffect _effect)
    {
        GameObject projectileInst = effectPool.GetObject((int)_effect);
        return projectileInst;
    }
}
