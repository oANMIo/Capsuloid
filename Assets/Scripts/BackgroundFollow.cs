using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [Header("Base Follow Settings")]
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;

    [Header("Parallax Settings")]
    [SerializeField] private bool useParallax = true;
    [SerializeField][Range(0.1f, 0.9f)] private float parallaxFactor = 0.5f; // 0.5 = ������� ����

    [Header("Infinite Repeat")]
    [SerializeField] private bool infiniteHorizontal = true;
    [SerializeField] private bool infiniteVertical = false;
    public SpriteRenderer backgroundSpriteRenderer;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    private Vector3 startPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        startPosition = transform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = sprite.texture.height / sprite.pixelsPerUnit;
        FitSpriteToScreen();
    }

    void LateUpdate()
    {
        Vector3 newPosition = transform.position;

        // ������� ���������� �� �������
        if (followX) newPosition.x = cameraTransform.position.x;
        if (followY) newPosition.y = cameraTransform.position.y;

        // ��������� ���������-������
        if (useParallax)
        {
            Vector3 delta = cameraTransform.position - lastCameraPosition;
            newPosition.x += delta.x * (1 - parallaxFactor);
            newPosition.y += delta.y * (1 - parallaxFactor);
        }

        transform.position = newPosition;
        lastCameraPosition = cameraTransform.position;

        // ����������� ���������� ����
        if (infiniteHorizontal && followX)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(
                    cameraTransform.position.x + offsetX,
                    transform.position.y,
                    transform.position.z);
            }
        }

        if (infiniteVertical && followY)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(
                    transform.position.x,
                    cameraTransform.position.y + offsetY,
                    transform.position.z);
            }
        }
    }

    void FitSpriteToScreen()
    {
        if (backgroundSpriteRenderer == null)
            return;

        Sprite sprite = backgroundSpriteRenderer.sprite;
        if (sprite == null)
            return;

        // ������� ������� � ����
        Vector2 spriteSize = sprite.bounds.size;

        // ������� ������ � ����� (������� �� orthographic size � ������)
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // ������ ���������
        Vector3 scale = backgroundSpriteRenderer.transform.localScale;
        scale.x = screenWidth / spriteSize.x;
        scale.y = screenHeight / spriteSize.y;

        backgroundSpriteRenderer.transform.localScale = scale;
    }
}