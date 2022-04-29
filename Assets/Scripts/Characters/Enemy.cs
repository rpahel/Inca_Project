using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Enemy : MonoBehaviour
{
    [Tooltip("Est-ce que c'est un gros ennemi ?")]
    public bool _isBig;
    public float _health;
    public PowerType _resist;
    [Range(0f, 100f)]
    public float _resistPercent;
    [HideInInspector]
    public FightManager _fightManager;

    [Header("Movement")]
    public float _speed;
    public float _gScale;
    [Tooltip("Distance d'arrêt par rapport au joueur.")]
    public float _stopDistance;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Vector2 _toPlayer;
    private GameObject _player;
    private bool _isStunned;
    private float _windForce;

    [Header("Attack")]
    public float _damage;
    public float _attackCd;
    public Transform _sweepStart, _sweepEnd;
    public float _knockBack;
    private bool _canAttack;
    private bool _isPotato;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _canAttack = true;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPotato)
        {
            return;
        }

        if (_windForce > 0)
        {
            if (!_isBig)
            {
                _rb.velocity += new Vector2(_windForce, 0);
            }
            else
            {
                _rb.velocity += new Vector2(_windForce / 8, 0);
            }
        }

        if (_isStunned)
        {
            return;
        }

        UpdateDirection();
        Movement();

        if (Mathf.Abs((_player.transform.position - gameObject.transform.position).x) <= _stopDistance)
        {
            if (_canAttack)
            {
                Attack();
            }
        }
    }

    private void UpdateDirection()
    {
        _toPlayer = (_player.transform.position - gameObject.transform.position).normalized;
        _toPlayer = new Vector2(_toPlayer.x / Mathf.Abs(_toPlayer.x + Mathf.Epsilon), 0);
    }

    private void Movement()
    {
        if(_rb.velocity.y <= 0)
        {
            if (_toPlayer.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (_toPlayer.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            _rb.velocity = new Vector2(_toPlayer.x * _speed, _rb.velocity.y);

            if (Mathf.Abs((_player.transform.position - gameObject.transform.position).x) <= _stopDistance)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }

        RaycastHit2D _hit = Physics2D.Raycast(_collider.bounds.center, transform.right, (2 * _collider.bounds.extents.x) * 1.25f);
        if(_hit && _hit.collider.gameObject.CompareTag("Enemy"))
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    public void OnDamage(float _damage, float _knockBack)
    {
        _health -= _damage;

        if (!_isBig)
        {
            _rb.AddForce(-_toPlayer * _knockBack + Vector2.up * 50, ForceMode2D.Impulse);
        }

        if(_health <= 0)
        {
            _fightManager.EnemyKilled(this.gameObject);
        }
    }

    private void Attack()
    {
        Debug.DrawLine(_sweepStart.position, _sweepEnd.position, Color.red, 0.5f);
        _canAttack = false;
        StartCoroutine(AttackCD());

        RaycastHit2D _hit = Physics2D.Linecast(_sweepStart.position, _sweepEnd.position);

        if (_hit)
        {
            if (_hit.collider.gameObject.CompareTag("Player"))
            {
                _hit.collider.gameObject.GetComponent<Player>().OnDamage(_damage, _knockBack, transform.position);
            }
        }
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCd(duration));
        _isStunned = true;
    }

    public void Potato(float duration)
    {
        _isPotato = true;
        StartCoroutine(PotatoCd(duration));
    }

    public void Wind(float force, float duration)
    {
        _windForce = force;
        StartCoroutine(WindCd(duration));
    }

    public void OnPowerDamage(float _damage, float _knockBack, PowerType _type)
    {
        if(_type == _resist)
        {
            _health -= (_damage - _damage * (_resistPercent / 100f));
        }
        else
        {
            _health -= _damage;
        }

        if (!_isBig)
        {
            _rb.AddForce(-_toPlayer * _knockBack + Vector2.up * 125, ForceMode2D.Impulse);
        }

        if (_health <= 0)
        {
            _fightManager.EnemyKilled(this.gameObject);
        }
    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(_attackCd);
        _canAttack = true;
        StopCoroutine(AttackCD());
    }

    IEnumerator StunCd(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        _isStunned = false;
        StopCoroutine(StunCd(stunDuration));
    }

    IEnumerator WindCd(float duration)
    {
        yield return new WaitForSeconds(duration);
        _windForce = 0;
        StopCoroutine(WindCd(duration));
    }

    IEnumerator PotatoCd(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isPotato = false;
        StopCoroutine(PotatoCd(duration));
    }
}
