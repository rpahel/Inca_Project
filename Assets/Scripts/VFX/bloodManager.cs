using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodManager : MonoBehaviour
{
    public GameObject gosse;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gosse = GameObject.FindWithTag("Kid");
    }
}
