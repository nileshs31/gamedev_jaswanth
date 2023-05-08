using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MoveTilesOffline : MonoBehaviour {
    GlobalDataHandler gdh = GlobalDataHandler.Instance;
    [SerializeField] private Button player;
    [SerializeField] private Button opponent;
    [SerializeField] private GameObject playerGoal;
    [SerializeField] private GameObject opponentGoal;
    [SerializeField] private TMP_Text winnerDisplay;
    [SerializeField] private Button goBackToMenu;

    private float maxTickDelay = 0.5f;

    private int PtapCount = 0;
    private int OtapCount = 0;

    private float strength = 0.1f;
    private float updateTickRate;
    private int dif;
    private bool isWinnerDisplayed = false;

    private void Start() {
        winnerDisplay.gameObject.SetActive(false);
        goBackToMenu.gameObject.SetActive(false);
        player.onClick.AddListener(() => {
            PtapCount++;
        });
        updateTickRate = maxTickDelay;
        if (gdh.gameMode == 0) {
            // copy start of vsCPU
            dif = gdh.difficulty;
        } else if (gdh.gameMode == 1) {
            // copy start of vsLocal
            dif = 1;
            opponent.onClick.AddListener(() => {
                OtapCount++;
            });
        }
    }

    private void Update() {
        if (gameEnd()) {
            if (!isWinnerDisplayed) {
                // pop a winner window and return to menu;
                if (this.transform.position.y >= playerGoal.transform.position.y) winnerDisplay.text = "You Won :)";
                else winnerDisplay.text = "You lost :(";
                winnerDisplay.gameObject.SetActive(true);
                goBackToMenu.gameObject.SetActive(true);
                isWinnerDisplayed = true;
                goBackToMenu.onClick.AddListener(() => {
                    SceneManager.LoadScene("Menu");
                });
            }
            return;
        }
        if (gdh.gameMode == 0) {
            // copy start of vsCPU
            OtapCount = UnityEngine.Random.Range(1, 4);
        }
        else if (gdh.gameMode == 1) {
            // copy start of vsLocal
        }
        updateTickRate -= Time.deltaTime;
        if (updateTickRate < 0) {
            float PclickRate = PtapCount / Time.deltaTime;
            float OclickRate = OtapCount / Time.deltaTime;

            this.gameObject.transform.position += Vector3.up * (PclickRate - (OclickRate*dif)) * strength;
            OtapCount = 0;
            PtapCount = 0;
            updateTickRate = maxTickDelay;
        }

    }

    /*
        GAME END:
            1.Two objects move from father edge and move towards center.
            2.Who ever touches the opposite on wins. (could be checked with a simple dummytransform in the script).
    */

    public bool gameEnd() {
        bool gameEndCon = (this.transform.position.y >= playerGoal.transform.position.y) || (this.transform.position.y <= opponentGoal.transform.position.y);
        return gameEndCon;
    }

}
