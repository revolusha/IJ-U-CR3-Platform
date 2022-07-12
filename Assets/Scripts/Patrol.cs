using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Patrol : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;

    public Vector3 _startPoint;
    public Vector3 _secondPoint;
    public Vector3 _targetPoint;

    private SpriteRenderer _sprite;
    private Coroutine _patrolCoroutine;
    private Coroutine _checkDeadCoroutine;
    private bool _isMovingToStart;

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x - _distance, transform.position.y, transform.position.z), Color.green);
    }

    private void OnEnable()
    {
        _isMovingToStart = true;
        _startPoint = transform.position;
        _secondPoint = new Vector3(_startPoint.x - _distance, _startPoint.y, _startPoint.z);
        _targetPoint = _startPoint;
    }

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _patrolCoroutine = StartCoroutine(PatrolJob());
        _checkDeadCoroutine = StartCoroutine(Moving());
    }

    public void StopPatrolJobs()
    {
        if (_checkDeadCoroutine != null)
            StopCoroutine(_checkDeadCoroutine);

        if (_patrolCoroutine != null)
            StopCoroutine(_patrolCoroutine);
    }

    private void CoroutineRestart(Coroutine coroutine, IEnumerator enumerator)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        StartCoroutine(enumerator);
    }

    private IEnumerator PatrolJob()
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
