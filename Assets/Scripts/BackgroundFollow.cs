using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;
    [SerializeField] private float textureUnitSize;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        textureUnitSize = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 newPos = transform.position;

        if (followX) newPos.x = cameraTransform.position.x;
        if (followY) newPos.y = cameraTransform.position.y;

        transform.position = newPos;

        transform.position = new Vector3(
        cameraTransform.position.x,
        cameraTransform.position.y,
        transform.position.z);

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSize)
        {
            float offset = (cameraTransform.position.x - transform.position.x) % textureUnitSize;
            transform.position = new Vector3(cameraTransform.position.x + offset, transform.position.y);
        }
    }
}