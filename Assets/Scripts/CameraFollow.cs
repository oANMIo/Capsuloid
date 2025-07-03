using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Camera cam;

    void Start()
    {
        // Находим игрока и камеру
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();

        // Фиксируем зум на 9f
        cam.orthographicSize = 9f;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Камера следует за игроком только по X и Y
            transform.position = new Vector3(
                player.position.x,
                player.position.y,
                transform.position.z // Сохраняем исходную Z-позицию
            );
        }
    }
}