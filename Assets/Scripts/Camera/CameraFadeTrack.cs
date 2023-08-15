using UnityEngine;

public class CameraFadeTrack : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 1f)]
    private float _radius = 0.3f;

    private Camera _camera;
    private Transform player;

    private void FixedUpdate()
    {
        Vector3 cameraPos = _camera.transform.position;
        Vector3 playerPos = player.position;

        Vector3 direction = (playerPos - cameraPos).normalized;

        RaycastHit[] results =
            Physics.SphereCastAll(cameraPos, _radius, direction, 100f);

        // TODO (Cheng Jun): Make the ray cast hit's sprite opacity low
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        player = GameObject.FindWithTag("Player").transform;
    }
}
