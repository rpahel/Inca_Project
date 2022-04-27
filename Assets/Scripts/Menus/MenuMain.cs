using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
{
    public GameObject[] _buttons;
    public GameObject _buttonsScreen;
    public GameObject _settingsScreen;
    public Slider _volumeSlider;
    private GameObject _selectedButton;
    private int _selectedButtonIndex;
    private bool _inOptions;

    private void Start()
    {
        if(_buttons.Length == 0)
        {
            throw new System.Exception("Aucun bouton n'est assigné à MenuMain.");
        }

        _selectedButton = _buttons[0];
        _selectedButtonIndex = 0;

        for(int i = 1; i < _buttons.Length; i++)
        {
            _buttons[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!_inOptions)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q))
            {
                SwitchButton(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SwitchButton(1);
            }

            if (Input.GetButtonDown("Attack"))
            {
                _selectedButton.GetComponent<Button>().onClick.Invoke();
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Attack"))
            {
                _inOptions = false;
                _settingsScreen.SetActive(false);
                _buttonsScreen.SetActive(true);
            }

            _volumeSlider.value += Input.GetAxis("Horizontal") * Time.deltaTime;
        }
    }

    void SwitchButton(int _side)
    {
        _selectedButtonIndex += _side;

        if (_selectedButtonIndex >= _buttons.Length)
        {
            _selectedButtonIndex = 0;
        }
        else if(_selectedButtonIndex < 0)
        {
            _selectedButtonIndex = _buttons.Length - 1;
        }

        _selectedButton.SetActive(false);
        _selectedButton = _buttons[_selectedButtonIndex];
        _selectedButton.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Test_Raphael_1");
    }

    public void Options()
    {
        _inOptions = true;
        _buttonsScreen.SetActive(false);
        _settingsScreen.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
