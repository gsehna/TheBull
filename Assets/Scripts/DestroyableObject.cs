using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public ParticleSystem destructionParticles;
    public float adrenalineGain;
    public float scoreGain;

    public virtual void Destroy()
    {
        if (destructionParticles != null)
        {
            Instantiate(destructionParticles,transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
