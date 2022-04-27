using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[CreateAssetMenu(fileName = "New Up Power", menuName = "Powers/Up Power")]
public class PowerUp : ScriptableObject
{
    public PowerType _powerType;
    public float _damage, _cd;
}
