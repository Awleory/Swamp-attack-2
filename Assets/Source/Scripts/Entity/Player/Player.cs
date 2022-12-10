using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private FillBar _healthBar;
    [SerializeField] private int _startMoney;

    public event Action MoneyChanged;
    public event Action<Weapon> WeaponChanged;

    private List<Weapon> _weapons;

    public int Money { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        _weapons = new List<Weapon>();
        if (_currentWeapon != null)
            _weapons.Add(_currentWeapon);

        Money = _startMoney;
    }

    protected override void Start()
    {
        base.Start();

        MoneyChanged?.Invoke();
        WeaponChanged?.Invoke(_currentWeapon);
    }

    protected override void Update()
    {
        base.Update();
        ProcessInput();
    }

    public void OnEnemyDied(int _reward)
    {
        AddMoney(_reward);
    }

    public bool TryBuyWeapon(Weapon weapon, int price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price));

        if (TrySpendMoney(price))
        {
            _weapons.Add(weapon);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetNextWeapon()
    {
        ChangeWeapon(true);
    }

    public void SetPreviousWeapon()
    {
        ChangeWeapon(false);
    }

    private bool TrySpendMoney(int count)
    {
        if (Money >= count)
        {
            Money -= count;
            MoneyChanged?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddMoney(int count)
    {
        Money += count;
        MoneyChanged?.Invoke();
    }

    private void ChangeWeapon(bool isNext)
    {
        if (_weapons.Count == 0)
            return;
        
        if (_currentWeapon == null)
        {
            SetWeapon(_weapons.First());
            return;
        }
    
        int offset = isNext ? 1 : -1;

        int currentIndex = _weapons.IndexOf(_currentWeapon);
        int weaponIndex = (int)Mathf.Repeat(currentIndex + offset, _weapons.Count);
        SetWeapon(_weapons[weaponIndex]);
    }

    private void SetWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        WeaponChanged?.Invoke(_currentWeapon);
    }

    protected override void OnHealthChanged()
    {
        base.OnHealthChanged();
        _healthBar.OnValueChanged(HealthPoints, MaxHealthPoints);
    }

    private void ProcessInput()
    {
        if (Time.timeScale == 0)
            return;

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), 0);
        MovementPhysics.SetDirectionMove(move);

        if (move.x != 0)
        {
            AnimationController.SetFlipX(move.x < 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.Shoot(_shootPoint);
                AnimationController.PlayAttack();
            }
        }
    }
}
