using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Patate;
    [SerializeField] private float _potatoDelay;
    [SerializeField] private int _nbPotato;
    [SerializeField] private Vector2 largeurHauteur = new Vector2(1920,1080);

    private void Start()
    {
        for(int i = 0; i < _nbPotato; i++)
        {
            StartCoroutine(Spawn((float)i));
        }
    }

    IEnumerator Spawn(float i)
    {
        yield return new WaitForSeconds(i * _potatoDelay);
        Vector2 position = new Vector2(transform.position.x + ((i/(_nbPotato+1))*largeurHauteur[0]),transform.position.y + largeurHauteur[1]);
        Instantiate(Patate,position,Quaternion.Euler(0,0,0));
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(transform.position.x,transform.position.y + largeurHauteur[1]), new Vector2(transform.position.x + largeurHauteur[0], transform.position.y + largeurHauteur[1]));
    }
}
