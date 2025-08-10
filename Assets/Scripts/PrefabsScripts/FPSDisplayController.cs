using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplayController : MonoBehaviour
{
    public Text fpsText; // ссылка на текст FPS
    public Button toggleButton; // ссылка на кнопку
    private bool showFPS = false;
    private float deltaTime = 0.0f;

    void Start()
    {
        // Изначально FPS скрыт
        fpsText.gameObject.SetActive(false);
        toggleButton.onClick.AddListener(ToggleFPS);
        UpdateButtonText();
    }

    void Update()
    {
        if (showFPS)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }

    void ToggleFPS()
    {
        showFPS = !showFPS;
        fpsText.gameObject.SetActive(showFPS);
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        toggleButton.GetComponentInChildren<TextMeshProUGUI>().text = showFPS ? "Скрыть FPS" : "Показать FPS";
    }
}