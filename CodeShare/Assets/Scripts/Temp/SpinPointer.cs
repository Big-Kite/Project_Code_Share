using System.Collections;
using UnityEngine;

public class SpinPointer : MonoBehaviour
{
    SpinArea curArea;

    void Start()
    {
        StartCoroutine(CoSpin());
    }
    IEnumerator CoSpin()
    {
        yield return YieldCache.WaitForSeconds(1.0f);

        float time = 1.4f;
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            // 선형 보간
            float curvedT = t * t;
            float z = Mathf.Lerp(0f, 360f, curvedT);
            transform.rotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        // 정확히 360도로 맞춤
        transform.rotation = Quaternion.Euler(0, 0, 360f);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            curArea?.CheckOn();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        var area = collision.GetComponent<SpinArea>();
        if (area != null)
        {
            curArea = area;
        }
    }
}
