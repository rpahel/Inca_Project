using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Player : MonoBehaviour
{
    public float _health;

    [Header("Managers")]
    public GameStuff _gameStuff;
    public PowerManager _powerManager;

    [Header("Power")]
    public PowerType _power;
    public PowerLeft _powerLeft;
    public PowerRight _powerRight;
    public PowerUp _powerUp;

    [Header("Movement")]
    public float _speed;
    public float _jumpForce;
    public float _gScale;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Animator _anim;
    private bool _knockBacked;
    private bool _isJumping;

    [Header("Attack")]
    public float _baseDamage;
    public float _attackCd;
    [Tooltip("Force du knockBack infligé aux ennemis.")]
    public float _knockBack;
    public Transform _sweepStart, _sweepEnd;
    private bool _canAttack;

    [Header("Holding Kids")]
    private GameObject _holdedObject;
    private bool _itsHold;
    private Coroutine _lastCoroutine;
    private bool _forDrop;
    private GameObject _grave;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gScale;
        _collider = GetComponent<Collider2D>();
        _canAttack = true;
        _anim = GetComponent<Animator>();
        _holdedObject = null;
        //Attribuer les modifiers aux pouvoirs et faire les pouvoirs aussi ahah
    }

    void Update()
    {
        Movement();
        Jump();
        AttackCheck();
    }

    private void Movement()
    {
        if (!_knockBacked)
        {
            _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _speed, _rb.velocity.y);
            _anim.SetFloat("xSpeed", Mathf.Abs(_rb.velocity.x));
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Jump()
    {
        RaycastHit2D _hit = Physics2D.Raycast(_collider.bounds.center, Vector2.down, _collider.bounds.extents.y + .01f);
        if (_hit)
        {
            _isJumping = false;
        }
        else
        {
            _isJumping = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (!_isJumping)
            {
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Je touche pas le sol.");
            }
        }

        _anim.SetBool("isJumping", _isJumping);
        _anim.SetFloat("ySpeed", _rb.velocity.y);
    }

    void AttackCheck()
    {
        if (Input.GetButtonDown("Attack"))
        {
            if (_holdedObject)
            {
                DropKid();
            }
        }

        if (_isJumping)
        return;

        if (Input.GetButtonDown("Attack"))
        {
            _lastCoroutine = StartCoroutine(HoldingAttack());
            _itsHold = false;

            if (_holdedObject)
            {
                DropKid();
                _forDrop = true;
            }
        }

        if (Input.GetButtonUp("Attack"))
        {
            StopCoroutine(_lastCoroutine);

            if (!_holdedObject)
            {
                if (!_forDrop)
                {
                    if (!_itsHold && _canAttack)
                    {
                        Attack();
                    }
                    else if (!_itsHold && !_canAttack)
                    {
                        Debug.Log("You are in Cooldown.");
                    }

                    _forDrop=false;
                }
            }
        }
    }

    private void Attack()
    {
        Debug.DrawLine(_sweepStart.position, _sweepEnd.position, Color.red, 0.5f);
        _canAttack = false;
        _anim.SetTrigger("Attack");
        StartCoroutine(AttackCD());

        RaycastHit2D[] _hit = Physics2D.LinecastAll(_sweepStart.position, _sweepEnd.position);
        for (int i = 0; i < _hit.Length; i++)
        {
            if (_hit[i].collider.gameObject.CompareTag("Enemy"))
            {
                _hit[i].collider.gameObject.GetComponent<Enemy>().OnDamage(_baseDamage, _knockBack);
            }
            else if (_hit[i].collider.gameObject.CompareTag("Kid"))
            {
                _hit[i].collider.gameObject.GetComponent<Kid>().OnDamage();
            }
        }
    }

    public void OnDamage(float _damage, float _knockBack, Vector3 _enemyPos)
    {
        _health -= _damage;

        Vector2 _toEnemy = (_enemyPos - transform.position).normalized;
        _toEnemy = new Vector2(_toEnemy.x / Mathf.Abs(_toEnemy.x + Mathf.Epsilon), 0);

        _knockBacked = true;
        _rb.AddForce(-_toEnemy * _knockBack + Vector2.up * 5f, ForceMode2D.Impulse);
        StartCoroutine(StopKnockBack());

        if (_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Desactiver le collider, le rb, le script mais avant dire au gamemanager que c'est game over quoi 
    }

    void PickUpKid()
    {
        if (!_holdedObject)
        {
            RaycastHit2D _hit = Physics2D.Raycast(_collider.bounds.center, Vector2.down, _collider.bounds.extents.y);
            if (_hit && _hit.collider.gameObject.CompareTag("Kid"))
            {
                _holdedObject = _hit.collider.gameObject;
                _holdedObject.transform.position = transform.position + Vector3.up * (_collider.bounds.extents.y + _hit.collider.bounds.extents.x) + new Vector3(0,0, _holdedObject.transform.position.z);
                _holdedObject.transform.parent = transform;
                _holdedObject.GetComponent<Kid>()._isHeld = true;
                _holdedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _holdedObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                _holdedObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }
            else if(!_hit || !_hit.collider.gameObject.CompareTag("Kid"))
            {
                _hit = Physics2D.Raycast(_collider.bounds.center, transform.right, _collider.bounds.extents.x * 4);
                if (_hit && _hit.collider.gameObject.CompareTag("Kid"))
                {
                    _holdedObject = _hit.collider.gameObject;
                    _holdedObject.transform.position = transform.position + Vector3.up * (_collider.bounds.extents.y + _hit.collider.bounds.extents.x) + new Vector3(0, 0, _holdedObject.transform.position.z);
                    _holdedObject.transform.parent = transform;
                    _holdedObject.GetComponent<Kid>()._isHeld = true;
                    _holdedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    _holdedObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                    _holdedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }
        }
    }

    void DropKid()
    {
        if(_holdedObject && _grave && !_holdedObject.GetComponent<Kid>()._isDead)
        {
            if (!_grave.GetComponent<Grave>().IsFilled())
            {
                Destroy(_holdedObject);
                _holdedObject = null;
                _gameStuff._kidsBuried++;
                _grave.GetComponent<Grave>().Bury();
                _forDrop = false;
            }
        }
        else if (_holdedObject)
        {
            _holdedObject.transform.parent = null;
            _holdedObject.GetComponent<Kid>()._isHeld = false;
            _holdedObject.GetComponent<Rigidbody2D>().gravityScale = 2f;
            _holdedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            _holdedObject.GetComponent<Rigidbody2D>().velocity = _rb.velocity;
            _holdedObject = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grave"))
        {
            _grave = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Grave"))
        {
            _grave = null;
        }
    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(_attackCd);
        _canAttack = true;
        StopCoroutine(AttackCD());
    }

    IEnumerator StopKnockBack()
    {
        yield return new WaitForSeconds(.25f);
        _knockBacked = false;
        StopCoroutine(StopKnockBack());
    }

    IEnumerator HoldingAttack()
    {
        yield return new WaitForSecondsRealtime(.5f);
        _itsHold = true;
        PickUpKid();
    }
}
