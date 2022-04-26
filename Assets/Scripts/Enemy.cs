using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float _speed;
    public float _jumpForce;
    public float _gForce;
    [Tooltip("Distance d'arrêt par rapport au joueur.")]
    public float _stopDistance;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Vector2 _toPlayer;
    private GameObject _player;

    [Header("Attack")]
    public float _damage;
    public float _attackCd;
    public Transform _sweepStart, _sweepEnd;
    private bool _canAttack;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();

        _rb.velocity = new Vector2(_toPlayer.x * _speed, _rb.velocity.y);

        if(Mathf.Abs((_player.transform.position - gameObject.transform.position).x) <= _stopDistance)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }


    }

    private void UpdateDirection()
    {
        _toPlayer = (_player.transform.position - gameObject.transform.position).normalized;
        _toPlayer = new Vector2(_toPlayer.x / Mathf.Abs(_toPlayer.x + Mathf.Epsilon), 0);
    }
}
