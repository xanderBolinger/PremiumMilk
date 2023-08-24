using Mirror;
using UnityEngine;

public class ChangeSoundTrack : MonoBehaviour
{
    public AudioClip music; 

    private AudioSource musicSource;

    private void Start()
    {
        musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<NetworkBehaviour>() != null 
            && other.gameObject.GetComponent<NetworkBehaviour>().isLocalPlayer) {
            musicSource.clip = music;
            musicSource.enabled = false;
            musicSource.enabled = true;
        }
    }

}
