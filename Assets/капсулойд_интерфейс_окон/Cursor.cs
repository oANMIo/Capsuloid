using UnityEngine;
using System.Collections.Generic;

public class CustomCursor : MonoBehaviour
{
    [Tooltip("������� �������� �������. �������� �������� �������� � ������ ����.")]
    public Texture2D cursorTexture;

    [Header("Cursor Size (pixels)")]
    public float cursorPixelSize = 32f;
    [Range(0.1f, 3f)]
    public float cursorScale = 1f;

    [Tooltip("�������� ������� ������� ������ ��������. ������ ������ �������� ���������� � ����������� ������ �� ������.")]
    public List<Texture2D> cursorTextures;

    [Tooltip("������ ��������� �������� �� ������ cursorTextures. ���� �����, ����� ������������ cursorTexture.")]
    public int textureIndex = -1; // ���� -1, ���������� cursorTexture

    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        ApplyCursor();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        // ����-������ ��� ���������� � ���������� (�� �����������)
        if (cursorTextures != null && cursorTextures.Count > 0)
        {
            // ���� ������ �� ��������� �� ������������ �������, ������� � -1
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

    // ���� ������ ����������� ������ ������ �� ����� ����������:
    public void SetTextureByIndex(int index)
    {
        textureIndex = index;
        ApplyCursor();
    }

    // ������ ������������� ��������� ������� ����� ����������
    public void SetCursorSize(int pixelSize)
    {
        cursorPixelSize = pixelSize;
        // ����� ����� ������������� ���������� ����� �������� �� ������� �����, ���� � ��� ���� ����� �������
        // ��� �������� ��������� ��� ��������� ����� ����� �������� � cursorTextures.
    }
}
