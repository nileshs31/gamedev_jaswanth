using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTilesVsLocal : MonoBehaviour {
    [SerializeField] private Button player;
    [SerializeField] private Button opponent;

    // private int OtapCount;
    // use the above when using multiplayer to get the tapcount of opponent.

    private float maxTickDelay = 0.5f;

    private int PtapCount = 0;
    private int OtapCount = 0;

    public float strength = 0.1f;
    private float updateTickRate;

    private void Start() {
        updateTickRate = maxTickDelay;
        player.onClick.AddListener(()=> {
            PtapCount++;
        });
        opponent.onClick.AddListener(()=> {
            OtapCount++;
        });
    }

    private void Update() {
        updateTickRate -= Time.deltaTime;
        if (updateTickRate < 0) {
            float PclickRate = PtapCount / Time.deltaTime;
            float OclickRate = OtapCount / Time.deltaTime;

            this.gameObject.transform.position += Vector3.up * (PclickRate-OclickRate) * strength;
            OtapCount = 0;
            PtapCount = 0;
            updateTickRate = maxTickDelay;
        }
    }
}