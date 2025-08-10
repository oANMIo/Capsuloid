using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Hero : MonoBehaviour
{
    private const int X = 7;
    private const int Y = 7;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private int healthPoints = 3;

    [Header("Turret Settings")]
    [SerializeField] private GameObject sentryPrefab;
    [SerializeField] private float spawnDistance = -200f;
    [SerializeField] private KeyCode spawnKey = KeyCode.E;

    private GameObject currentSentry;
    public static Hero Instance { get; set; }

    public float knockbackStrength = 8f;
    public bool wasJumpingUp = false;
    public bool isDead = false;

    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Text winText;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    public AudioSource audioSource;
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip SentryUp;

    private SpriteRenderer spriteRenderer;

    // Флаг для уязвимости
    private bool isInvulnerable = false;

    private void Awake()
    {
        // Безопасная инициализация
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    [System.Obsolete]
    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleTurret();
        HandleFallAnimation();
    }

    private void HandleMovement()
    {
        if (isDead)
            return;

        float movement = Input.GetAxis("Horizontal");
        if (movement != 0f)
        {
            transform.position += new Vector3(movement, 0f, 0f) * speed * Time.deltaTime;
            animator?.SetBool("Walk", true);

            // Разворот спрайта в зависимости от направления
            if (movement > 0.1f)
                transform.localScale = new Vector3(-X, Y, 1);
            else if (movement < -0.1f)
                transform.localScale = new Vector3(X, Y, 1);
        }
        else
        {
            animator?.SetBool("Walk", false);
        }

        bool jumpPressed = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space));
        if (jumpPressed && isGrounded)
        {
            audioSource?.PlayOneShot(JumpSound);
            animator?.SetTrigger("Jump");
            rb?.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (animator != null)
            animator.SetBool("IsGrounded", isGrounded);
    }

    private void CheckGrounded()
    {
        if (groundCheck == null)
        {
            isGrounded = true;
            return;
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleTurret()
    {
        if (spawnKey == KeyCode.None) return;

        if (Input.GetKeyDown(spawnKey))
        {
            if (currentSentry == null)
            {
                SpawnSentry();
            }
            else
            {
                Destroy(currentSentry);
                currentSentry = null;
            }
        }
    }

    [System.Obsolete]
    private void HandleFallAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("Fall", !isGrounded && rb != null && rb.velocity.y < 0);
        }
    }

    private void SpawnSentry()
    {
        if (!isGrounded || sentryPrefab == null)
            return;

        if (currentSentry != null)
        {
            Destroy(currentSentry);
            currentSentry = null;
            return;
        }

        Vector3 spawnDirection = transform.localScale.x > 0 ? Vector3.left : Vector3.right;
        Vector3 spawnPosition = transform.position + spawnDirection * spawnDistance;

        RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.down, 2f, groundLayer);

        if (hit.collider != null)
        {
            spawnPosition.y = hit.point.y + 0.1f;
            currentSentry = Instantiate(sentryPrefab, spawnPosition, Quaternion.identity);

            bool facingLeft = transform.localScale.x > 0;
            Sentry sentryScript = currentSentry.GetComponent<Sentry>();
            if (sentryScript != null)
            {
                sentryScript.SetFacingDirection(facingLeft);
            }

            if (audioSource != null && SentryUp != null)
            {
                audioSource.PlayOneShot(SentryUp);
            }
        }
        else
        {
            Debug.LogWarning("Не удалось найти землю для турели");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WinZone"))
            WinGame();
    }

    private void WinGame()
    {
        if (winText != null)
        {
            winText.text = "You won, congrats!";
            winText.gameObject.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void GetDamage()
    {
        if (isInvulnerable)
            return;

        healthPoints--;
        HeartSystem.health -= 1;
        ApplyKnockbackFromPosition(transform.position);
        if (healthPoints <= 0)
            Die();
        else
            StartCoroutine(InvulnerabilityRoutine());
    }

    public void ApplyKnockbackFromPosition(Vector2 attackerPosition)
    {
        if (rb == null)
            return;

        Vector2 direction;

        if (wasJumpingUp)
        {
            direction = Vector2.up;
        }
        else
        {
            // Отталкиваем от атакующего по направлению от него к игроку
            direction = ((Vector2)transform.position - attackerPosition).normalized;
        }

        rb.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);

        // После использования сбрасываем флаг
        wasJumpingUp = false;
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        animator?.SetTrigger("Die");

        Rigidbody2D rb2 = GetComponent<Rigidbody2D>();
        if (rb2 != null)
            rb2.simulated = false;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        HeartSystem.health -= healthPoints;
        Destroy(gameObject, 3f);
        Invoke("ReloadScene", 2f);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float duration = 2f;
        float elapsed = 0f;
        float flickerInterval = 0.2f;

        Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

        while (elapsed < duration)
        {
            // 50% прозрачность чередованием
            float alpha = (Mathf.FloorToInt(elapsed / flickerInterval) % 2 == 0) ? 0.5f : 1f;
            if (spriteRenderer != null)
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Восстановить исходный цвет
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        isInvulnerable = false;
    }
}