using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public CanvasGroup canvasGroup; // можно назначить через инспектор
    public float fadeDuration = 1.6f;

    private bool isFading;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        // Убедимся, что панель не видна в начале
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    // Международный метод для fade-out с колбэком
    public void FadeOut(System.Action onComplete = null)
    {
        if (isFading) return;
        StartCoroutine(Fade(1f, onComplete));
    }

    public void FadeIn(System.Action onComplete = null)
    {
        if (isFading) return;
        StartCoroutine(Fade(0f, onComplete));
    }

    private System.Collections.IEnumerator Fade(float targetAlpha, System.Action onComplete)
    {
        isFading = true;
        canvasGroup.blocksRaycasts = true; // блокируем ввод во время затухания
        float startAlpha = canvasGroup.alpha;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
        canvasGroup.blocksRaycasts = targetAlpha > 0.99f; // если полностью непрозрачен, блокируем ввод
        isFading = false;
        onComplete?.Invoke();
    }

}
