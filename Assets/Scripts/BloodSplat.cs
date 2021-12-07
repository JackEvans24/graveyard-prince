using System.Collections;
using UnityEngine;

public class BloodSplat : MonoBehaviour
{
    [SerializeField] private SoundFromArray stabbing;
    [SerializeField] private ParticleSystem blood;

    public IEnumerator Play()
    {
        this.blood.Play();
        yield return this.stabbing.Play();
    }
}
