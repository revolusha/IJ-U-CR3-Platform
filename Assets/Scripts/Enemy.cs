using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Patrol))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private Color _damagedColor;
    [SerializeField] private Player _player;
    [SerializeField] private UnityEvent _getKilled = new UnityEvent();

    private Patrol _patrol;
    private CoinSpawner _spawner;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb2d;
    private CapsuleCollider2D _capsuleCollider;
    private BoxCollider2D _boxCollider;
    private bool _isDead;

    private void Awake()
    {
        _spawner = FindObjectOfType<CoinSpawner>();
    }

    private void Start()
    {
        _patrol = GetComponent<Patrol>();
        _rb2d = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_isDead)
            return;

        if (collider.TryGetComponent<Player>(out _))
        {
            if (_boxCollider.IsTouching(_player.GetComponent<Collider2D>()))
            {
                _player.GetDamaged();
            }
            else if (_capsuleCollider.IsTouching(_player.GetComponent<Collider2D>()))
            {
                PlayDead();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (_isDead)
            return;

        if (collider.TryGetComponent<Player>(out _))
        {
            if (_boxCollider.IsTouching(_player.GetComponent<Collider2D>()))
            {
                _player.GetDamaged();
            }
        }
    }

    private void PlayDead()
    {
        const float TimeBeforeDestroy = 2f;

        _boxCollider.enabled = false;
        _patrol.StopPatrolJobs();
        _sprite.color = _damagedColor;
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
        _getKilled.Invoke();
        _spawner.TriggerScript(transform);
        Destroy(gameObject, TimeBeforeDestroy);
    }

}
