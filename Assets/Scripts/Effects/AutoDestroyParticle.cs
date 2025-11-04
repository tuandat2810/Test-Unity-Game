using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour {
    void Start() {
        // Destroy after the particle system's duration
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}