using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    // Тег объекта Image, который нужно обновлять
    [SerializeField] private string imageTag = "AmmoBarImage";

    // Спрайты полоски патронов: 0..5 (или адаптируй под свой maxAmmo)
    [SerializeField] private Sprite[] ammoBarSprites;

    // Внутренний кэш найденного Image
    private Image _ammoBarImage;

    private void Awake()
    {
        // Поиск Image по тегу один раз при старте
        if (!string.IsNullOrEmpty(imageTag))
        {
            var obj = GameObject.FindWithTag(imageTag);
            if (obj != null)
            {
                _ammoBarImage = obj.GetComponent<Image>();
                if (_ammoBarImage == null)
                {
                    Debug.LogWarning("AmmoBar: найден объект с тегом, но на нем нет компонента Image.");
                }
            }
            else
            {
                Debug.LogWarning("AmmoBar: не найден объект с тегом " + imageTag);
            }
        }
        else
        {
            Debug.LogWarning("AmmoBar: imageTag пустой. Установите тег изображения в инспекторе.");
        }
    }

    // Обновление UI в зависимости от текущего количества патронов
    public void UpdateAmmoUI(int currentAmmo)
    {
        if (_ammoBarImage == null)
        {
            // Попробуй найти изображение повторно (на случай, если тег был установлен позже)
            if (!string.IsNullOrEmpty(imageTag))
            {
                var obj = GameObject.FindWithTag(imageTag);
                if (obj != null)
                {
                    _ammoBarImage = obj.GetComponent<Image>();
                }
            }

            if (_ammoBarImage == null)
                return;
        }

        if (ammoBarSprites == null || ammoBarSprites.Length == 0)
            return;

        int idx = Mathf.Clamp(currentAmmo, 0, ammoBarSprites.Length - 1);
        _ammoBarImage.sprite = ammoBarSprites[idx];
    }
}