using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
<<<<<<< Updated upstream:Assets/Scripts/PrefabsScripts/Hero.cs
    public float knockbackStrength = 8f; // Настраиваемая сила отбрасывания
    public bool wasJumpingUp = false;
    public bool isDead = false;
=======
    public float knockbackStrength = 5f;
    public bool wasJumpingUp = false;
>>>>>>> Stashed changes:Assets/Scripts/Hero.cs

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (winText != null) winText.gameObject.SetActive(false);
    }

    [System.Obsolete]
    void Update()
    {
        Instance = this;
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
        if (movement != 0)
        {
            transform.position += new Vector3(movement, 0, 0) * speed * Time.deltaTime;
            animator.SetBool("Walk", true);
            if (movement > 0.1f) transform.localScale = new Vector3(-X, Y, 1);
            else if (movement < -0.1f) transform.localScale = new Vector3(X, Y, 1);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded || (Input.GetKeyDown(KeyCode.W) && isGrounded || (Input.GetKeyDown(KeyCode.Space) && isGrounded)))
        {
            audioSource.PlayOneShot(JumpSound);
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        animator.SetBool("IsGrounded", isGrounded);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleTurret()
    {
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
            animator.SetBool("Fall", !isGrounded && rb.velocity.y < 0);
        }
    }

    private void SpawnSentry()
    {
        if (!isGrounded)
        {
            return;
        }

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
        if (collision.CompareTag("WinZone")) WinGame();
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
        if (healthPoints <= 0) Die();
        else StartCoroutine(InvulnerabilityRoutine());
    }

    public void ApplyKnockbackFromPosition(Vector2 attackerPosition)
    {
        if (rb != null)
        {
            Vector2 direction;
            if (wasJumpingUp)
            {
<<<<<<< Updated upstream:Assets/Scripts/PrefabsScripts/Hero.cs
                // Отталкивать вверх, если прыгал
=======
>>>>>>> Stashed changes:Assets/Scripts/Hero.cs
                direction = Vector2.up;
            }
            else
            {
<<<<<<< Updated upstream:Assets/Scripts/PrefabsScripts/Hero.cs
                // Отталкиваем в сторону от врага
                direction = (transform.position - (Vector3)attackerPosition).normalized;
            }

            rb.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);

            // После использования сбрасываем флаг
            wasJumpingUp = false;
        }
=======
                direction = (transform.position - (Vector3)attackerPosition).normalized;
            }
            rb.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);
        }

        // После применения эффекта сбрасываем флаг
        wasJumpingUp = false;
>>>>>>> Stashed changes:Assets/Scripts/Hero.cs
    }

    public void Die()
    {
        isDead = true;
        if (animator != null) animator.SetTrigger("Die");
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
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

        Color originalColor = spriteRenderer.color;

        while (elapsed < duration)
        {
            // Чередуем прозрачность: 50% альфа (полупрозрачное состояние) и 100% (полностью видно)
            float alpha = (Mathf.FloorToInt(elapsed / flickerInterval) % 2 == 0) ? 0.5f : 1f;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Восстановить исходный цвет
        spriteRenderer.color = originalColor;
        isInvulnerable = false;
    }
}