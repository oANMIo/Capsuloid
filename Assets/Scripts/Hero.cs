using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private int healthPoints = 3;

    [Header("Turret Settings")]
    [SerializeField] private GameObject sentryPrefab; // Префаб турели (Sentry)
    [SerializeField] private float spawnDistance = 2f; // Дистанция спавна перед игроком
    [SerializeField] private KeyCode spawnKey = KeyCode.E; // Клавиша для спавна

    public static Hero Instance { get; set; }

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private Text winText; // Текст победы

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    void Update()
    {
        Instance = this;
        float movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.05f)
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        sr.flipX = movement < 0;

        // Спавн турели по нажатию клавиши
        if (Input.GetKeyDown(spawnKey) && sentryPrefab != null)
        {
            SpawnSentry();
        }
    }

    private void SpawnSentry()
    {
        // Определяем направление спавна (перед игроком)
        Vector3 spawnDirection = sr.flipX ? Vector3.left : Vector3.right;

        // Спавн на уровне ног (основная позиция - 0.5f ниже)
        Vector3 spawnPosition = transform.position + spawnDirection * spawnDistance - new Vector3(0, 0.65f, 0);

        // Создаём турель
        GameObject sentry = Instantiate(sentryPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Turret spawned at: " + spawnPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WinZone"))
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (winText != null)
        {
            winText.text = "You won, congrats!";
            winText.gameObject.SetActive(true);
        }
        Debug.Log("You won!");
        Time.timeScale = 0f;
    }

    public void GetDamage()
    {
        healthPoints -= 1;
        Debug.Log(healthPoints);
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("You lost");
        Destroy(this.gameObject);
    }
}