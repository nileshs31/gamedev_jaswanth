using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public class MoveTilesMultiplayer : NetworkBehaviour {
    [SerializeField] private Button player;
    //[SerializeField] private GameObject deciderPrefab;
    private NetworkVariable<bool> isGameEnd;
    private NetworkVariable<bool> isGameStart; // this is used to wait for all the player to connect to the game;
    private NetworkVariable<char> winner;
    [SerializeField] private GameObject playerGoal;
    [SerializeField] private GameObject opponentGoal;
    [SerializeField] private TMP_Text winnerDisplay;
    [SerializeField] private Button goBackToMenu;
    [SerializeField] private GameObject winCond;
    [SerializeField] private Image winBackGround;
    private bool isWinnerDisplayed = false;

    private float strength = 0.4f;
    private int tapCount = 0;
    private Vector2 dummyTransform;
    private float screenScaleFactor = 1;
    // private Vector2 dummyGoalTransform;
    private float speed = 15f;

    private void Start() {
        // screenScaleFactor = Screen.height/1280f;
        Debug.Log(NetworkManager.Singleton.IsHost);
        Debug.Log(NetworkManager.Singleton.IsClient);
        Debug.Log(NetworkManager.Singleton.IsConnectedClient);
        AdLoadnShow.Instance.LoadAd();
        isGameEnd = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        winner = new NetworkVariable<char>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        isGameStart = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        winCond.SetActive(false); 
        //playerGoal.gameObject.SetActive(false);
        //opponentGoal.gameObject.SetActive(false);
        winnerDisplay.gameObject.SetActive(false);
        goBackToMenu.gameObject.SetActive(false);
        winBackGround.gameObject.SetActive(false);

        goBackToMenu.onClick.AddListener(() => {
            SceneManager.LoadScene("Menu");
        });
    }

    public override void OnNetworkSpawn() {
        
        // player.onClick.AddListener(() => {
        //     tapCount++;
        // });
        winCond.SetActive(true);
        //playerGoal.gameObject.SetActive(true);
        //opponentGoal.gameObject.SetActive(true);
        if (IsHost) {
            dummyTransform = new Vector2(0f, 0f);
            //dummyGoalTransform = new Vector2(0f, 640f);
        }

        NetworkManager.Singleton.OnClientConnectedCallback += (ulong a) =>{
            if(!isGameStart.Value && IsHost && NetworkManager.Singleton.ConnectedClientsList.Count == 2) isGameStart.Value = true;
            tapCount = 0;
            player.onClick.AddListener(() => {
                tapCount++;
            });
        };
    }

    private void Update() {
        if (!AdLoadnShow.Instance.isAdCompleted()) return;
        // if (!isGameStart.Value && IsHost && NetworkManager.Singleton.ConnectedClientsList.Count == 2) isGameStart.Value = true;
        if (!isGameStart.Value) return;
        if (isGameEnd.Value) {
            if (!isWinnerDisplayed) {
                winCond.SetActive(false);
                //playerGoal.gameObject.SetActive(false);
                //opponentGoal.gameObject.SetActive(false);
                // get who the winner is!!
                if ((winner.Value == 'h' && IsHost) || (winner.Value == 'c' && !IsHost && IsClient)) winnerDisplay.text = "YOU WON";
                else winnerDisplay.text = "YOU LOST";
                winBackGround.gameObject.SetActive(true);
                winnerDisplay.gameObject.SetActive(true);
                goBackToMenu.gameObject.SetActive(true);
                isWinnerDisplayed = true;
            }
            // NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
            return;
        }
        // screenScaleFactor = Screen.height/1280f;
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
            this.GetComponent<RectTransform>().anchoredPosition = dummyTransform * screenScaleFactor;
            //playerGoal.GetComponent<RectTransform>().anchoredPosition = new Vector2(dummyGoalTransform.x, dummyGoalTransform.y);
            //opponentGoal.GetComponent<RectTransform>().anchoredPosition = new Vector2(dummyGoalTransform.x, -1*dummyGoalTransform.y);
            ClientTileUpdateClientRpc(dummyTransform);
            checkGameEnd();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PosUpdateServerRpc(float clickrate,int dir) {
        dummyTransform += Vector2.up * clickrate * strength * dir;
        //dummyGoalTransform += Vector2.down * speed * Time.deltaTime;
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void TileUpdateServerRpc() {
    //    ClientTileUpdateClientRpc(decider.transform.position);
    //}

    [ClientRpc]
    private void ClientTileUpdateClientRpc(Vector2 pos) {
        if (IsHost) return;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x,-1*pos.y*screenScaleFactor);
        // playerGoal.GetComponent<RectTransform>().anchoredPosition = new Vector2(goalpos.x, goalpos.y);
        // opponentGoal.GetComponent<RectTransform>().anchoredPosition = new Vector2(goalpos.x, -1*goalpos.y);

    }

    public void checkGameEnd() {
        if (!IsHost) return;

        isGameEnd.Value =  isGameEnd.Value ||(this.transform.position.y >= playerGoal.transform.position.y) || (this.transform.position.y <= opponentGoal.transform.position.y);
        isGameEnd.Value = isGameEnd.Value || (isGameStart.Value && NetworkManager.Singleton.ConnectedClientsList.Count < 2);

        if (!isGameEnd.Value) return;
        
        if (this.transform.position.y >= playerGoal.transform.position.y && NetworkManager.Singleton.ConnectedClientsList.Count == 2) winner.Value = 'h';
        else winner.Value = 'c';
    }

    
}