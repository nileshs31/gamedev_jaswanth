using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public void playAudio(int index) {
        AudioSourceControl.Instance.playClip(index);
    }
}
