using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public interface IRelayManager {

    public async Task<string> CreateRelay() {
        try {
            Debug.Log("trying to start a relay");
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            Debug.Log("created allocation");

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("got the joinCode : " + joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            Debug.Log("Setup the network relay data");

            NetworkManager.Singleton.StartHost();
            Debug.Log("Started the host");
            return joinCode;
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string code) {
        try {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }
}