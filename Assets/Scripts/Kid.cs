using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    private HubManager _hubManager;

    [Header("Movement")]
    private Rigidbody2D _rb;
    public float _speed;
    public float _distance;
    private Vector3 _startPos, _endPos, _currentTarget;

    private SpriteRenderer _sprite;
    private Collider2D _collider;
    [HideInInspector] public bool _isDead;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _startPos = transform.position - new Vector3(_distance, 0, 0);
        _endPos = transform.position + new Vector3(_distance, 0, 0);
        _currentTarget = _startPos;
    }

    private void Start()
    {
        _hubManager = FindObjectOfType<HubManager>();
    }

    private void Update()
    {
        if (!_isDead)
        {
            Vector2 _toTarget = (transform.position - _currentTarget).normalized;
            _toTarget = new Vector2(-_toTarget.x / Mathf.Abs(_toTarget.x + Mathf.Epsilon), 0);
            _rb.velocity = new Vector2(_toTarget.x * _speed, _rb.velocity.y);

            if (_toTarget.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (_toTarget.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }

            if (transform.position.x <= _startPos.x)
            {
                _currentTarget = _endPos;
            }
            else if (transform.position.x >= _endPos.x)
            {
                _currentTarget = _startPos;
            }

            RaycastHit2D _hit = Physics2D.Raycast(_collider.bounds.center + Vector3.up * 0.1f, transform.right, _collider.bounds.extents.x + 0.01f);
            if(_hit)
            {
                if (!_hit.collider.gameObject.CompareTag("Player"))
                {
                    if (_currentTarget == _endPos)
                    {
                        _currentTarget = _startPos;
                    }
                    else if (_currentTarget == _startPos)
                    {
                        _currentTarget = _endPos;
                    }
                }
            }
        }
    }

    public void OnDamage()
    {
        if (!_isDead)
        {
            _isDead = true;
            _sprite.color = Color.red;
            transform.rotation = Quaternion.Euler(0, 0, 90f);
            _hubManager.KidKilled();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position - new Vector3(_distance,0,0), transform.position + new Vector3(_distance, 0, 0));
        Gizmos.DrawWireSphere(transform.position - new Vector3(_distance, 0, 0), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(_distance, 0, 0), 0.2f);
    }
}
