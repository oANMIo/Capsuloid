using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float trackingSpeed = 3f;
    [SerializeField] private float maxSpeedFactor = 2f;
    [SerializeField] private float deadZoneRadius = 1.5f;
    [SerializeField] private float smoothTime = 2.2f; // Новая переменная для плавности

    private Camera cam;
    private Vector3 velocity;
    private float baseOrthographicSize;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();
        baseOrthographicSize = 9f;
        cam.orthographicSize = baseOrthographicSize;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 cameraCenter = new Vector3(transform.position.x, transform.position.y, player.position.z);
        float distanceToPlayer = Vector3.Distance(cameraCenter, player.position);

        if (distanceToPlayer <= deadZoneRadius)
        {
            return;
        }

        float speedFactor = Mathf.Clamp(
            (distanceToPlayer - deadZoneRadius) / (baseOrthographicSize - deadZoneRadius),
            1f,
            maxSpeedFactor
        );

        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z
        );

        // Используем smoothTime для управления плавностью
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime / (trackingSpeed * speedFactor) // Учитываем smoothTime
        );

        // Опциональный динамический зум
        float targetZoom = baseOrthographicSize + (distanceToPlayer * 0.1f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * trackingSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, deadZoneRadius);
    }
}