using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class MovementPhysics : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _gravityRate = 1f;
    [SerializeField] private LayerMask _collisionLayerMask;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private int _extraJumps = 0;
    [SerializeField] private float _minGroundNormalY = .65f;

    public event Action<float> Moved;

    public bool Jumped { get; private set; }
    public bool Grounded { get; private set; }
    public float VelocityX => _velocity.x;
    public float VelocityY => _velocity.y;

    private ContactFilter2D _contactFilter;
    private Rigidbody2D _rigidBody2D;

    private int _jumpsDone;
    private Vector2 _directionVelocity;
    private Vector2 _groundNormal;
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    private Vector2 _velocity;

    private const float _minMoveDistance = .001f;
    private const float _shellRadius = .01f;

    private void OnEnable()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_collisionLayerMask);
        _contactFilter.useLayerMask = true;
    }

    public void SetDirectionMove(Vector2 direction)
    {
        _directionVelocity = direction;
    }

    public bool TryJump()
    {
        if (_jumpsDone <= _extraJumps)
        {
            Jumped = true;
            _jumpsDone++;
            _velocity.y = _jumpForce;

            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateMove()
    {
        if (_directionVelocity.y != 0)
        {
            _velocity.y = _directionVelocity.y * _speed;
        }
        _velocity += _gravityRate * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _directionVelocity.x * _speed;

        Grounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 moveVector = moveAlongGround * deltaPosition.x;

        Move(moveVector, false);

        moveVector = Vector2.up * deltaPosition.y;

        Move(moveVector, true);

        Moved?.Invoke(VelocityX);
    }

    private void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > _minMoveDistance)
        {
            RaycastHit2D[] _tempHitBuffer = new RaycastHit2D[16];

            int count = _rigidBody2D.Cast(move, _contactFilter, _tempHitBuffer, distance + _shellRadius);

            _hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_tempHitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    Grounded = true;
                    Jumped = false;
                    _jumpsDone = 0;

                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidBody2D.position = _rigidBody2D.position + move.normalized * distance;
    }
}
