using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]

public class Coin : MonoBehaviour
{
    [SerializeField] private UnityEvent _reached;

    private SpriteRenderer _sprite;
    private CircleCollider2D _circleCollider;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<Player>(out Player player))
        {
            const float TimeToPlaySound = 3f;

            player.TakeCoin();
            _reached.Invoke();
            _sprite.color = new Color(0, 0, 0, 0);
            _circleCollider.enabled = false;
            Destroy(gameObject, TimeToPlaySound);
        }
    }
}
