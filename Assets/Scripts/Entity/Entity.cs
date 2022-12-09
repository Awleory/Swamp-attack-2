using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
[RequireComponent (typeof(MovementPhysics))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float _maxHealthPoints;

    public AnimationController AnimationController { get; private set; }
    public MovementPhysics MovementPhysics { get; private set; }
    public float HealthPoints => _healthPoints;
    public bool IsAlive => _healthPoints > 0;

    private float _healthPoints;
    private float _deathTime = 10;

    protected virtual void Awake()
    {
        AnimationController = GetComponent<AnimationController>();
        MovementPhysics = GetComponent<MovementPhysics>();
    }

    protected virtual void Start()
    {
        _healthPoints = _maxHealthPoints;
    }

    protected virtual void OnEnable()
    {
        MovementPhysics.Moved += OnMoved;
    }

    protected virtual void OnDisable()
    {
        MovementPhysics.Moved -= OnMoved;
    }

    protected virtual void Update()
    {
        MovementPhysics.UpdateMove();
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        _healthPoints = Math.Max(0, _healthPoints - damage);
        AnimationController.PlayGettingDamage();

        if (_healthPoints <= 0)
            Kill();
    }

    public void Kill()
    {
        OnDied();
        AnimationController.PlayDeath();
        StartCoroutine(DeathCoroutine());
    }

    protected virtual void OnDied() { }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(_deathTime);
        Destroy(gameObject);
    }

    private void OnMoved(float velocityX)
    {
        AnimationController.OnMove(Mathf.Abs(velocityX));
    }
}
