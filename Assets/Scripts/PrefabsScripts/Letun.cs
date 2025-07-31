using UnityEngine;

public class Letun : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 2f; // �������� ��������
    [SerializeField] private float movementHeight = 3f; // ������ ��������
    [SerializeField] private float damageAmount = 1f; // ���� ������

    [Header("Visual Settings")]
    [SerializeField] private ParticleSystem hitEffect;

    private Vector3 startPosition;
    private bool movingUp = true;
    private float targetY;

    void Start()
    {
        startPosition = transform.position;
        // ������ ��������� ���� (����� ��� ����)
        targetY = startPosition.y + movementHeight;
    }

    void Update()
    {
        // ���������� ������ � ������� �������
        float step = movementSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, targetY, step), transform.position.z);

        // ��������� �������� �� �� ����� ���������� � ������ �����������
        if (Mathf.Approximately(transform.position.y, targetY))
        {
            // ������ ������� �������
            if (movingUp)
            {
                targetY = startPosition.y - movementHeight;
            }
            else
            {
                targetY = startPosition.y + movementHeight;
            }
            movingUp = !movingUp;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ������� ���� ������
            Hero hero = other.GetComponent<Hero>();
            if (hero != null)
            {
                hero.GetDamage();
                hero.ApplyKnockbackFromPosition(transform.position);
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