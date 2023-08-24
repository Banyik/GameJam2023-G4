using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpParticleHandler : MonoBehaviour
{
    public ParticleSystem sys;
    public Material[] materials;

    public void PlayParticle(int particleMat)
    {
        sys.GetComponent<ParticleSystemRenderer>().material = materials[particleMat];
        sys.Play();
    }
}
