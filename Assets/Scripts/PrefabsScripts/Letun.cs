using UnityEngine;

public class Letun : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 2f; // Скорость движения
    [SerializeField] private float movementHeight = 3f; // Высота движения
    [SerializeField] private float damageAmount = 1f; // Урон игроку

    [Header("Visual Settings")]
    [SerializeField] private ParticleSystem hitEffect; // Эффект при столкновении

    private Vector3 startPosition;
    private bool movingUp = true;
    private float randomOffset; // Для вариативности движения

    void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI); // Случайное смещение фазы
    }

    void Update()
    {
        // Плавное движение вверх-вниз по синусоиде
        float newY = startPosition.y + Mathf.Sin((Time.time + randomOffset) * movementSpeed) * movementHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Наносим урон игроку
            Hero player = other.GetComponent<Hero>();
            if (player != null)
            {
                player.GetDamage();

                // Визуальный эффект
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
                }
            }
        }
    }

    // Метод для настройки параметров (опционально)
    public void SetMovementParameters(float speed, float height)
    {
        movementSpeed = speed;
        movementHeight = height;
    }
}