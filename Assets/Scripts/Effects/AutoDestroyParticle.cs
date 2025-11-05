using UnityEngine;

// This component will automatically destroy the GameObject it is on
// after the Particle System finishes playing.
[RequireComponent(typeof(ParticleSystem))]
public class AutoDestroyParticle : MonoBehaviour 
{
    private ParticleSystem ps;

    void Start() 
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Check if the particle system is still alive
        if (ps == null || !ps.IsAlive())
        {
            // If it's finished, destroy the whole GameObject
            Destroy(gameObject);
        }
    }
}