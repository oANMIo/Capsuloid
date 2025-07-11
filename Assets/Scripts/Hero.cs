using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    [System.Obsolete]
    private void HandleMovement()
    {
        // Обработка движения по горизонтали
        float movement = Input.GetAxis("Horizontal");

        if (movement != 0)
        {
            transform.position += new Vector3(movement, 0, 0) * speed * Time.deltaTime;
            animator.SetBool("Walk", true);

            // Поворот
            if (movement > 0.1f) transform.localScale = new Vector3(-X, Y, 1);
            else if (movement < -0.1f) transform.localScale = new Vector3(X, Y, 1);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        // Обработка прыжка через триггер
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded || (Input.GetKeyDown(KeyCode.W) && isGrounded || (Input.GetKeyDown(KeyCode.Space) && isGrounded)))
        {
            audioSource.PlayOneShot(JumpSound);
            animator.SetTrigger("Jump"); 
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        // Дополнительно: синхронизируем параметр isGrounded в аниматоре
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
                Destroy(currentSentry); // Уничтожаем турель при повторном нажатии
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

        audioSource.PlayOneShot(SentryUp);

        Vector3 spawnDirection = transform.localScale.x > 0 ? Vector3.left : Vector3.right;
        Vector3 spawnPosition = transform.position + spawnDirection * spawnDistance;

        // Проверяем, есть ли земля под местом спавна (чтобы турель не висела в воздухе)
        RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.down, 2f, groundLayer);
        if (hit.collider != null)
        {
            spawnPosition.y = hit.point.y + 0.1f; // Небольшой отступ от земли
        }

        if (currentSentry != null) Destroy(currentSentry);
        currentSentry = Instantiate(sentryPrefab, spawnPosition, Quaternion.identity);
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
        healthPoints--;
        HeartSystem.health -= 1;
        if (healthPoints <= 0) Die();
    }

    public void Die()
    {
        if (animator != null) animator.SetTrigger("Die");
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        HeartSystem.health -= healthPoints;
;
        Destroy(gameObject, 3f);
        Invoke("ReloadScene", 2f);
    }

    private void ReloadScene()
    {
        // Получаем индекс текущей сцены и перезагружаем её
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Восстанавливаем нормальную скорость игры (на случай, если она была изменена)
        Time.timeScale = 1f;
    }
}