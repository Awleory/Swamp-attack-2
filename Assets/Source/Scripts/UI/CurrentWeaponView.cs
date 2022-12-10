
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponView : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _label;

    private void OnEnable()
    {
        _player.WeaponChanged += OnWeaponChanged;
    }

    private void OnDisable()
    {
        _player.WeaponChanged -= OnWeaponChanged;
    }

    private void OnWeaponChanged(Weapon weapon)
    {
        Render(weapon);
    }

    private void Render(Weapon weapon)
    {
        _image.sprite = weapon.Icon;
        _label.text = weapon.Label;
    }
}
