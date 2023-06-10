using UnityEngine;

public class PlayAnimationOnEnable : MonoBehaviour{
    Animator myAnimator;
    [SerializeField] ParticleSystem ps1;
    [SerializeField] ParticleSystem ps2;


    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }
    private void OnEnable() {
        myAnimator.SetTrigger("trigger");
        if(ps1!=null  && ps2!=null){
            ps1.Play();
            ps2.Play();
        }
    }
}