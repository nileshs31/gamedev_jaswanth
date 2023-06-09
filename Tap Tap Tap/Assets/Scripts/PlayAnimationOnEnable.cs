using UnityEngine;

public class PlayAnimationOnEnable : MonoBehaviour{
    Animator myAnimator;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }
    private void OnEnable() {
        myAnimator.SetTrigger("trigger");
    }
}