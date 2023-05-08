using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveGoalslocal : MonoBehaviour {
    [SerializeField] private GameObject playerGoal;
    [SerializeField] private GameObject opponentGoal;
    public float speed;

    private void Update() {
        playerGoal.transform.position += Vector3.down * speed * Time.deltaTime;
        opponentGoal.transform.position += Vector3.up * speed * Time.deltaTime;
    }
}