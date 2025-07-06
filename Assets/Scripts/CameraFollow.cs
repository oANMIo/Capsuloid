using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Camera cam;

    void Start()
    {
        // ������� ������ � ������
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();

        // ��������� ��� �� 9f
        cam.orthographicSize = 9f;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // ������ ������� �� ������� ������ �� X � Y
            transform.position = new Vector3(
                player.position.x,
                player.position.y,
                transform.position.z // ��������� �������� Z-�������
            );
        }
    }
}