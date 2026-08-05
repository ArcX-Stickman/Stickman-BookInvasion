using UnityEngine;
public class PlayASoundAnimation : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip Sound;
    public AudioClip[] SoundV2;



    public bool WantDifferentPitches;
    public void SFX()
    {
        if (WantDifferentPitches)
        {
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.5f);
        }
        AudioP.PlayOneShot(Sound);
    }

    public void SFXNew(int i)
    {
        if (WantDifferentPitches)
        {
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.5f);
        }
        AudioP.PlayOneShot(SoundV2[i]);
    }
}
