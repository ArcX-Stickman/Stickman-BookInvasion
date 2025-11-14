using UnityEngine;

public class Particle2d : MonoBehaviour
{
    public float fixedZ = 0f;
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            particles[i].position = new Vector3(particles[i].position.x, particles[i].position.y, fixedZ);
        }

        ps.SetParticles(particles, count);
    }
}
