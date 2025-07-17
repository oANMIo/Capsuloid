using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 12.5f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private Animator animator; // ������ �� Animator ��� ������� ���������
    [SerializeField] private float hitAnimationDuration = 1f; // ������������ �������� ���������

    private Transform target;
    private float spawnTime;
    private bool isHit = false; // ���� ���������

    void Start()
    {
        spawnTime = Time.time;

        // ��������� Animator ���� �� ��������
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (isHit) return; // �� ��������� ����� ���������

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }

        if (Time.time > spawnTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isHit) return; // ���������� ��������� ���������

        if (other.CompareTag("Enemy"))
        {
            // ��������� �������� ���������
            if (animator != null)
            {
                animator.SetTrigger("Hit");
                isHit = true;
            }

            // ������� ����
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage();
            }

            // ���������� ���� ����� ��������
            if (hitAnimationDuration > 0)
                Destroy(gameObject, hitAnimationDuration);
            else
                Destroy(gameObject);
        }
    }
}