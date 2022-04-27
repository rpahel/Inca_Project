using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[CreateAssetMenu(fileName = "New Right Power", menuName = "Powers/Right Power")]
public class PowerRight : ScriptableObject
{
    public PowerType _powerType;
    public float _damage, _range, _cd;
}