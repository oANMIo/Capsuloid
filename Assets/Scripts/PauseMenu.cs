using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public bool PauseGame { get; private set; }
    public GameObject PauseGameMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        // Автоматически снимаем паузу при загрузке меню
        if (newScene.name == "Menus")
        {
            if (PauseGame) Resume();
            PauseGameMenu.SetActive(false);
            enabled = false; // Отключаем скрипт в меню
        }
        else
        {
            enabled = true; // Включаем скрипт в игровых сценах
        }
    }

    // Обработка Esc через OnGUI (работает без Update)
    private void OnGUI()
    {
        if (!enabled) return; // Если скрипт выключен (в меню), игнорируем

        Event e = Event.current;
        if (e.isKey && e.keyCode == KeyCode.Escape && e.type == EventType.KeyUp)
        {
            if (PauseGame) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        PauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
    }

    public void Pause()
    {
        PauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menus");
    }
}