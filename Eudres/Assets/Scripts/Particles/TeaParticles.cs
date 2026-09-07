using UnityEngine;
using UnityEngine.UI;

public class TeaParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Image biscuitImage;

    void Start()
    {
        HideBiscuit();
        particles.Stop();
    }

    public void PlayParticles(Sprite particleSprite)
    {
        particles.Clear();

        particles.textureSheetAnimation.SetSprite(0, particleSprite);

        particles.Play();
    }

    public void ShowBiscuit()
    {
        biscuitImage.gameObject.SetActive(true);
    }

    public void HideBiscuit()
    {
        biscuitImage.gameObject.SetActive(false);
    }
}
