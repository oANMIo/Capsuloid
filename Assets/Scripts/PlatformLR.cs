using UnityEngine;

public class PlatformLR : MonoBehaviour
{
    public float speed = 3f; 
    public float moveDistance = 5f; 

    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;
        if (movingRight)
        {
            transform.Translate(Vector3.right * movement);
            if (transform.position.x >= startPos.x + moveDistance)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector3.left * movement);
            if (transform.position.x <= startPos.x - moveDistance)
                movingRight = true;
        }
    }
}