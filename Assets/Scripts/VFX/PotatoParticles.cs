using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoParticles : MonoBehaviour
{
    [SerializeField] float LifeTime;
    void Start()
    {
        StartCoroutine(Mort_programme());
    }
    IEnumerator Mort_programme()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }

}
