using UnityEngine;
public class PlayASoundAnimation : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip Sound;

    public bool WantDifferentPitches;
    public void SFX()
    {
        if (WantDifferentPitches)
        {
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.5f);
        }
        AudioP.PlayOneShot(Sound);
    }
}
