using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        // Fait crash unity

        //int count = _activeEnemies.Count;
        //
        //for (int i = 0; i < count;)
        //{
        //    if(_activeEnemies[i].GetComponent<Enemy>()._resist == _power)
        //    {
        //        if(_activeEnemies[i].GetComponent<Enemy>()._health - (_damage - _damage * (_activeEnemies[i].GetComponent<Enemy>()._resistPercent / 100f)) > 0)
        //        {
        //            i++;
        //        }
        //    }
        //    else
        //    {
        //        if (_activeEnemies[i].GetComponent<Enemy>()._health - _damage > 0)
        //        {
        //            i++;
        //        }
        //    }
        //
        //    _activeEnemies[i].GetComponent<Enemy>().OnPowerDamage(_damage, 0, _power);
        //}
    }

    public void UseRightPower(float _damage, float _range, PowerType _power)
    {

    }

    public void Stun(float stunDuration)
    {

    }

    public void Potato(float duration)
    {

    }

    public void Wind(float force, float duration)
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position - new Vector3(_spawnRange, 0, 0), transform.position + new Vector3(_spawnRange, 0, 0));
        Gizmos.DrawWireSphere(transform.position - new Vector3(_spawnRange, 0, 0), 0.2f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(_spawnRange, 0, 0), 0.2f);
    }
}
