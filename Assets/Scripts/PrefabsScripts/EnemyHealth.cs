using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        // ���� AudioSource �� �������� � ����������, ��������� �������� ��� �������������
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        currentHealth -= 1;
        Debug.Log("Enemy hp = " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // ������������� ���� ������, ���� �� ����
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // ���������� ������ ����� ������������ �����
        Destroy(gameObject, deathSound != null ? deathSound.length : 0);
    }
}