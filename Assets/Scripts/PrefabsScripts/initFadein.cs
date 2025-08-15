using UnityEngine;

public class InitFadeIn : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
        var fc = FindObjectOfType<FadeController>();
        if (fc != null)
        {
            fc.FadeIn();
        }
    }
}