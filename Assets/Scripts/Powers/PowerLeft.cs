using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[CreateAssetMenu(fileName = "New Left Power", menuName = "Powers/Left Power")]
public class PowerLeft : ScriptableObject
{
    public PowerType _powerType;
    public float _heightScale, _resistScale, _dmgScale, _knockBackForce, _windForce, _stunDuration, _duration, _cd;
    public bool _potato;
}