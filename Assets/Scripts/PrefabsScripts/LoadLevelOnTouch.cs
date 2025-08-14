using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnTouch : MonoBehaviour
{
    public FadeController fadeController;

    private void Awake()
    {
        // ����� ��������� ��������� FadeController, ���� �� �� ��� �� Canvas
        if (fadeController == null)
        {
            // ���� �� ����/����������, ���� �� ���������� ����� Inspector
            var fc = FindObjectOfType<FadeController>();
            if (fc != null) fadeController = fc;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fadeController != null)
            {
                fadeController.FadeOut(() =>
                {
                    LoadNextLevel();
                    // ����� �������� ����� �������� FadeIn �� ����� �����.
                    // ������ FadeIn ����� ������� ����� ����, ��� ����� ���������� � ���� ������/FadeController ����� ������.
                    // �� ����� ������� FadeIn �����, �� ����� ������������� �� ����� ����� ����� ��������.
                    // ����� ���������, ������� FadeIn ����� ����� �������� �� ��������� �����.
                    // ���������� ���� ������� ���������� ������ FadeIn � ����� ����� (��. ����).
                });
            }
            else
            {
                // �����: ������ ��������� ��� ���������
                LoadNextLevel();
            }
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // �������� ��������� �����
        SceneManager.LoadScene(nextSceneIndex);
        // ����������: FadeIn ��� ����� ����� ����� �������� �� �����, � � ������ ��������� �����,
        // ����� �� ��������. ��� ����� ������� ����� ���������� ������ fadeControllerFadeIn
        // � Start() ��������� �����, ���� FadeController �������� ���.
    }

}