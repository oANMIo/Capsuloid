using UnityEngine;

public class Letun : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 2f; // Скорость движения
    [SerializeField] private float movementHeight = 3f; // Высота движения
    [SerializeField] private float damageAmount = 1f; // Урон игроку

    [Header("Visual Settings")]
    [SerializeField] private ParticleSystem hitEffect;

    private Vector3 startPosition;
    private bool movingUp = true;
    private float targetY;

    void Start()
    {
        startPosition = transform.position;
        // задаем начальную цель (вверх или вниз)
        targetY = startPosition.y + movementHeight;
    }

    void Update()
    {
        // Перемещаем объект к целевой позиции
        float step = movementSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, targetY, step), transform.position.z);

        // Проверяем достигли ли мы точки назначения и меняем направление
        if (Mathf.Approximately(transform.position.y, targetY))
        {
            // меняем целевую позицию
            if (movingUp)
            {
                targetY = startPosition.y - movementHeight;
            }
            else
            {
                targetY = startPosition.y + movementHeight;
            }
            movingUp = !movingUp;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Наносим урон игроку
            Hero hero = other.GetComponent<Hero>();
            if (hero != null)
            {
                hero.GetDamage();
                hero.ApplyKnockbackFromPosition(transform.position);
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