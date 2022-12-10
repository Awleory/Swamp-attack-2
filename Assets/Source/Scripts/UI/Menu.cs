
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _nextWeaponButton;
    [SerializeField] private Button _previousWeaponButton;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _shopButton.onClick.AddListener(OnShopButtonPressed);
        _nextWeaponButton.onClick.AddListener(OnNextWeaponButtonPressed);
        _previousWeaponButton.onClick.AddListener(OnPreviousButtonPressed);
    }

    private void OnDisable()
    {
        _shopButton.onClick.RemoveListener(OnShopButtonPressed);
        _nextWeaponButton.onClick.RemoveListener(OnNextWeaponButtonPressed);
        _previousWeaponButton.onClick.RemoveListener(OnPreviousButtonPressed);
    }

    private void OnShopButtonPressed()
    {
        _shop.Show();
    }

    private void OnNextWeaponButtonPressed()
    {
        _player.SetNextWeapon();
    }

    private void OnPreviousButtonPressed()
    {
        _player.SetPreviousWeapon();
    }
}
