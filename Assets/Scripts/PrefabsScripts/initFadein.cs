using UnityEngine;

public class InitFadeIn : MonoBehaviour
{
    void Start()
    {
        var fc = FindObjectOfType<FadeController>();
        if (fc != null)
        {
            fc.FadeIn();
        }
    }
}