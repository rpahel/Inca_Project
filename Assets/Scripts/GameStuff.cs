using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[CreateAssetMenu(fileName = "New GameStuff", menuName = "Don't Touch/GameStuff")]
public class GameStuff : ScriptableObject
{
    public PowerType _powerType;
    [HideInInspector]
    public bool _isPlayerDead;

    [Header("Waves")]
    public Wave[] _waves;
}
