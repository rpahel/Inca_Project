using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum PowerType
    {
        NONE,
        Rock,
        Fire,
        Thunder,
        Death,
        Potato
    }

    [System.Serializable]
    public struct Wave
    {
        public GameObject[] _enemies;
        public PowerType _resist;
    }
}
