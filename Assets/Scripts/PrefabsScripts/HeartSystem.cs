using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartSystem : MonoBehaviour
{
    public static int health;
    public GameObject Heart1, Heart2, Heart3;
    private Canvas heartsCanvas;

    private void Awake()
    {
        heartsCanvas = GetComponent<Canvas>();
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void Start()
    {
        // ��������� ������ ������� ����� ��� ������
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (IsGameScene(currentSceneIndex))
        {
            heartsCanvas.enabled = true;
            InitializeHearts();
        }
        else
        {
            heartsCanvas.enabled = false;
        }
    }

    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        // �������� Canvas ������ �� ������ � ��������� 1, 2, 3
        if (IsGameScene(newScene.buildIndex))
        {
            heartsCanvas.enabled = true;
            InitializeHearts();
        }
        else
        {
            heartsCanvas.enabled = false;
        }
    }

    // ���������, �������� �� ����� ������� (������� 1, 2, 3)
    private bool IsGameScene(int sceneIndex)
    {
        return sceneIndex >= 1 && sceneIndex <= 3;
    }

    private void InitializeHearts()
    {
        health = 3;
        Heart1.SetActive(true);
        Heart2.SetActive(true);
        Heart3.SetActive(true);
    }

    private void Update()
    {
        switch (health)
        {
            case 3:
                Heart1.SetActive(true);
                Heart2.SetActive(true);
                Heart3.SetActive(true);
                break;
            case 2:
                Heart1.SetActive(true);
                Heart2.SetActive(true);
                Heart3.SetActive(false);
                break;
            case 1:
                Heart1.SetActive(true);
                Heart2.SetActive(false);
                Heart3.SetActive(false);
                break;
            case 0:
                Heart1.SetActive(false);
                Heart2.SetActive(false);
                Heart3.SetActive(false);
                break;
        }
    }
}