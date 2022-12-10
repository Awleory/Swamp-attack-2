
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<ShopCell> _weaponCells = new List<ShopCell>();
    [SerializeField] private WeaponShopView _weaponViewTemplate;
    [SerializeField] private GameObject _content;
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Button _exitButton;

    private List<WeaponShopView> _weaponShopViews = new List<WeaponShopView>();

    private void Awake()
    {
        _weaponCells.ForEach(cell => CreateWeaponShop(cell));
    }

    private void OnEnable()
    {
        UpdateListeners(true);
    }

    private void OnDisable()
    {
        UpdateListeners(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateMoneyText();
        Time.timeScale = 0;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void CreateWeaponShop(ShopCell shopCell)
    {
        WeaponShopView weaponShopView = Instantiate(_weaponViewTemplate, _content.transform);
        weaponShopView.Initialize(shopCell.Weapon, shopCell.Price);
        _weaponShopViews.Add(weaponShopView);
    }

    private void UpdateListeners(bool add)
    {
        if (add)
        {
            _weaponShopViews.ForEach(view => view.BuyButtonPressed += OnBuyButtonPressed);
            _player.MoneyChanged += OnMoneyChanged;
            _exitButton.onClick.AddListener(OnExitButtonPressed);
        }
        else
        {
            _weaponShopViews.ForEach(view => view.BuyButtonPressed -= OnBuyButtonPressed);
            _player.MoneyChanged -= OnMoneyChanged;
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
        }
    }

    private void OnBuyButtonPressed(Weapon weapon, WeaponShopView weaponShopView)
    {
        BuyWeapon(weapon, weaponShopView);
    }

    private void OnMoneyChanged()
    {
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        _moneyText.text = _player.Money.ToString();
    }

    private void OnExitButtonPressed()
    {
        Hide();
    }

    private void BuyWeapon(Weapon weapon, WeaponShopView weaponShopView)
    {
        int price;
        ShopCell shopCell = _weaponCells.FirstOrDefault(cell => cell.Weapon == weapon);
        if (shopCell != null)
            price = shopCell.Price;
        else
            price = 0;

        bool weaponSold = _player.TryBuyWeapon(weapon, price);
        weaponShopView.UpdateView(weaponSold);
    }
}

[System.Serializable]
public class ShopCell
{
    public Weapon Weapon;
    public int Price;
}
