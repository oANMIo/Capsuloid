using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevelOnTouch : MonoBehaviour
{
    private Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
<<<<<<< Updated upstream
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        {
            animator = GetComponent<Animator>();
            animator.Play("Animation 1");
            SceneManager.LoadScene(nextSceneIndex);
        }
=======
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
>>>>>>> Stashed changes
    }

}