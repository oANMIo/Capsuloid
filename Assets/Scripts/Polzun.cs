using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Polzun : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private bool startMovingRight = true;

    private Vector3 leftBound;
    private Vector3 rightBound;
    private bool movingRight;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        // Инициализация границ движения
        leftBound = transform.position + Vector3.left * moveDistance;
        rightBound = transform.position + Vector3.right * moveDistance;
        movingRight = startMovingRight;
    }

    private void Update()
    {
        MoveBetweenPoints();
        UpdateSpriteDirection();
    }

    private void MoveBetweenPoints()
    {
        // Выбираем текущую целевую точку
        Vector3 target = movingRight ? rightBound : leftBound;

        // Двигаемся к цели
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Проверяем достижение границы
        if (Vector3.Distance(transform.position, target) < 1f)
        {
            movingRight = !movingRight; // Меняем направление
        }
    }

    private void UpdateSpriteDirection()
    {
        // Разворачиваем спрайт в зависимости от направления
        sprite.flipX = movingRight;
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация в редакторе
        Gizmos.color = Color.cyan;
        Vector3 startPos = Application.isPlaying ? (leftBound + rightBound) * 0.5f : transform.position;
        float dist = Application.isPlaying ? Vector3.Distance(leftBound, rightBound) * 0.5f : moveDistance;

        Gizmos.DrawWireSphere(startPos + Vector3.left * dist, 0.2f);
        Gizmos.DrawWireSphere(startPos + Vector3.right * dist, 0.2f);
        Gizmos.DrawLine(startPos + Vector3.left * dist, startPos + Vector3.right * dist);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var hero = collision.gameObject.GetComponent<Hero>();
            if (hero != null) hero.GetDamage();
        }
    }
}