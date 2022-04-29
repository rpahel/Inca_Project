using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cailloux : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticlesCailloux;
    void Start()
    {
        Instantiate(ParticlesCailloux,transform.position,Quaternion.Euler(0,0,135));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
