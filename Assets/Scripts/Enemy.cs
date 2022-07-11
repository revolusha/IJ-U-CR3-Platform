using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private Player _player;
    [SerializeField] private UnityEvent _getKilled = new UnityEvent();

    private CoinSpawner _spawner;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb2d;
    private CapsuleCollider2D _capsuleCollider;
    private BoxCollider2D _boxCollider;
    private Coroutine _patrolCoroutine;
    private Coroutine _checkDeadCoroutine;
    public Vector3 _startPoint;
    public Vector3 _secondPoint;
    public Vector3 _targetPoint;
    public bool _isMovingToStart;
    public bool _isDead;

    private void Awake()
    {
        _spawner = FindObjectOfType<CoinSpawner>();
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _isDead = false;
        _isMovingToStart = true;
        _startPoint = transform.position;
        _secondPoint = new Vector3(_startPoint.x - _distance, _startPoint.y, _startPoint.z);
        _targetPoint = _startPoint;
        _patrolCoroutine = StartCoroutine(Patrol());
        _checkDeadCoroutine = StartCoroutine(Moving());
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

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x - _distance, transform.position.y, transform.position.z), Color.green);
    }

    private void PlayDead()
    {
        const float TimeBeforeDestroy = 2f;

        _boxCollider.enabled = false;

        if (_checkDeadCoroutine != null)
            StopCoroutine(_checkDeadCoroutine);

        if (_patrolCoroutine != null)
            StopCoroutine(_patrolCoroutine);

        _sprite.color = new Color(1f, .5f, .5f);
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
        _getKilled.Invoke();
        _spawner.TriggerScript(transform);
        Destroy(gameObject, TimeBeforeDestroy);
    }

    private void CoroutineRestart(Coroutine coroutine, IEnumerator enumerator)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        StartCoroutine(enumerator);
    }

    private IEnumerator Patrol()
    {
        const float MinRandomStartPatrolTime = 1f;
        const float MaxRandomStartPatrolTime = 4f;
        const float WaitTime = 2f;
        const bool IsAlways = true;

        yield return new WaitForSeconds(Random.Range(MinRandomStartPatrolTime, MaxRandomStartPatrolTime));

        while (IsAlways)
        {
            if (_isMovingToStart)
            {
                _sprite.flipX = false;

                if (transform.position == _startPoint)
                {
                    _isMovingToStart = false;
                    _targetPoint = _secondPoint;
                }
            }
            else
            {
                _sprite.flipX = true;

                if (transform.position == _secondPoint)
                {
                    _isMovingToStart = true;
                    _targetPoint = _startPoint;
                }
            }

            yield return new WaitForSeconds(WaitTime);
        }
    }

    private IEnumerator Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _speed * Time.deltaTime);

        yield return new WaitForEndOfFrame();
        CoroutineRestart(_checkDeadCoroutine, Moving());
    }
}
