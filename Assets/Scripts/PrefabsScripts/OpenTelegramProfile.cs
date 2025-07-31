using UnityEngine;

public class OpenTelegramProfile : MonoBehaviour
{
    // Вставьте сюда свою ссылку на профиль в Телеграме
    public string telegramProfileLink = "https://t.me/oanmio";

    // Эта функция должна быть добавлена в кнопку через инспектор, чтобы вызвать при нажатии
    public void OpenProfile()
    {
        Application.OpenURL(telegramProfileLink);
    }
}