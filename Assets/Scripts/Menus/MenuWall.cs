using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWall : MonoBehaviour
{
    public bool _isLeft;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_isLeft)
            {
                collision.gameObject.transform.position = new Vector3(-transform.position.x - 1f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
            }
            else
            {
                collision.gameObject.transform.position = new Vector3(-transform.position.x + 1f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
            }
        }
    }
}
