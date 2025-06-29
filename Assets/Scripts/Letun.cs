using UnityEngine;

public class Letun : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 2f; // �������� ��������
    [SerializeField] private float movementHeight = 3f; // ������ ��������
    [SerializeField] private float damageAmount = 1f; // ���� ������

    [Header("Visual Settings")]
    [SerializeField] private ParticleSystem hitEffect; // ������ ��� ������������

    private Vector3 startPosition;
    private bool movingUp = true;
    private float randomOffset; // ��� ������������� ��������

    void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI); // ��������� �������� ����
    }

    void Update()
    {
        // ������� �������� �����-���� �� ���������
        float newY = startPosition.y + Mathf.Sin((Time.time + randomOffset) * movementSpeed) * movementHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ������� ���� ������
            Hero player = other.GetComponent<Hero>();
            if (player != null)
            {
                player.GetDamage();

                // ���������� ������
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
                }
            }
        }
    }

    // ����� ��� ��������� ���������� (�����������)
    public void SetMovementParameters(float speed, float height)
    {
        movementSpeed = speed;
        movementHeight = height;
    }
}