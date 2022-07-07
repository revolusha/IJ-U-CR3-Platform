using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speedXMultiplier = 5f;
    [SerializeField] private float _speedYForce = 5f;

    private HealthController _health;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;
    private int _jumpHash;
    private int _speedHash;
    private bool _isDamaged;

    private void Start()
    {
        _isDamaged = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = Vector2.zero;
        _jumpHash = Animator.StringToHash("isFlying");
        _speedHash = Animator.StringToHash("Speed");
        _health = FindObjectOfType<HealthController>();
    }

    private void Update()
    {
        float directionX = Input.GetAxis("Horizontal");

        _rigidbody.velocity = new Vector2(directionX * _speedXMultiplier, _rigidbody.velocity.y);

        if (Input.GetButton("Jump") && CheckIfGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speedYForce);
            _animator.SetTrigger(_jumpHash);
        }
        else
        {
            if (directionX < 0)
            {
                directionX = -directionX;
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            _animator.SetFloat(_speedHash, directionX);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
    }

    public void GetDamaged()
    {
        if (_isDamaged == false)
        {
            _health.RemoveLife();
            _isDamaged = true;
            StartCoroutine(ResistInstantDamage());
        }
    }

    private bool CheckIfGrounded()
    {
        const float CheckLength = 0.05f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, CheckLength, _layerMask);
        return raycastHit.collider != null;
    }

    private IEnumerator ResistInstantDamage()
    {
        const float DamageDelay = 1f;

        yield return new WaitForSeconds(DamageDelay);

        _isDamaged = false;
    }
}
