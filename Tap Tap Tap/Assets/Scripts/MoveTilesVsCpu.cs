using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTilesVsCpu : MonoBehaviour {
    [SerializeField] private Button player;
    [SerializeField] private Button opponent;

    // private int OtapCount;
    // use the above when using multiplayer to get the tapcount of opponent.

    private float maxTickDelay = 0.5f;

    private int tapCount = 0;
    public float strength = 0.1f;
    public float resStrength = 0.1f;
    private float Oclickrate = 0f;
    private float updateTickRate;

    private void Start() {
        updateTickRate = maxTickDelay;
        player.onClick.AddListener(moveTile);
    }

    private void Update() {
        updateTickRate -= Time.deltaTime;
        if (tapCount > 0) Debug.Log(tapCount);
        if (updateTickRate < 0) {
            float clickRate = tapCount / Time.deltaTime;
            float resRate = Random.Range(1f, 3f) / Time.deltaTime;
            this.gameObject.transform.position += Vector3.up * ((clickRate * strength) - (resRate * resStrength));
            tapCount = 0;
            updateTickRate = maxTickDelay;
        }
    }

    public void moveTile() {
        tapCount++;
    }
}