using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerManager", menuName = "Don't Touch/PowerManager")]
public class PowerManager : ScriptableObject
{
    [Header("Up Powers")]
    public PowerUp[] _upPowers;

    [Header("Left Powers")]
    public PowerLeft[] _leftPowers;

    [Header("Right Powers")]
    public PowerRight[] _rightPowers;
}
