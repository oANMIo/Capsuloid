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
    private bool isFacingLeft = true; // �� ���������, ��� ����� ���������� ����� �����

    void Start()
    {
        currentAmmo = maxAmmo;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (TryFindNearestEnemy()) // ������ ���������� true ������ ��� ������� �����
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

    public void SetFacingDirection(bool facingLeft)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingLeft ? -1 : 1);
        transform.localScale = scale;
    }

    public void Attack()
    {
        if (bulletPrefab == null || enemy == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bool facingLeft = Mathf.Sign(transform.localScale.x) < 0;
        Vector2 direction = facingLeft ? Vector2.left : Vector2.right;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }

        // ���� ������ ��������� �����, ������������� ������ ���� �������� ����� //������
        if (isFacingLeft)
        {
            direction = Vector2.left;
        }

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