using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public bool PauseGame { get; private set; }
    public GameObject PauseGameMenu;
    [Tooltip("Саунд-эффект, который проигрывается при открытии/закрытии")]
    public AudioClip clickSound;
    public float volume = 1f;
    public GameObject Autors;
    private bool isAuthorsVisible = false;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    public void Info()
    {
        if (Autors != null)
        {
            PlayClickSound();
            Autors.SetActive(true);
            isAuthorsVisible = true;
        }
    }

    public void CloseInfo()
    {
        // Если окно авторов активно, скрываем его при клике
        if (isAuthorsVisible)
        {
            PlayClickSound();
            Autors.SetActive(false);
            isAuthorsVisible = false;
        }
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
        PlayClickSound();
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
        PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menus");
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
        }
    }
}