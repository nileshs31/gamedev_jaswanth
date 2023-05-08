using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class settingsHandler : MonoBehaviour {
    [SerializeField] Button back_btn;
    [SerializeField] Toggle easy_btn;
    [SerializeField] Toggle medium_btn;
    [SerializeField] Toggle hard_btn;

    private void Awake() {
        if (back_btn != null) {
            back_btn.onClick.AddListener(back);
        }
        if (easy_btn != null) {
            easy_btn.onValueChanged.AddListener((value) => {
                easy();
            });
        }
        if (easy_btn != null) {
            medium_btn.onValueChanged.AddListener((value) => {
                medium();
            });
        }
        if (easy_btn != null) {
            hard_btn.onValueChanged.AddListener((value) => {
                hard();
            });
        }
    }

    private void Start() {
        int diff = GlobalDataHandler.Instance.difficulty;
        if(diff == 2) {
            medium_btn.isOn = true;
        } else if (diff == 3) {
            hard_btn.isOn = true;
        } else if (diff == 1) {
            easy_btn.isOn = true;
        }
    }

    private void back() {
        this.gameObject.SetActive(false);
    }

    public void easy() {
        if (easy_btn.isOn) {
            medium_btn.isOn = false;
            hard_btn.isOn = false;
            GlobalDataHandler.Instance.difficulty = 1;
        }
        else {
            if (!medium_btn.isOn && !hard_btn.isOn) {
                easy_btn.isOn = true;
            }
        }
    }

    private void medium() {
        if (medium_btn.isOn) {
            easy_btn.isOn = false;
            hard_btn.isOn = false;
            GlobalDataHandler.Instance.difficulty = 2;
        }
        else {
            if (!easy_btn.isOn && !hard_btn.isOn) {
                medium_btn.isOn = true;
            }
        }
    }
    private void hard() {
        if (hard_btn.isOn) {
            medium_btn.isOn = false;
            easy_btn.isOn = false;
            GlobalDataHandler.Instance.difficulty = 3;
        }
        else {
            if (!medium_btn.isOn && !easy_btn.isOn) {
                hard_btn.isOn = true;
            }
        }
    }
}