using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteMenu;
    public AudioSource audioSource;
    [SerializeField] private AudioClip WinSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
            Time.timeScale = 0f; 
            levelCompleteMenu.SetActive(true);
            audioSource?.PlayOneShot(WinSound);
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menus");
        audioSource?.PlayOneShot(WinSound);
    }
}
