using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[CreateAssetMenu(fileName = "New GameStuff", menuName = "Don't Touch/GameStuff")]
public class GameStuff : ScriptableObject
{
    public PowerManager _powerManager;
    public PowerType _powerType;

    [Header("Waves")]
    public Wave[] _waves;
}
