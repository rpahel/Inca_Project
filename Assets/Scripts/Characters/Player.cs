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
    public HubManager _hubManager;
    public FightManager _fightManager;

    [Header("Power")]
    public PowerType _power;
    public PowerLeft _powerLeft;
    public PowerRight _powerRight;
    public PowerUp _powerUp;
    private float _resistance;
    private float _leftPowerCD;
    private float _rightPowerCD;
    private float _upPowerCD;
    private float _leftPowerDuration;

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

    [Header("Original Values")]
    private Vector3 _iniScale;
    private float _iniResistance;
    private float _iniBaseDamage;
    private float _iniKnockBack;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gScale;
        _collider = GetComponent<Collider2D>();
        _canAttack = true;
        _anim = GetComponent<Animator>();
        _holdedObject = null;
        _resistance = 1;

        _iniScale = transform.localScale;
        _iniResistance = _resistance;
        _iniBaseDamage = _baseDamage;
        _iniKnockBack = _knockBack;
    }

    private void Start()
    {
        _hubManager = FindObjectOfType<HubManager>();
        _fightManager = FindObjectOfType<FightManager>();

        GivePowers();
    }

    void Update()
    {
        Movement();
        Jump();
        AttackCheck();
        UsePowers();
        UpdatePowers();
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
                _forDrop = true;
            }
        }

        if (_isJumping)
        return;

        if (Input.GetButtonDown("Attack"))
        {
            _lastCoroutine = StartCoroutine(HoldingAttack());
            _itsHold = false;
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
                }
            }

           _forDrop = false;
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
                _holdedObject.transform.position = transform.position + Vector3.up * (_collider.bounds.extents.y + _hit.collider.bounds.extents.x * .5f) + new Vector3(0,0, _holdedObject.transform.position.z);
                _holdedObject.transform.parent = transform;
                _holdedObject.GetComponent<Kid>()._isHeld = true;
                _holdedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                _holdedObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                _holdedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                if (!_holdedObject.GetComponent<Kid>()._isDead)
                {
                    _holdedObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
            }
            else if(!_hit || !_hit.collider.gameObject.CompareTag("Kid"))
            {
                _hit = Physics2D.Raycast(_collider.bounds.center + -transform.right * (_collider.bounds.extents.x - 0.01f), transform.right, _collider.bounds.extents.x * 4);
                if (_hit && _hit.collider.gameObject.CompareTag("Kid"))
                {
                    _holdedObject = _hit.collider.gameObject;
                    _holdedObject.transform.position = transform.position + Vector3.up * (_collider.bounds.extents.y + _hit.collider.bounds.extents.x * .5f) + new Vector3(0, 0, _holdedObject.transform.position.z);
                    _holdedObject.transform.rotation = Quaternion.Euler(0, 0, 90);
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
                _hubManager.KidBuried();
                _grave.GetComponent<Grave>().Bury();
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

    void GivePowers()
    {
        for (int i = 0; i < _powerManager._leftPowers.Length; i++)
        {
            if (_powerManager._leftPowers[i]._powerType == _power)
            {
                _powerLeft = _powerManager._leftPowers[i];
                break;
            }
        }

        for (int i = 0; i < _powerManager._rightPowers.Length; i++)
        {
            if (_powerManager._rightPowers[i]._powerType == _power)
            {
                _powerRight = _powerManager._rightPowers[i];
                break;
            }
        }

        for (int i = 0; i < _powerManager._upPowers.Length; i++)
        {
            if (_powerManager._upPowers[i]._powerType == _power)
            {
                _powerUp = _powerManager._upPowers[i];
                break;
            }
        }
    }

    void UsePowers()
    {
        if (Input.GetButtonDown("LeftPower") && _powerLeft != null && _leftPowerCD == 0)
        {
            transform.localScale = Vector3.one * _powerLeft._heightScale;
            _resistance = _powerLeft._resistScale;
            _baseDamage *= _powerLeft._dmgScale;
            _knockBack *= _powerLeft._knockBackForce;
            _leftPowerDuration = _powerLeft._duration;
            _leftPowerCD = _powerLeft._cd;

            if(_powerLeft._windForce > 0)
            {
                _fightManager.Wind(_powerLeft._windForce, _leftPowerDuration);
            }

            if(_powerLeft._stunDuration > 0)
            {
                _fightManager.Stun(_powerLeft._stunDuration);
            }

            if (_powerLeft._potato)
            {
                _fightManager.Potato(_leftPowerDuration);
            }
        }
        else if(_leftPowerCD > 0)
        {
            Debug.Log("Left Power is in cooldown");
        }

        if (Input.GetButtonDown("RightPower") && _powerRight != null && _rightPowerCD == 0)
        {
            _fightManager.UseRightPower(_powerRight._damage, _powerRight._range, _power);
            _rightPowerCD = _powerRight._cd;
        }
        else if (_rightPowerCD > 0)
        {
            Debug.Log("Right Power is in cooldown");
        }

        if (Input.GetButtonDown("UpPower") && _powerUp != null && _upPowerCD == 0)
        {
            _fightManager.UseUpPower(_powerUp._damage, _power);
            _upPowerCD = _powerUp._cd;
        }
        else if (_upPowerCD > 0)
        {
            Debug.Log("Up Power is in cooldown");
        }
    }

    void UpdatePowers()
    {
        _leftPowerCD -= Time.deltaTime;
        _rightPowerCD -= Time.deltaTime;
        _upPowerCD -= Time.deltaTime;

        _leftPowerCD = Mathf.Clamp(_leftPowerCD, 0, 100);
        _rightPowerCD = Mathf.Clamp(_rightPowerCD, 0, 100);
        _upPowerCD = Mathf.Clamp(_upPowerCD, 0, 100);

        _leftPowerDuration -= Time.deltaTime;
        _leftPowerDuration = Mathf.Clamp(_leftPowerDuration, 0, 100);

        if(_leftPowerDuration == 0)
        {
            transform.localScale = _iniScale;
            _resistance = _iniResistance;
            _baseDamage = _iniBaseDamage;
            _knockBack = _iniKnockBack;
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
