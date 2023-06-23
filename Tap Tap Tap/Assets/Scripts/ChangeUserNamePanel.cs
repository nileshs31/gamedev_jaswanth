using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeUserNamePanel : MonoBehaviour{
    [SerializeField] GameObject changeNamePanel;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField playername;

    [SerializeField] Button save;
    [SerializeField] Button cancel;

    private void saveUN(){
        if(username.text.Length > 0){
                GlobalDataHandler.Instance.playerName = username.text;
                playername.text = GlobalDataHandler.Instance.playerName;
                changeNamePanel.SetActive(false);
        } else {
            GameToastHandler.Instance.sendToast("Enter a Valid username");
        }
    }

    private void cancelUN(){
        changeNamePanel.SetActive(false);
    }
    private void firsttimelistner(){
        GameToastHandler.Instance.sendToast("Hey new guy!");
        GlobalDataHandler.Instance.firtTime = false;
        cancel.gameObject.SetActive(true);
        save.onClick.RemoveListener(firsttimelistner);
    }

    private void OnEnable() {
        cancel.onClick.AddListener(cancelUN);
        if(GlobalDataHandler.Instance.firtTime){
            cancel.gameObject.SetActive(false);
            save.onClick.AddListener(firsttimelistner);
        }
        save.onClick.AddListener(saveUN);
    }

    private void OnDisable() {
        save.onClick.RemoveListener(saveUN);
        cancel.onClick.RemoveListener(cancelUN);
    }

}