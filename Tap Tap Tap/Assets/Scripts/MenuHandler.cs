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
    [SerializeField] private GameObject settings;
    [SerializeField] private Button settings_btn;
    [SerializeField] private GameObject nameChangePanel;

    private void Start() {
        settings.gameObject.SetActive(false);
        settings_btn.onClick.AddListener(settings_btn_fn);
        localvs.onClick.AddListener(localVs);
        quit_btn.onClick.AddListener(quit);
        vscpu.onClick.AddListener(vsCpu);
        mulitplayer.onClick.AddListener(multiplayerOnline);
        if(GlobalDataHandler.Instance.firtTime){
            nameChangePanel.SetActive(true);
        }
    }

    public void localVs() {
        GlobalDataHandler.Instance.gameMode = 1; // Just to handle offline game modes in a single scene
        SceneManager.LoadScene("local");
    }

    public void vsCpu() {
        GlobalDataHandler.Instance.gameMode = 0; // Just to handle offline game modes in a single scene
        SceneManager.LoadScene("local");
    }

    public void multiplayerOnline() {
        GlobalDataHandler.Instance.gameMode = 2;
        if(Application.internetReachability == NetworkReachability.NotReachable) {
            // Do nothing
		GameToastHandler.Instance.sendToast("Check your Internet");
        } else {
            SceneManager.LoadScene("multiplayer");
        }
    }

    public void quit() {
        Application.Quit();
    }

    public void settings_btn_fn() {
        settings.gameObject.SetActive(true);
        GameToastHandler.Instance.sendToast("settings here!");
    }
}
