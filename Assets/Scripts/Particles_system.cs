using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles_system : MonoBehaviour
{
    [SerializeField] private GameObject _sfxGameObject;
    [SerializeField] private ParticleSystem _sfxParticleSystem;

    void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startColor = new Color32(255, 0, 0, 255);
            enter[i] = p;
            Vector3 _newPos = new Vector3(p.position.x + transform.position.x, Random.Range(transform.position.y-0.5f, transform.position.y), 0);
            //Random.Range(-4f, -3f)
            
            if(_sfxGameObject != null)
            {
                Instantiate(_sfxGameObject, _newPos, Quaternion.Euler(0, 0, Random.Range(0, 90)));
            }
            if (_sfxParticleSystem != null)
            {
                Instantiate(_sfxParticleSystem, _newPos, Quaternion.Euler(0, 0, Random.Range(0, 90)));
            }
        }

        // set
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        
    }

}
