using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;

public class MoveTilesMultiplayer : NetworkBehaviour {
    [SerializeField] private Button player;
    //[SerializeField] private GameObject deciderPrefab;
    private NetworkVariable<bool> isGameEnd;
    private NetworkVariable<bool> isGameStart; // this is used to wait for all the player to connect to the game;
    private NetworkVariable<char> winner;
    private NetworkVariable<float> countDownTimer;
    [SerializeField] private GameObject playerGoal;
    [SerializeField] private GameObject opponentGoal;
    [SerializeField] private TMP_Text winnerDisplay;
    [SerializeField] private GameObject winCond;
    [SerializeField] private Image winBackGround;
    [SerializeField] private TMP_Text timerDisplay;
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
        countDownTimer = new NetworkVariable<float>(60f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        winCond.SetActive(false); 
        //playerGoal.gameObject.SetActive(false);
        //opponentGoal.gameObject.SetActive(false);
        winnerDisplay.gameObject.SetActive(false);
        winBackGround.gameObject.SetActive(false);
        timerDisplay.gameObject.SetActive(false);
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
        timerDisplay.gameObject.SetActive(true);
        timerDisplay.text = "" + Mathf.Floor(countDownTimer.Value);
    }

    private void Update() {
        if (!AdLoadnShow.Instance.isAdCompleted()) return;
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

        // Update timer
        if(IsHost){
            countDownTimer.Value -= Time.deltaTime;
            if(countDownTimer.Value < 0){
                countDownTimer.Value = 0;
            }
        }
        timerDisplay.text = "" + Mathf.Floor(countDownTimer.Value);
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

        if(countDownTimer.Value < 10e-7){
            isGameEnd.Value = true;
            if(this.transform.position.y > 0) winner.Value = 'h';
            else winner.Value = 'c';
            return;
        }

        if (!isGameEnd.Value) return;
        
        if (this.transform.position.y >= playerGoal.transform.position.y && NetworkManager.Singleton.ConnectedClientsList.Count == 2) winner.Value = 'h';
        else winner.Value = 'c';
    }

    
}