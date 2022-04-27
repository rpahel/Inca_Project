using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    public GameStuff _gameStuff;

    [Header("Movement")]
    private Rigidbody2D _rb;
    public float _speed;
    public float _distance;
    private Vector3 _startPos, _endPos, _currentTarget;

    private SpriteRenderer _sprite;
    [HideInInspector] public bool _isDead;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _startPos = transform.position - new Vector3(_distance, 0, 0);
        _endPos = transform.position + new Vector3(_distance, 0, 0);
        _currentTarget = _startPos;
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
        }
    }

    public void OnDamage()
    {
        if (!_isDead)
        {
            _isDead = true;
            _sprite.color = Color.red;
            transform.rotation = Quaternion.Euler(0, 0, 90f);
            _gameStuff._kidsKilled++;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position - new Vector3(_distance,0,0), transform.position + new Vector3(_distance, 0, 0));
        Gizmos.DrawWireSphere(transform.position - new Vector3(_distance, 0, 0), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(_distance, 0, 0), 0.2f);
    }
}
