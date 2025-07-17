using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 12.5f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private Animator animator; // Ссылка на Animator для эффекта попадания
    [SerializeField] private float hitAnimationDuration = 1f; // Длительность анимации попадания

    private Transform target;
    private float spawnTime;
    private bool isHit = false; // Флаг попадания

    void Start()
    {
        spawnTime = Time.time;

        // Автопоиск Animator если не назначен
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (isHit) return; // Не двигаемся после попадания

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
        if (isHit) return; // Игнорируем повторные попадания

        if (other.CompareTag("Enemy"))
        {
            // Запускаем анимацию попадания
            if (animator != null)
            {
                animator.SetTrigger("Hit");
                isHit = true;
            }

            // Наносим урон
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage();
            }

            // Уничтожаем пулю после анимации
            if (hitAnimationDuration > 0)
                Destroy(gameObject, hitAnimationDuration);
            else
                Destroy(gameObject);
        }
    }
}