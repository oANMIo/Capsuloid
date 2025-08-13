using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.EventSystems; // Добавляем для работы с событиями UI

public class MenuManager : MonoBehaviour, IPointerClickHandler // Реализуем интерфейс для обработки кликов
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject Autors; // Ссылка на объект с информацией об авторах

    private bool isVideoPlaying = false;
    private bool isAuthorsVisible = false; // Флаг видимости окна авторов

    [Tooltip("Саунд-эффект, который проигрывается при открытии/закрытии")]
    public AudioClip clickSound;
    public float volume = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.gameObject.SetActive(false);
        }

        // Скрываем окно авторов при старте
        if (Autors != null)
        {
            Autors.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Если окно авторов активно, скрываем его при клике
        if (isAuthorsVisible)
        {
            PlayClickSound();
            Autors.SetActive(false);
            isAuthorsVisible = false;
        }
    }

    public void PlayGame()
    {
        if (videoPlayer != null && !isVideoPlaying)
        {
            PlayClickSound();
            menuUI.SetActive(false);
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
            isVideoPlaying = true;
        }
        else
        {
            SceneManager.LoadScene(1);
        }
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

    public void Exit()
    {
        PlayClickSound();
        Application.Quit();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }

    // Обновляем метод для обработки кликов вне UI элементов
    private void Update()
    {
        if (isAuthorsVisible && Input.GetMouseButtonDown(0))
        {
            // Проверяем, что клик не по UI элементу
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Autors.SetActive(false);
                isAuthorsVisible = false;
            }
        }
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
        }
    }
}