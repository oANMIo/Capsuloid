using UnityEngine;

public class Sentry : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxAmmo = 5;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    public AudioSource audioSource;
    [SerializeField] private AudioClip ShootSound;

    private Animator animator;
    private int currentAmmo;
    private Transform enemy;
    private float lastAttackTime;

    void Start()
    {
        currentAmmo = maxAmmo;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (TryFindNearestEnemy()) // “еперь возвращает true только при наличии врага
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
        return enemy != null; 
    }

    public void Attack()
    { 
        if (bulletPrefab == null || enemy == null || firePoint == null)
        {
            return;
        }

        Debug.Log($"Attacking enemy: {enemy.name}");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (enemy.position - firePoint.position).normalized;

        Destroy(bullet, 5f);
        audioSource.PlayOneShot(ShootSound);
        currentAmmo--;
        lastAttackTime = Time.time;
        animator.SetBool("SeeEnemy", true);
    }

    private void Die()
    {
        Destroy(gameObject, 0.5f);
    }
}