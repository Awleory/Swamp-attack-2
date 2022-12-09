using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string _label;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _purchased = false;
    [SerializeField] private Bullet _bullet;

    public virtual void Shoot(Transform startPoint)
    {
        Instantiate(_bullet, startPoint.position, Quaternion.identity);
    }
}
