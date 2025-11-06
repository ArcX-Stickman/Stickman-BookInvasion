using UnityEngine;
public class PlayASoundAnimation : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip Sound;
    public void SFX()
    {
        AudioP.PlayOneShot(Sound);
    }
}
