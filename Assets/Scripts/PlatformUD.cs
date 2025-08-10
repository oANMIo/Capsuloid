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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
