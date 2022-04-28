using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_forest_générator : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;
    private GradientColorKey[] _colorKey;

    [SerializeField] private int _nbBg;
    [SerializeField] private float _DécalageX = 28.173f;
    public GameObject _Bg;

    [ContextMenu("Generator")]
    void BackgroudGenerator()
    {
        for(int i = 0; i < _nbBg; i++)
        {
            Vector3 position = new Vector3(transform.position.x + _DécalageX * i, transform.position.y, transform.position.z);
            GameObject background = Instantiate(_Bg, position, Quaternion.Euler(0, 0, 0));
            SpriteRenderer _renderer = background.GetComponent<SpriteRenderer>();
            _renderer.color = _gradient.Evaluate((float)i/_nbBg);
        }
    }
    
}
