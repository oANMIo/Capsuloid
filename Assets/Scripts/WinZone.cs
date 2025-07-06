using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] private string nextLevelName; // Имя следующей сцены
    [SerializeField] private GameObject levelCompleteMenu; // Ссылка на меню

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f; // Останавливаем игру
            levelCompleteMenu.SetActive(true); // Показываем меню
        }
    }
}