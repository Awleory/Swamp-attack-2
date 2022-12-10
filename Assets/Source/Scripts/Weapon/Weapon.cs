using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string _label;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Bullet _bullet;

    public Sprite Icon => _icon;
    public string Label => _label;

    public virtual void Shoot(Transform startPoint)
    {
        Instantiate(_bullet, startPoint.position, Quaternion.identity);
    }
}
