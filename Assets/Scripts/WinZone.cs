using UnityEngine;

public class WinZone : MonoBehaviour
{
    [SerializeField] private string nextLevelName; // ��� ��������� �����
    [SerializeField] private GameObject levelCompleteMenu; // ������ �� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f; // ������������� ����
            levelCompleteMenu.SetActive(true); // ���������� ����
        }
    }
}