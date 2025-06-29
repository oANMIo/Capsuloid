using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    [SerializeField] private string mainMenuName = "MainMenu";

    public void NextLevel()
    {
        Time.timeScale = 1f; // Возобновляем время
        SceneManager.LoadScene(nextLevelName);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuName);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}