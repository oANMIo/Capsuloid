using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 12.5f;           // базова€ скорость (можно переопределить)
    [SerializeField] private float lifetime = 3f;           // врем€ жизни пули
    [SerializeField] private int damage = 1;                // урон
    [SerializeField] private Animator animator;              // анимаци€ попадани€
    [SerializeField] private float hitAnimationDuration = 1f; // врем€ анимации попадани€
    [SerializeField] private float bulletSpeed = 10f;       // скорость дл€ движени€ по направлению

    private Vector2 moveDirection = Vector2.right;         // направление движка (по умолчанию вправо)
    private Transform target;                                // цель (если есть)
    private float spawnTime;                                 // врем€ по€влени€
    private bool isHit = false;                              // флаг попадани€

    void Start()
    {
        spawnTime = Time.time;

        // јвтопоиск Animator, если не назначен
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // ћетод дл€ задани€ направлени€ пули
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        // ¬ариант, если нужно, мен€ть отображение пули или ее поворот
        if (moveDirection.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (isHit) return; // Ќе двигаемс€ после попадани€

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
        }

        if (Time.time > spawnTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isHit) return; // »гнорируем повторные попадани€

        if (other.CompareTag("Enemy"))
        {
            // «апускаем анимацию попадани€
            if (animator != null)
            {
                animator.SetTrigger("Hit");
                isHit = true;
            }

            // Ќаносим урон
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage();
            }

            // ”ничтожаем пулю после анимации
            if (hitAnimationDuration > 0)
                Destroy(gameObject, hitAnimationDuration);
            else
                Destroy(gameObject);
        }
    }
}