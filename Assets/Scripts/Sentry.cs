using UnityEngine;

public class Sentry : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private int currentAmmo;
    private Transform enemy;
    private float lastAttackTime;

    void Start()
    {
        currentAmmo = maxAmmo;

        if (firePoint == null)
            Debug.LogError("FirePoint not assigned in Sentry script!", this);
        if (bulletPrefab == null)
            Debug.LogError("BulletPrefab not assigned in Sentry script!", this);
    }

    void Update()
    {
        // Добавим визуализацию радиуса обнаружения
        Debug.DrawRay(transform.position, Vector2.right * detectionRange, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * detectionRange, Color.red);

        if (TryFindNearestEnemy()) // Теперь возвращает true только при наличии врага
        {
            if (currentAmmo > 0 && Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
            }
            else if (currentAmmo <= 0)
            {
                Die();
            }
        }
    }

    private bool TryFindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemyObj in enemies)
        {
            if (enemyObj == null) continue;

            float distance = Vector3.Distance(transform.position, enemyObj.transform.position);
            if (distance < detectionRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyObj.transform;
            }
        }

        enemy = closestEnemy;
        return enemy != null; // Теперь правильно: true если враг найден
    }

    public void Attack()
    {
        if (bulletPrefab == null || enemy == null || firePoint == null)
        {
            Debug.LogError("Attack components missing!");
            return;
        }

        Debug.Log($"Attacking enemy: {enemy.name}");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (enemy.position - firePoint.position).normalized;

        // Альтернативный вариант без компонента Bullet
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            float bulletSpeed = 10f;
            bulletRb.velocity = direction * bulletSpeed;

            // Поворачиваем пулю в направлении движения
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            Debug.LogError("Bullet has no Rigidbody2D!", bullet);
        }

        Destroy(bullet, 5f);
        currentAmmo--;
        lastAttackTime = Time.time;
    }

    private void Die()
    {
        // Здесь можно добавить анимацию смерти
        Destroy(gameObject, 0.5f); // Задержка для проигрывания анимации
    }
}