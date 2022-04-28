using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilPotato : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] float Speed;
    [SerializeField] float LifeTime;
    [SerializeField] ParticleSystem Particles;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = -transform.up * Speed;
        StartCoroutine(Mort_programmee());
    }
    IEnumerator Mort_programmee()
    {
        yield return new WaitForSeconds(LifeTime);
        Instantiate(Particles,transform.position,Quaternion.Euler(0,0,transform.rotation.z));
        Destroy(gameObject);
    }



}
