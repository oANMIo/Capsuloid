using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : MonoBehaviour
{
    public GameObject tutorialPanel;
    private bool tutorialActive = false;

    // �����
    [Tooltip("�����-������, ������� ������������� ��� ��������/��������")]
    public AudioClip clickSound;
    public float volume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        // �������� ��� ��������� AudioSource �� ��� �� ������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    // ��������� ����� ��������� ������:
    public void OpenTutorial()
    {
        if (tutorialPanel == null) return;
        tutorialPanel.SetActive(true);
        tutorialActive = true;
        PlayClickSound();
    }

    public void CloseTutorial()
    {
        if (tutorialPanel == null) return;
        tutorialPanel.SetActive(false);
        tutorialActive = false;
        PlayClickSound();
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
        }
    }

}
