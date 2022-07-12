using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class Player : MonoBehaviour
{
    const int MaxHealth = 3;

    [SerializeField] private GoldLabel _label;
    [SerializeField] private HealthUI _healthUI;
    [SerializeField] private UnityEvent _getDamaged = new UnityEvent();
    [SerializeField] private UnityEvent _getHeart = new UnityEvent();
    [SerializeField] private UnityEvent _getKilled = new UnityEvent();
    [SerializeField] private Color _damagedColor;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speedXMultiplier = 5f;
    [SerializeField] private float _speedYForce = 5f;


    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;
    private int _jumpHash;
    private int _speedHash;
    private int _health;
    private int _gold;
    private bool _isDamaged;

    private void Start()
    {
        _gold = 0;
        _health = MaxHealth;
        _isDamaged = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = Vector2.zero;
        _jumpHash = Animator.StringToHash("isFlying");
        _speedHash = Animator.StringToHash("Speed");
        _label.UpdateValue();
    }

    private void Update()
    {
        float directionX = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump") && CheckIfGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speedYForce);
            _animator.SetTrigger(_jumpHash);
        }
        else
        {
            _rigidbody.velocity = new Vector2(directionX * _speedXMultiplier, _rigidbody.velocity.y);

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

    public void GetDamaged()
    {
        const float TimeBeforeDestroy = 2f;

        if (_isDamaged == false)
        {
            _health--;
            _healthUI.UpdateRender(_health);

            if (_health < 1)
            {
                PlayDead();
                Destroy(gameObject, TimeBeforeDestroy);
                return;
            }

            _getDamaged.Invoke();
            _isDamaged = true;
            TryConvertGoldToHeart();
            StartCoroutine(ResistInstantDamage());
        }
    }

    public void TakeCoin()
    {
        _gold++;
        _label.UpdateValue(_gold);
        TryConvertGoldToHeart();
    }

    private void TryConvertGoldToHeart()
    {
        const int HeartCost = 5;

        if (_gold >= HeartCost && _health < 3)
        {
            _gold -= HeartCost;
            _health++;
            _healthUI.UpdateRender(_health);
            _label.UpdateValue(_gold);
            _getHeart.Invoke();
        }
    }

    private bool CheckIfGrounded()
    {
        const float CheckLength = 0.05f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, CheckLength, _layerMask);
        return raycastHit.collider != null;
    }

    private void PlayDead()
    {
        _spriteRenderer.color = new Color(1f, .5f, .5f);
        _boxCollider.enabled = false;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speedYForce);
        _getKilled.Invoke();
    }

    private IEnumerator ResistInstantDamage()
    {
        const float Delay = 0.10f;
        const int BlinkSteps = 5;

        for (int i = 0; i < BlinkSteps; i++)
        {
            yield return new WaitForSeconds(Delay);
            _spriteRenderer.color = new Color(1f, .5f, .5f);
            yield return new WaitForSeconds(Delay);
            _spriteRenderer.color = Color.white;
        }

        _isDamaged = false;
    }
}
