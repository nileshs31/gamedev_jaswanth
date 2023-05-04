using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {
    [SerializeField] private Button localvs;
    [SerializeField] private Button quit_btn;
    [SerializeField] private Button vscpu;
    [SerializeField] private Button mulitplayer;

    private void Start() {
        localvs.onClick.AddListener(localVs);
        quit_btn.onClick.AddListener(quit);
        vscpu.onClick.AddListener(vsCpu);
        mulitplayer.onClick.AddListener(multiplayerOnline);
    }

    public void localVs() {
        // change the scene to game.
        // beware of coroutines..
        SceneManager.LoadScene("localVs");
    }

    public void vsCpu() {
        SceneManager.LoadScene("cpuVs");
    }

    public void multiplayerOnline() {
        SceneManager.LoadScene("multiplayer");
    }

    public void quit() {
        Application.Quit();
    }
}
