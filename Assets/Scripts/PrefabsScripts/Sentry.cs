using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Sprite[] ammoBarSprites;
    [SerializeField] private AmmoBar ammoBar; // привяжи в инспекторе

    private Animator animator;
    private int currentAmmo;
    private Transform enemy;
    private float lastAttackTime;
    private bool isFacingLeft = true; // по умолчанию, или можно установить через метод



    void Start()
    {
        if (ammoBar == null)
        {
            var barObj = GameObject.FindWithTag("AmmoBarImage"); // тег на объекте Image
            if (barObj != null)
            {
                ammoBar = FindObjectOfType<AmmoBar>();
            }
        }
            currentAmmo = maxAmmo;
            // если AmmoBar нашёл Image по тегу и собрал спрайты, можно просто обновить состояние
            ammoBar?.UpdateAmmoUI(currentAmmo);
            animator = GetComponent<Animator>();
        
    }
        void Update()
        {
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

        private void UpdateAmmoUI()
        {
            if (ammoBar != null)
            {
                ammoBar?.UpdateAmmoUI(currentAmmo);
            }
            else
            {
                return;
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

            // Если турель повернута влево, принудительно задаем пуле движение влево //убрать
            if (isFacingLeft)
            {
                direction = Vector2.left;
            }

            Destroy(bullet, 5f);
            audioSource.PlayOneShot(ShootSound);
            currentAmmo--;
            ammoBar?.UpdateAmmoUI(currentAmmo);
            lastAttackTime = Time.time;
            animator.SetBool("SeeEnemy", true);
        }

        private void Die()
        {
            ammoBar?.UpdateAmmoUI(maxAmmo);
            Destroy(gameObject, 0.5f);
        }
    }
