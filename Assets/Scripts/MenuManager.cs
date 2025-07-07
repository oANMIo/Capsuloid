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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Если окно авторов активно, скрываем его при клике
        if (isAuthorsVisible)
        {
            Autors.SetActive(false);
            isAuthorsVisible = false;
        }
    }

    public void PlayGame()
    {
        if (videoPlayer != null && !isVideoPlaying)
        {
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
            Autors.SetActive(true);
            isAuthorsVisible = true;
        }
    }

    public void Exit()
    {
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
}