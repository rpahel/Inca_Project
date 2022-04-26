using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float _speed;
    public float _jumpForce;
    public float _gForce;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    [Header("Attack")]
    public float _damage;
    public float _attackCd;
    [Tooltip("Force du knockBack inflig� aux ennemis.")]
    public float _knockBack;
    public Transform _sweepStart, _sweepEnd;
    private bool _canAttack;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gForce;
        _collider = GetComponent<Collider2D>();
        _canAttack = true;
    }

    void Update()
    {
        Movement();
        Jump();

        //Attack
        if (Input.GetButtonDown("Attack"))
        {
            if (_canAttack)
            {
                Attack();
            }
            else
            {
                Debug.Log("You are in Cooldown.");
            }
        }
    }

    private void Movement()
    {
        _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * _speed, _rb.velocity.y);

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
        Debug.DrawLine(_collider.bounds.center, _collider.bounds.center + Vector3.down * (_collider.bounds.extents.y + .01f), Color.red);

        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D _hit = Physics2D.Raycast(_collider.bounds.center, Vector2.down, _collider.bounds.extents.y + .01f);
            if (_hit)
            {
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Je touche pas le sol.");
            }
        }
    }

    private void Attack()
    {
        Debug.DrawLine(_sweepStart.position, _sweepEnd.position, Color.red, 0.5f);
        _canAttack = false;
        StartCoroutine(AttackCD());

        RaycastHit2D[] _hit = Physics2D.LinecastAll(_sweepStart.position, _sweepEnd.position);
        for (int i = 0; i < _hit.Length; i++)
        {
            if (_hit[i].collider.gameObject.CompareTag("Enemy"))
            {
                _hit[i].collider.gameObject.GetComponent<Enemy>().OnDamage(_damage, _knockBack);
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