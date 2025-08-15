using UnityEngine;
using UnityEngine.UI;

public class VideoBackgroundScaler : MonoBehaviour
{
    public RectTransform videoRectTransform;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
        StretchToFullScreen();
    }

    void StretchToFullScreen()
    {
        // Устанавливаем якоря в уголки
        videoRectTransform.anchorMin = new Vector2(0, 0);
        videoRectTransform.anchorMax = new Vector2(1, 1);
        // Сбросируем смещения
        videoRectTransform.offsetMin = Vector2.zero;
        videoRectTransform.offsetMax = Vector2.zero;
    }
}
