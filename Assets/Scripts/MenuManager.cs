using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.EventSystems; // ��������� ��� ������ � ��������� UI

public class MenuManager : MonoBehaviour, IPointerClickHandler // ��������� ��������� ��� ��������� ������
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject Autors; // ������ �� ������ � ����������� �� �������

    private bool isVideoPlaying = false;
    private bool isAuthorsVisible = false; // ���� ��������� ���� �������

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.gameObject.SetActive(false);
        }

        // �������� ���� ������� ��� ������
        if (Autors != null)
        {
            Autors.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ���� ���� ������� �������, �������� ��� ��� �����
        if (isAuthorsVisible)
        {
            Autors.SetActive(false);
            isAuthorsVisible = false;
        }
    }

    public void PlayGame()
    {
        if (videoPlayer != null && !isVideoPlaying)
        {
            menuUI.SetActive(false);
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
            isVideoPlaying = true;
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Info()
    {
        if (Autors != null)
        {
            Autors.SetActive(true);
            isAuthorsVisible = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }

    // ��������� ����� ��� ��������� ������ ��� UI ���������
    private void Update()
    {
        if (isAuthorsVisible && Input.GetMouseButtonDown(0))
        {
            // ���������, ��� ���� �� �� UI ��������
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Autors.SetActive(false);
                isAuthorsVisible = false;
            }
        }
    }
}