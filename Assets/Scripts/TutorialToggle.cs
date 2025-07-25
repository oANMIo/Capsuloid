using UnityEngine;

public class TutorialToggle : MonoBehaviour
{
    public GameObject speechText;
    public GameObject tutorialPanel;

    private bool tutorialActive = false;
    private float tutorialOpenTime = 0f; // ����� ��������

    void Start()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1;
        tutorialActive = false;
    }

    void Update()
    {
        if (!tutorialActive && Input.GetKeyDown(KeyCode.Tab))
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0;
            tutorialActive = true;
            tutorialOpenTime = Time.unscaledTime; // ���������� Time.unscaledTime ��� timeScale = 0
        }

        if (tutorialActive)
        {
            // ��������� 0.2 ������� ����� ��������� (����� �� ������� TAB ��������)
            if (Time.unscaledTime - tutorialOpenTime > 0.2f && Input.anyKeyDown)
            {
                tutorialPanel.SetActive(false);
                Time.timeScale = 1;
                tutorialActive = false;
            }
        }
    }
}
