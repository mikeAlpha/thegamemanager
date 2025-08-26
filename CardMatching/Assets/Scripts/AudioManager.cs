using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip matchClip,mismatchClip,cardPickClip,gameOverClip;

    private void OnEnable()
    {
        EventHandler.RegisterEvent<string>(GameStaticEvents.OnAudioUpdate,PlayClip);
    }

    private void OnDisable()
    {
        EventHandler.UnregisterEvent<string>(GameStaticEvents.OnAudioUpdate, PlayClip);
    }

    void PlayClip(string playType)
    {
        if (audioSource != null)
        {
            switch (playType)
            {
                case "card":
                    audioSource.PlayOneShot(cardPickClip);
                    break;
                case "match":
                    audioSource.PlayOneShot(matchClip);
                    break;
                case "mismatch":
                    audioSource.PlayOneShot(mismatchClip);
                    break;
                case "gameOver":
                    audioSource.PlayOneShot(gameOverClip);
                    break;
            }
        }
    }
}
