using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] private bool _flipX;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private const string _moveParameter = "Speed";
    private const string _attackParameter = "Attack";
    private const string _deathParameter = "Death";
    private const string _gettingDamage = "GettingDamage";

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFlipX(bool state)
    {
        _spriteRenderer.flipX = _flipX ? !state : state;
    }

    public void OnMove(float speed)
    {
        _animator.SetFloat(_moveParameter, speed);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger(_attackParameter);
    }

    public void PlayDeath()
    {
        _animator.SetTrigger(_deathParameter);
    }

    public void PlayGettingDamage()
    {
        _animator.SetTrigger(_gettingDamage);
    }
}
