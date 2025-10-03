using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    ParticleSystem particles;
    float lifetime;

    void Awake()
    {
        particles = transform.GetChild(0).GetComponent<ParticleSystem>();
        lifetime = particles.main.duration;
    }

    void TryDestroyParticles()
    {
        bool has_no_particles = particles.particleCount == 0;
        bool is_playing = particles.isPlaying;
        if (has_no_particles && !is_playing)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0) TryDestroyParticles();
    }
}
