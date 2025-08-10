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
    }

}