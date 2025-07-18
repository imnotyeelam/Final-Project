using UnityEngine;

public class PropCollector : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip hpClip;
    public AudioClip energyClip;
    public AudioClip ammoClip;
    public AudioClip pieceClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HPProp"))
        {
            UIManager.Instance.AddProp("HP");
            PlayClip(hpClip);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnergyProp"))
        {
            UIManager.Instance.AddProp("Energy");
            PlayClip(energyClip);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("AmmoProp"))
        {
            UIManager.Instance.AddProp("Ammo");
            PlayClip(ammoClip);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Piece"))
        {
            FindObjectOfType<PlayerVitalsManager>().CollectPiece();
            PlayClip(pieceClip);
            Destroy(other.gameObject);
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (clip && audioSource)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
