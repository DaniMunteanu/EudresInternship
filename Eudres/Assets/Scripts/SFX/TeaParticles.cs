using UnityEngine;

public class TeaParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    void Start()
    {
        particles.Stop();
    }

    public void PlayParticles(Color ingredientColor)
    {
        var main = particles.main;
        main.startColor = ingredientColor;

        particles.Play();
    }
}
