using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

public class WindParticles : MonoBehaviour
{
    private ParticleSystem particles;

    private void Awake() {
        particles = GetComponent<ParticleSystem>();
    }

    private void Start() {
        StartCoroutine(nameof(WindUpdate));
    }

    private IEnumerator WindUpdate(){
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            ReadDir();
        }
    }

    private void ReadDir(){
        var vOverL = particles.velocityOverLifetime;
        vOverL.x = WindManager.WindDir.x;
        vOverL.z = WindManager.WindDir.z;
    }
}
