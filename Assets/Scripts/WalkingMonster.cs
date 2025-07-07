using UnityEngine;

public class WalkingMonster : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private bool startMovingRight = true;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 1f;

    private Vector3 leftBound;
    private Vector3 rightBound;
    private bool movingRight;
    private SpriteRenderer sprite;
    private Transform player;
    private bool isChasing = false;
    private Animator animator;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        // ������������� ������ ��������
        leftBound = transform.position + Vector3.left * moveDistance;
        rightBound = transform.position + Vector3.right * moveDistance;
        movingRight = startMovingRight;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;

            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                animator.SetTrigger("Attack"); 
            }
        }
        else
        {
            isChasing = false;
            MoveBetweenPoints();
        }

        UpdateSpriteDirection();
    }

    private void MoveBetweenPoints()
    {

        Vector3 target = movingRight ? rightBound : leftBound;

        // ��������� � ����
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // ��������� ���������� �������
        if (Vector3.Distance(transform.position, target) < 1f)
        {
            movingRight = !movingRight; // ������ �����������
        }
    }

    private void ChasePlayer()
    {
        // ��������� � ������
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime);

        // ��������� ����������� ��� �������������
        movingRight = player.position.x > transform.position.x;
    }

    private void UpdateSpriteDirection()
    {
        // ������������� ������ � ����������� �� �����������
        sprite.flipX = movingRight;
    }

    private void OnDrawGizmosSelected()
    {
        // ������������ � ���������
        Gizmos.color = Color.cyan;
        Vector3 startPos = Application.isPlaying ? (leftBound + rightBound) * 0.5f : transform.position;
        float dist = Application.isPlaying ? Vector3.Distance(leftBound, rightBound) * 0.5f : moveDistance;

        Gizmos.DrawWireSphere(startPos + Vector3.left * dist, 0.2f);
        Gizmos.DrawWireSphere(startPos + Vector3.right * dist, 0.2f);
        Gizmos.DrawLine(startPos + Vector3.left * dist, startPos + Vector3.right * dist);

        // ������������ ������� �������������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // ������������ ������� �����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
            var hero = collision.gameObject.GetComponent<Hero>();
            if (hero != null) hero.GetDamage();
        }
    }
}