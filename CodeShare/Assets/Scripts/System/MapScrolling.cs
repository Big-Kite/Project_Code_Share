using UnityEngine;

public class MapScrolling : MonoBehaviour
{
    Transform cameraTransform;   // ���� ī�޶�
    [SerializeField] float parallaxMultiplier;  // ��� �̵� ���� (0~1)

    Vector3 previousCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        previousCameraPosition = cameraTransform.position;
    }
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;
        Vector3 parallaxMovement = new Vector3(deltaMovement.x * parallaxMultiplier, 0.0f, 0.0f);

        transform.position += parallaxMovement;

        previousCameraPosition = cameraTransform.position;
    }
}
