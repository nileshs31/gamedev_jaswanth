using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveGoalslocal : MonoBehaviour {
    [SerializeField] private GameObject playerGoal;
    [SerializeField] private GameObject opponentGoal;
    public float speed = 0f;

    private void Awake() {
        AdLoadnShow.Instance.LoadAd();
    }

    private void Update() {
        if (!AdLoadnShow.Instance.isAdCompleted()) return;
        playerGoal.transform.position += Vector3.down * speed * Time.deltaTime;
        opponentGoal.transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private void OnEnable() {
        speed = 10f;
    }

    private void OnDisable() {
        speed = 0f;
    }
}