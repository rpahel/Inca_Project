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
    public Transform _sweepStart, _sweepEnd;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gForce;
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        Movement();
        Jump();
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
}
