using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Est-ce que c'est un gros ennemi ?")]
    public bool _isBig;
    public float _health;

    [Header("Movement")]
    public float _speed;
    public float _jumpForce;
    public float _gForce;
    [Tooltip("Distance d'arrêt par rapport au joueur.")]
    public float _stopDistance;
    private Rigidbody2D _rb;
    //private Collider2D _collider;
    private Vector2 _toPlayer;
    private GameObject _player;

    [Header("Attack")]
    public float _damage;
    public float _attackCd;
    public Transform _sweepStart, _sweepEnd;
    public float _knockBack;
    private bool _canAttack;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_collider = GetComponent<Collider2D>();
        _canAttack = true;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
        Movement();

        if (Mathf.Abs((_player.transform.position - gameObject.transform.position).x) <= _stopDistance)
        {
            if (_canAttack)
            {
                Attack();
            }
            else
            {
                //Debug.Log(gameObject.name + " is in Cooldown.");
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
    }

    public void OnDamage(float _damage, float _knockBack)
    {
        _health -= _damage;

        if (!_isBig)
        {
            _rb.AddForce(-_toPlayer * _knockBack + Vector2.up * 2, ForceMode2D.Impulse);
        }

        if(_health <= 0)
        {
            Destroy(gameObject);
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

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(_attackCd);
        _canAttack = true;
        StopCoroutine(AttackCD());
    }
}
