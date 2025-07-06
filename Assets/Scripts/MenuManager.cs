using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void Info()
    {
        SceneManager.LoadScene(4);
    }

    public void Exit()
    {
        Application.Quit();
    }
}