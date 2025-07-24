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
        // Получаем текущий номер сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Инкрементируем его на 1
        int nextSceneIndex = currentSceneIndex + 1;

        // Загружаем следующую сцену
        SceneManager.LoadScene(nextSceneIndex);
    }
}