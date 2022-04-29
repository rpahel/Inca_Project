using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles_autoDestroy : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    public bool _Destroy = true;

    void Start()
    {
        StartCoroutine(Mort_prog());
    }

    IEnumerator Mort_prog()
    {
        yield return new WaitForSeconds(_lifeTime);
        if (_Destroy)
        {
            Destroy(gameObject);
        }
    }
}
