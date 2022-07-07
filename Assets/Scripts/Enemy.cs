using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent (typeof(BoxCollider2D))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private Player _player;

    private CapsuleCollider2D _capsuleCollider;
    private BoxCollider2D _boxCollider;
    private Coroutine _coroutine;
    public Vector3 _startPoint;
    public Vector3 _secondPoint;
    public Vector3 _targetPoint;
    public bool _isMovingToStart;
    public bool _isDead;

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _isDead = false;
        _isMovingToStart = true;
        _startPoint = transform.position;
        _secondPoint = new Vector3(_startPoint.x - _distance, _startPoint.y, _startPoint.z);
        _targetPoint = _startPoint;
        _coroutine = StartCoroutine(Patrol());
        Debug.Log("start");
    }

    private void Update()
    {
        if (_isDead)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                Debug.Log("stop");
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _speed * Time.deltaTime);

        Debug.DrawLine(_startPoint, _secondPoint, Color.green);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out _))
        {
            if (_boxCollider.IsTouching(_player.GetComponent<Collider2D>()))
            {
                _player.GetDamaged();
            }
            else if (_capsuleCollider.IsTouching(_player.GetComponent<Collider2D>()))
            {
                Debug.Log("2");
            }
        }
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
                if (transform.position == _startPoint)
                {
                    _isMovingToStart = false;
                    _targetPoint = _secondPoint;
                }
            }
            else
            {
                if (transform.position == _secondPoint)
                {
                    _isMovingToStart = true;
                    _targetPoint = _startPoint;
                }
            }

            yield return new WaitForSeconds(WaitTime);
        }
    }
}
