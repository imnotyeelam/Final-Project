using UnityEngine;

public class Sound1 : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource ªÚ clip Œ¥…Ë÷√£°");
        }
    }
}
