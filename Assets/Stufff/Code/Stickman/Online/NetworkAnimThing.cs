using Unity.Netcode.Components;

public class NetworkAnimThing : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
