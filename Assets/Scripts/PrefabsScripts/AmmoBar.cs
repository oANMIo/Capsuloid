using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    // ��� ������� Image, ������� ����� ���������
    [SerializeField] private string imageTag = "AmmoBarImage";

    // ������� ������� ��������: 0..5 (��� ��������� ��� ���� maxAmmo)
    [SerializeField] private Sprite[] ammoBarSprites;

    // ���������� ��� ���������� Image
    private Image _ammoBarImage;

    private void Awake()
    {
        // ����� Image �� ���� ���� ��� ��� ������
        if (!string.IsNullOrEmpty(imageTag))
        {
            var obj = GameObject.FindWithTag(imageTag);
            if (obj != null)
            {
                _ammoBarImage = obj.GetComponent<Image>();
                if (_ammoBarImage == null)
                {
                    Debug.LogWarning("AmmoBar: ������ ������ � �����, �� �� ��� ��� ���������� Image.");
                }
            }
            else
            {
                Debug.LogWarning("AmmoBar: �� ������ ������ � ����� " + imageTag);
            }
        }
        else
        {
            Debug.LogWarning("AmmoBar: imageTag ������. ���������� ��� ����������� � ����������.");
        }
    }

    // ���������� UI � ����������� �� �������� ���������� ��������
    public void UpdateAmmoUI(int currentAmmo)
    {
        if (_ammoBarImage == null)
        {
            // �������� ����� ����������� �������� (�� ������, ���� ��� ��� ���������� �����)
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