using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NextRound : MonoBehaviour
{
    private bool _playerIn;
    public GameStuff _gStuff;
    public GameObject _parchment;
    public TMP_Text _scriptedText;
    private int _heavy;
    private int _light;

    private void Start()
    {
        foreach (var enemy in _gStuff._waves[_gStuff._currentRound]._enemies)
        {
            if (enemy.GetComponent<Enemy>()._isBig)
            {
                _heavy++;
            }
            else
            {
                _light++;
            }
        }

        _scriptedText.text = "Wave n°" + _gStuff._currentRound + "\n\n" + _light + " light soldiers.\n" + _heavy + " heavy soldiers.\n\n" + "They resist to :\n" + "   " + _gStuff._waves[_gStuff._currentRound]._resist;

        if (_gStuff._waves[_gStuff._currentRound]._resist == Data.PowerType.NONE)
            _scriptedText.text = "Wave n°" + _gStuff._currentRound + "\n\n" + _light + " light soldiers.\n" + _heavy + " heavy soldiers.\n\n\n\n" + "They resist to :\n" + "   Nothing";

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIn = true;
            _parchment.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIn = false;
            _parchment.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && _playerIn)
        {
            FindObjectOfType<HubManager>().NextLevel();
        }
    }
}
