using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    private HubManager _hubManager;
    public GameObject _kidHead;

    [Header("Movement")]
    private Rigidbody2D _rb;
    public float _speed;
    public float _distance;
    private Vector3 _startPos, _endPos, _currentTarget;

    [Header("Gender")]
    private bool _isGirl;
    public Sprite _girlHeadSprite;

    private Collider2D _collider;
    private Animator _animator;
    [HideInInspector] public bool _isDead;

    private void Awake()
    {
        if(_kidHead == null)
        {
            throw new System.Exception(name + " has no associated head !!");
        }

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _startPos = transform.position - new Vector3(_distance, 0, 0);
        _endPos = transform.position + new Vector3(_distance, 0, 0);
        _currentTarget = _startPos;

        int a = Random.Range(0, 2);
        if(a == 0)
        {
            _isGirl = false;
        }
        else
        {
            _isGirl = true;
        }
        _animator.SetBool("isGirl", _isGirl);
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
                transform.rotation = Quaternion.Euler(0, 180, 0);
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
            GameObject _head = Instantiate(_kidHead, _collider.bounds.center + Vector3.up * _collider.bounds.extents.y, Quaternion.identity);
            
            if(_isGirl)
                _head.GetComponent<SpriteRenderer>().sprite = _girlHeadSprite;
            
            Rigidbody2D _headRb = _head.GetComponent<Rigidbody2D>();
            _headRb.velocity = _rb.velocity + Vector2.up * 3f;
            _headRb.angularVelocity = 180f;
            Destroy(_head, 3f);

            //transform.rotation = Quaternion.Euler(0, 0, 90f);
            _hubManager.KidKilled();
            _animator.SetTrigger("Die");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position - new Vector3(_distance,0,0), transform.position + new Vector3(_distance, 0, 0));
        Gizmos.DrawWireSphere(transform.position - new Vector3(_distance, 0, 0), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(_distance, 0, 0), 0.2f);
    }
}
