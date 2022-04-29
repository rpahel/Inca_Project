using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class supay : MonoBehaviour
{
    [SerializeField] ParticleSystem particlesSupay;
    [SerializeField] float _delay;
    void Start()
    {
        ParticleSystem teteParticles = Instantiate(particlesSupay, transform.position, Quaternion.Euler(0, 0, 0));
        StartCoroutine(Mort(teteParticles));
    }
    IEnumerator Mort(ParticleSystem particles)
    {
        yield return new WaitForSeconds(_delay);
        Destroy(particles);
        Destroy(gameObject);
    }
}
