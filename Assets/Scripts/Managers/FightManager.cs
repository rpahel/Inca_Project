using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data;

public class FightManager : MonoBehaviour
{
    public GameStuff _gameStuff;

    [Header("Enemies Stuff")]
    public float _spawnRange;
    private GameObject[] _enemies;
    [SerializeField]
    private List<GameObject> _activeEnemies = new List<GameObject>();
    private PowerType _resistance;
    private Vector2 _spawnStart;
    [SerializeField] ParticleSystem bloodParticles;
    [SerializeField] List<ParticleSystem> _listParticles;
    [SerializeField] List<GameObject> _listGameObject;

    private void Awake()
    {
        _gameStuff._spanishKilled = 0;
        _spawnStart = transform.position - new Vector3(_spawnRange, 0, 0);
        _enemies = _gameStuff._waves[_gameStuff._currentRound]._enemies;
        _resistance = _gameStuff._waves[_gameStuff._currentRound]._resist;
        FindObjectOfType<Player>()._power = _gameStuff._powerType;
    }

    private void Start()
    {
        for(int i = 0; i < _enemies.Length; i++)
        {
            GameObject _newEnemy = Instantiate(_enemies[i], new Vector3(_spawnStart.x + ((_spawnRange * 2) * i) / (float)_enemies.Length, 0, 1), Quaternion.identity);
            _newEnemy.GetComponent<Enemy>()._resist = _resistance;
            _newEnemy.GetComponent<Enemy>()._fightManager = this;
            _activeEnemies.Add(_newEnemy);
            _newEnemy.GetComponent<Enemy>().sang = bloodParticles;
        }
    }

    public void EnemyKilled(GameObject _enemy)
    {
        _activeEnemies.Remove(_enemy);
        Destroy(_enemy);
        _gameStuff._spanishKilled++;
    }

    public void UseUpPower(float _damage, PowerType _power)
    {

        for (int i = _activeEnemies.Count - 1; i > -1; i--)
        {
            StartCoroutine(UpPower(i,_damage,_power));
            
        }
        switch (_power)
        {
            case PowerType.NONE:
                //instantier les particules none
                break;
            case PowerType.Rock:
                Instantiate(_listParticles[1], GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.Euler(0, 0, 0));
                break;
            case PowerType.Fire:
                Instantiate(_listParticles[0], GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.Euler(0,0,0));
                break;
            case PowerType.Death:
                Instantiate(_listGameObject[0], GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
                break;
            case PowerType.Potato:
                Instantiate(_listGameObject[1], new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - 8, GameObject.FindGameObjectWithTag("Player").transform.position.y), Quaternion.identity);
                break;
            case PowerType.Thunder:
                Instantiate(_listGameObject[2], new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x - 8, GameObject.FindGameObjectWithTag("Player").transform.position.y), Quaternion.identity);
                break;
        }
    }
    IEnumerator UpPower(int i, float _damage, PowerType _power)
    {

        yield return new WaitForSeconds(0);
        _activeEnemies[i].GetComponent<Enemy>().OnPowerDamage(_damage, 0, _power);
    }
    public void Stun(float stunDuration)
    {
        foreach(GameObject enemy in _activeEnemies)
        {
            enemy.GetComponent<Enemy>().Stun(stunDuration);
        }
    }

    public void Potato(float duration)
    {
        foreach (GameObject enemy in _activeEnemies)
        {
            enemy.GetComponent<Enemy>().Potato(duration);
        }
    }

    public void Wind(float force, float duration)
    {
        foreach (GameObject enemy in _activeEnemies)
        {
            enemy.GetComponent<Enemy>().Wind(force, duration); ;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position - new Vector3(_spawnRange, 0, 0), transform.position + new Vector3(_spawnRange, 0, 0));
        Gizmos.DrawWireSphere(transform.position - new Vector3(_spawnRange, 0, 0), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(_spawnRange, 0, 0), 0.2f);
    }

    private void Update()
    {
        if(_activeEnemies.Count == 0)
        {
            _gameStuff._currentRound++;
            if(_gameStuff._currentRound >= _gameStuff._waves.Length)
            {
                _gameStuff._currentRound = 4;
            }
            SceneManager.LoadScene("Hub_Scene");
        }
    }
}
