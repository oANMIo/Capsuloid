using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnTouch : MonoBehaviour
{
    public FadeController fadeController;

    private void Awake()
    {
        // Можно автоматом подцепить FadeController, если он на том же Canvas
        if (fadeController == null)
        {
            // Ищем по тегу/компоненту, если не прикреплен через Inspector
            var fc = FindObjectOfType<FadeController>();
            if (fc != null) fadeController = fc;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fadeController != null)
            {
                fadeController.FadeOut(() =>
                {
                    LoadNextLevel();
                    // После загрузки сцены сделайте FadeIn на новой сцене.
                    // Логика FadeIn будет вызвана после того, как сцена загрузится и этот скрипт/FadeController будет создан.
                    // Мы можем вызвать FadeIn здесь, но лучше запланировать на новой сцене после загрузки.
                    // Чтобы упростить, вызовем FadeIn прямо после загрузки на следующей сцене.
                    // Реализация ниже требует добавления вызова FadeIn в новой сцене (см. ниже).
                });
            }
            else
            {
                // Бэкап: просто загрузить без затухания
                LoadNextLevel();
            }
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Загрузка следующей сцены
        SceneManager.LoadScene(nextSceneIndex);
        // Примечание: FadeIn для новой сцены лучше вызывать не здесь, а в начале следующей сцены,
        // после ее загрузки. Это можно сделать путем добавления вызова fadeControllerFadeIn
        // в Start() следующей сцены, если FadeController доступен там.
    }

}