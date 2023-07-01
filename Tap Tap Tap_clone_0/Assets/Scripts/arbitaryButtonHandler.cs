using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class arbitaryButtonHandler : MonoBehaviour {
    [SerializeField] Button back_btn;

    private void Start() {
        if(back_btn != null) {
            back_btn.onClick.AddListener(back);
        }
    }

    private void back() {
        SceneManager.LoadScene("Menu");
    }

    public void tapAnyWhere() {
        SceneManager.LoadScene("Menu");
    }
}