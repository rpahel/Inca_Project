using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRound : MonoBehaviour
{
    private bool _playerIn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIn = false;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && _playerIn)
        {
            SceneManager.LoadScene("Fight_Scene");
        }
    }
}
