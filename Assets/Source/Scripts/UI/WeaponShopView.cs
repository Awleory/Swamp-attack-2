using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopView : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _buyText;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _labelText;

    public event Action<Weapon, WeaponShopView> BuyButtonPressed;

    private Weapon _weapon;
    private bool _purchased = false;

    private const string _buyString = "Buy";
    private const string _soldString = "Sold";

    public void Initialize(Weapon weapon, float price)
    {
        _weapon = weapon;

        _priceText.text = price.ToString();
        _image.sprite = weapon.Icon;
        _labelText.text = weapon.Label;
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
    }

    private void OnBuyButtonClick()
    {
        if (_purchased == false)
            BuyButtonPressed?.Invoke(_weapon, this);
    }

    public void UpdateView(bool weaponIsSold)
    {
        _purchased = weaponIsSold;

        if (_purchased)
        {
            _buyText.text = _soldString;
            _buyButton.interactable = false;
        }
        else
        {
            _buyText.text = _buyString;
            _buyButton.interactable = true;
        }
    }
}
