using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _existTime = 2;

    private WaitForSeconds _liveTime;

    private void Awake()
    {
        _liveTime = new WaitForSeconds(_existTime);
    }

    private void Start()
    {
        StartCoroutine(LiveTimeCoroutine());
    }

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.left, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (enemy.IsAlive)
            {
                enemy.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator LiveTimeCoroutine()
    {
        yield return _liveTime;
        Destroy(gameObject);
    }
}
