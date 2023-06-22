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

    [SerializeField] private AudioSource srcSmall;
    [SerializeField] private AudioSource srcLarge;
    public AudioClip[] audioClips;
    public bool Mute;

    private void Start() {
        if(!GlobalDataHandler.Instance.sound){
            mute();
        }
    }

    public void playClip(int index) {
        srcSmall.PlayOneShot(audioClips[index]);
    }

    public void mute() {
        Mute = true;
        srcLarge.mute = Mute;
        srcSmall.mute = Mute;
    }

    public void unmute() {
        Mute = false;
        srcLarge.mute = Mute;
        srcSmall.mute = Mute;
    }
}
