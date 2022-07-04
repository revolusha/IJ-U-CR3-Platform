using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speedXMultiplier = 5f;
    [SerializeField] private float _speedYForce = 5f;
    [SerializeField] private Vector2 _velocity;

    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;
    private bool _isGrounded;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = Vector2.zero;
        _isGrounded = false;
    }

    private void Update()
    {
        float directionX = Input.GetAxis("Horizontal");

        _rigidbody.velocity = new Vector2(directionX * _speedXMultiplier, _rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump") && CheckIfGrounded())
        {
            _isGrounded = false;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speedYForce);
        }

        _velocity = _rigidbody.velocity;
    }

    private bool CheckIfGrounded()
    {
        const float CheckLength = 0.05f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, CheckLength, _layerMask);
        return raycastHit.collider != null;
    }
}
