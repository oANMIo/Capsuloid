using UnityEngine;

public class OpenTelegramProfile : MonoBehaviour
{
    // �������� ���� ���� ������ �� ������� � ���������
    public string telegramProfileLink = "https://t.me/oanmio";

    // ��� ������� ������ ���� ��������� � ������ ����� ���������, ����� ������� ��� �������
    public void OpenProfile()
    {
        Application.OpenURL(telegramProfileLink);
    }
}