using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;

public class HubManager : MonoBehaviour
{
    public GameStuff _gameStuff;

    [Header("Kids Stuff")]
    public int _kidsKilled;
    public int _kidsBuried;
    public float _spawnRange;
    private GameObject _kid;
    private int _nbKids;
    private Vector2 _spawnStart;

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
        }
    }

    public void KidKilled()
    {
        _kidsKilled++;
        _gameStuff._kidsKilled = _kidsKilled;
    }

    public void KidBuried()
    {
        _kidsBuried++;
        _gameStuff._kidsBuried = _kidsBuried;
    }

    public void NextLevel()
    {
        if(_kidsKilled > 0)
        {
            _gameStuff._powerType = PowerType.Rock;
        }
        if (_kidsKilled > 1)
        {
            _gameStuff._powerType = PowerType.Fire;
        }
        if (_kidsKilled > 2)
        {
            _gameStuff._powerType = PowerType.Thunder;
        }
        if (_kidsKilled > 3)
        {
            _gameStuff._powerType = PowerType.Death;
        }

        if (_kidsBuried > 0)
        {
            _gameStuff._powerType = PowerType.Potato;
        }

        SceneManager.LoadScene("Fight_Scene");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position - new Vector3(_spawnRange, 0, 0), transform.position + new Vector3(_spawnRange, 0, 0));
        Gizmos.DrawWireSphere(transform.position - new Vector3(_spawnRange, 0, 0), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(_spawnRange, 0, 0), 0.2f);
    }
}
