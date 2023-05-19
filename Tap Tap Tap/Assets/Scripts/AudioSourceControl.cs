using UnityEngine;

public class AudioSourceControl : MonoBehaviour
{
    public static AudioSourceControl Instance {
        get;
        set;
    }
    private void Awake() {
        Debug.Log("Script : GlobalDataHandler");

        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }

    [SerializeField] private AudioSource src;
    public AudioClip[] audioClips;

    public void playClip(int index) {
        src.PlayOneShot(audioClips[index]);
    }
}
