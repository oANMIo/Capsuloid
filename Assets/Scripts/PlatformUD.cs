using UnityEngine;

public class PlatformUD : MonoBehaviour
{
    public float speed = 3f; 
    public float moveDistance = 5f; 
    private Vector3 startPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;
        if (movingUp)
        {
            transform.Translate(Vector3.up * movement);
            if (transform.position.y >= startPos.y + moveDistance)
                movingUp = false;
        }
        else
        {
            transform.Translate(Vector3.down * movement);
            if (transform.position.y <= startPos.y - moveDistance)
                movingUp = true;
        }
    }
}
