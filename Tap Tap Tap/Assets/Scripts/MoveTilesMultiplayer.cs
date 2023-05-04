using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

public class MoveTilesMultiplayer : NetworkBehaviour {
    [SerializeField] private Button player;
    //[SerializeField] private GameObject deciderPrefab;

    public float strength = 0.1f;
    private int tapCount = 0;
    private float updateTimer = 1f;
    private Vector2 dummyTransform;

    public override void OnNetworkSpawn() {
        player.onClick.AddListener(() => {
            tapCount++;
        });
        if (IsHost) {
            dummyTransform = new Vector2(0f, 0f);
        }
    }



    private void Update() {
        if (IsClient && !IsHost) {
            float clickrate = tapCount / Time.deltaTime;
            PosUpdateServerRpc(clickrate,-1);
            tapCount = 0;
        } else if (IsHost) {
            float clickrate = tapCount / Time.deltaTime;
            PosUpdateServerRpc(clickrate, +1);
            tapCount = 0;
        }

        // Now adjust the position of the tiles according to decider
        if (IsHost) {
            this.GetComponent<RectTransform>().anchoredPosition = dummyTransform;
            ClientTileUpdateClientRpc(dummyTransform);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PosUpdateServerRpc(float clickrate,int dir) {
        dummyTransform += Vector2.up * clickrate * strength * dir;
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void TileUpdateServerRpc() {
    //    ClientTileUpdateClientRpc(decider.transform.position);
    //}

    [ClientRpc]
    private void ClientTileUpdateClientRpc(Vector2 pos) {
        if (IsHost) return;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x,-1*pos.y);
    }
}