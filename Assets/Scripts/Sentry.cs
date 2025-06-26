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
        if (TryFindNearestEnemy() == false)
            return;

        if (currentAmmo > 0 && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
        else if (currentAmmo <= 0)
        {
            Die();
        }
    }

    private bool TryFindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemyObj in enemies)
        {
            if (enemyObj == null) continue; // Пропускаем уничтоженные объекты

            float distance = Vector3.Distance(transform.position, enemyObj.transform.position);
            if (distance < detectionRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyObj.transform;
            }
        }
        enemy = closestEnemy;
        if (enemy == null) 
        { return true; }
        else {return false; }
    }

    public void Attack()
    {

        if (bulletPrefab == null)
        {
            Debug.LogError("BulletPrefab missing!");
            return;
        }

        Debug.Log($"Attacking enemy: {enemy.name} at position {enemy.position}");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();

        if (bulletComponent == null)
        {
            Debug.LogError("Instantiated bullet has no Bullet component!", bullet);
            return;
        }

        bulletComponent.SetTarget(enemy);
        currentAmmo--;
        lastAttackTime = Time.time;
    }

    private void Die()
    {
        // Здесь можно добавить анимацию смерти
        Destroy(gameObject, 0.5f); // Задержка для проигрывания анимации
    }

}