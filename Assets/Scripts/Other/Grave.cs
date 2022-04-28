using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    private bool _isFilled;
    public Sprite _filledGrave;

    public bool IsFilled()
    {
        return _isFilled;
    }

    public void Bury()
    {
        _isFilled = true;
        GetComponent<SpriteRenderer>().sprite = _filledGrave;
    }
}
