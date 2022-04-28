using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing_potato : MonoBehaviour
{
    [SerializeField] private float _ExplosionDelay;
    [SerializeField] private GameObject _petitePatate;
    void Start()
    {
        StartCoroutine(Explosion());
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds((float)_ExplosionDelay);
        int chiffre = Random.Range(0, 90);
        for(int i = 0; i < 4; i++)
        {
            GameObject _lilPotato = Instantiate(_petitePatate, transform.position, Quaternion.Euler(0, 0, chiffre + (90 * i)));
            Destroy(gameObject);
        }
    }
}
