using UnityEngine;
using System.Collections.Generic;

public class CustomCursor : MonoBehaviour
{
    [Tooltip("Ѕазова€ текстура курсора. ƒобавьте варианты размеров в список ниже.")]
    public Texture2D cursorTexture;

    [Header("Cursor Size (pixels)")]
    public float cursorPixelSize = 32f;
    [Range(0.1f, 3f)]
    public float cursorScale = 1f;

    [Tooltip("¬арианты текстур курсора разных размеров. –азмер каждой текстуры определ€ет еЄ фактический размер на экране.")]
    public List<Texture2D> cursorTextures;

    [Tooltip("»ндекс выбранной текстуры из списка cursorTextures. ≈сли пусто, будет использовать cursorTexture.")]
    public int textureIndex = -1; // если -1, используем cursorTexture

    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        ApplyCursor();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // авто-подбор при изменени€х в инспекторе (не об€зательно)
        if (cursorTextures != null && cursorTextures.Count > 0)
        {
            // если индекс не указывает на существующий элемент, сбросим к -1
            if (textureIndex < -1 || textureIndex >= cursorTextures.Count)
                textureIndex = -1;
        }
    }
#endif

    public void ApplyCursor()
    {
        Texture2D texToUse = null;

        if (textureIndex >= 0 && cursorTextures != null && textureIndex < cursorTextures.Count)
        {
            texToUse = cursorTextures[textureIndex];
        }
        else if (cursorTexture != null)
        {
            texToUse = cursorTexture;
        }

        if (texToUse != null)
        {
            Cursor.SetCursor(texToUse, hotSpot, cursorMode);
        }
    }

    // ≈сли хотите динамически мен€ть размер во врем€ выполнени€:
    public void SetTextureByIndex(int index)
    {
        textureIndex = index;
        ApplyCursor();
    }

    // ѕример динамического изменени€ размера через переменную
    public void SetCursorSize(int pixelSize)
    {
        cursorPixelSize = pixelSize;
        // «десь можно дополнительно обработать поиск текстуры по размеру батча, если у вас есть набор текстур
        // дл€ простоты оставл€ем как изменение через выбор текстуры в cursorTextures.
    }
}
