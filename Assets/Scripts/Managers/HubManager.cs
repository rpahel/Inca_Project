using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
using TMPro;

public class HubManager : MonoBehaviour
{
    public GameStuff _gameStuff;
    [SerializeField] private ParticleSystem sang;

    [Header("Kids Stuff")]
    public int _kidsKilled;
    public int _kidsBuried;
    public float _spawnRange;
    private GameObject _kid;
    private int _nbKids;
    private Vector2 _spawnStart;

    public GameObject _powerGivenObj;
    public TMP_Text _powerGivenText;

    private void Awake()
    {
        _kid = _gameStuff._kid;
        _nbKids = _gameStuff._baseNbOfKids + (int)(_gameStuff._spanishKilled * 0.2f);

        if( _kid == null)
        {
            throw new System.Exception("Kid is not known by GameStuff.");
        }

        if (_nbKids == 0)
        {
            throw new System.Exception("No kids to spawn.");
        }

        _gameStuff._kidsKilled = 0;
        _gameStuff._kidsBuried = 0;
        _gameStuff._powerType = Data.PowerType.NONE;

        _spawnStart = transform.position - new Vector3(_spawnRange, 0, 0);
    }

    public void Start()
    {
        for (int i = 0; i < _nbKids; i++)
        {
            GameObject _kidClone;
            _kidClone = Instantiate(_kid, new Vector3(_spawnStart.x + ((_spawnRange * 2) * i) / (float)_nbKids, 0, 1), Quaternion.identity);
            Kid _kidScript = _kidClone.GetComponent<Kid>();
            _kidScript._speed = Random.Range(1f, 5f);
            _kidScript._distance = Random.Range(2f, 4f);
            _kidScript.bloodParticles = sang;
        }
    }

    public void KidKilled()
    {
        _kidsKilled++;
        _gameStuff._kidsKilled = _kidsKilled;

        if (_kidsKilled > 2)
        {
            _powerGivenText.text = "The God of Mountains is appeased.";
        }
        if (_kidsKilled > 5)
        {
            _powerGivenText.text = "The God of Fire is appeased.";
        }
        if (_kidsKilled > 10)
        {
            _powerGivenText.text = "The God of Thunder is appeased.";
        }
        if (_kidsKilled > 15)
        {
            _powerGivenText.text = "The Goddess of Potatoes is appeased.";
        }
        if (_kidsBuried > 3 && _kidsKilled > 9)
        {
            _powerGivenText.text = "The God of Death is appeased.";
        }

        if(_kidsKilled == 3 || _kidsKilled == 6 || _kidsKilled == 11 || _kidsKilled == 16 || (_kidsKilled > 9 && _kidsBuried == 4) || (_kidsKilled == 10 && _kidsBuried > 3))
        {
            _powerGivenObj.SetActive(true);
            StartCoroutine(TextDisappear());
        }
    }

    public void KidBuried()
    {
        _kidsBuried++;
        _gameStuff._kidsBuried = _kidsBuried;

        if (_kidsBuried > 3 && _kidsKilled > 9)
        {
            _powerGivenText.text = "The God of Death is appeased.";
        }

        if ((_kidsKilled > 9 && _kidsBuried == 4) || (_kidsKilled == 10 && _kidsBuried > 3))
        {
            _powerGivenObj.SetActive(true);
            StartCoroutine(TextDisappear());
        }
    }

    public void NextLevel()
    {
        if(_kidsKilled > 2)
        {
            _gameStuff._powerType = PowerType.Rock;
        }
        if (_kidsKilled > 5)
        {
            _gameStuff._powerType = PowerType.Fire;
        }
        if (_kidsKilled > 10)
        {
            _gameStuff._powerType = PowerType.Thunder;
        }
        if (_kidsKilled > 15)
        {
            _gameStuff._powerType = PowerType.Potato;
        }

        if (_kidsBuried > 3 && _kidsKilled > 9)
        {
            _gameStuff._powerType = PowerType.Death;
        }

        SceneManager.LoadScene("Fight_Scene");
    }

    IEnumerator TextDisappear()
    {
        yield return new WaitForSeconds(3f);
        _powerGivenObj.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position - new Vector3(_spawnRange, 0, 0), transform.position + new Vector3(_spawnRange, 0, 0));
        Gizmos.DrawWireSphere(transform.position - new Vector3(_spawnRange, 0, 0), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(_spawnRange, 0, 0), 0.2f);
    }
}
