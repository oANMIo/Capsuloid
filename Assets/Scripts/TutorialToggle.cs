using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialToggle : MonoBehaviour
{
    public GameObject speechText;
    public GameObject tutorialPanel;

    private bool tutorialActive = false;
    private float tutorialOpenTime = 0f; // Время открытия

    void Start()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1;
        tutorialActive = false;

        // Проверка индекса сцены и установка активности speechText
        SetSpeechTextVisibility();
    }

    void Update()
    {
        if (!tutorialActive && Input.GetKeyDown(KeyCode.Tab))
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0;
            tutorialActive = true;
            tutorialOpenTime = Time.unscaledTime; // используем Time.unscaledTime при timeScale = 0
        }

        if (tutorialActive)
        {
            // Подождать 0.2 секунды перед закрытием (чтобы не поймать TAB повторно)
            if (Time.unscaledTime - tutorialOpenTime > 0.2f && Input.anyKeyDown)
            {
                tutorialPanel.SetActive(false);
                Time.timeScale = 1;
                tutorialActive = false;

                // При закрытии можно снова проверять сцену (если скрипт остаётся активным)
                SetSpeechTextVisibility();
            }
        }
    }

    private void SetSpeechTextVisibility()
    {
        // Проверяем индекс текущей сцены
        if (speechText != null)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            // Делаем видимым только на сцене с индексом 1
            speechText.SetActive(sceneIndex == 1);
        }
    }

}