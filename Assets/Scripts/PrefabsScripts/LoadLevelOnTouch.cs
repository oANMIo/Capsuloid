using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // �������� ������� ����� �����
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // �������������� ��� �� 1
        int nextSceneIndex = currentSceneIndex + 1;

        // ��������� ��������� �����
        SceneManager.LoadScene(nextSceneIndex);
    }
}