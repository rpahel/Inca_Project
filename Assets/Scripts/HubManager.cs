using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public GameStuff _gameStuff;

    public int _kidsKilled;

    private void Awake()
    {
        _gameStuff._kidsKilled = 0;
        _gameStuff._powerType = Data.PowerType.NONE;
    }

    public void KidKilled()
    {
        _kidsKilled++;
        _gameStuff._kidsKilled = _kidsKilled;
    }
}
