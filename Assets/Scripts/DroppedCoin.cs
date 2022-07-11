using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class DroppedCoin : MonoBehaviour
{
    [SerializeField] private UnityEvent _reached;

    private CapsuleCollider2D _capsuleCollider;
    private SpriteRenderer _sprite;
    private CircleCollider2D _circleCollider;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _capsuleCollider.enabled = false;
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.enabled = false;
        _sprite = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<Player>(out Player player))
        {
            const float TimeToPlaySound = 2f;

            player.TakeCoin();
            _reached.Invoke();
            _sprite.color = new Color(0, 0, 0, 0);
            _circleCollider.enabled = false;
            _capsuleCollider.enabled = false;
            Destroy(gameObject, TimeToPlaySound);
        }
    }

    public void Drop(Vector2 vector)
    {
        _rigidbody.velocity = vector;
        StartCoroutine(Ghoster());
    }

    private IEnumerator Ghoster()
    {
        const float TimeToIgnoreColliders = 0.2f;

        yield return new WaitForSeconds(TimeToIgnoreColliders);
        _circleCollider.enabled = true;
        _capsuleCollider.enabled = true;
    }
}

