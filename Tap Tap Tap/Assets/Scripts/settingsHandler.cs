using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class settingsHandler : MonoBehaviour {
    [SerializeField] TMP_InputField playerName;
    [SerializeField] Button back_btn;
    [SerializeField] Toggle easy_btn;
    [SerializeField] Toggle medium_btn;
    [SerializeField] Toggle hard_btn;
    [SerializeField] Button selectTile;
    [SerializeField] Image currentTile;
    [SerializeField] Button left, right;
    [SerializeField] Button sound;
    [SerializeField] TMP_Text sound_txt;
    [SerializeField] GameObject changeNamePanel;
    int tileRot;

    private void Awake() {
        if (back_btn != null) {
            back_btn.onClick.AddListener(back);
        }
        if (easy_btn != null) {
            easy_btn.onValueChanged.AddListener((bool value) => {
                easy();
            });
        }
        if (easy_btn != null) {
            medium_btn.onValueChanged.AddListener((bool value) => {
                medium();
            });
        }
        if (easy_btn != null) {
            hard_btn.onValueChanged.AddListener((bool value) => {
                hard();
            });
        }
        if (sound != null) {
            sound.onClick.AddListener(soundToggle);
        }
        // playerName.onSubmit.AddListener((string value) => {
        //     Debug.Log(value);
        //     GlobalDataHandler.Instance.playerName = value;
        // });
        playerName.onSelect.AddListener((string value)=>{
            changeNamePanel.SetActive(true);
        });

        selectTile.onClick.AddListener(() => {
            GlobalDataHandler.Instance.userTile = tileRot;
        });
        left.onClick.AddListener(() => {
            tileRot--;
            tileRot %= GlobalDataHandler.Instance.tiles.Length;
            currentTile.sprite = GlobalDataHandler.Instance.tiles[tileRot];
        });
        right.onClick.AddListener(() => {
            tileRot++;
            tileRot %= GlobalDataHandler.Instance.tiles.Length;
            currentTile.sprite = GlobalDataHandler.Instance.tiles[tileRot];
        });
    }

    private void Start() {
        tileRot = GlobalDataHandler.Instance.userTile;
        currentTile.sprite = GlobalDataHandler.Instance.tiles[tileRot];
        playerName.text = GlobalDataHandler.Instance.playerName;
        int diff = GlobalDataHandler.Instance.difficulty;
        if(diff == 2) {
            medium_btn.isOn = true;
        } else if (diff == 3) {
            hard_btn.isOn = true;
        } else if (diff == 1) {
            easy_btn.isOn = true;
        }
        if (GlobalDataHandler.Instance.sound) {
            AudioSourceControl.Instance.unmute();
            sound_txt.text = "sound: on";
        }
        else {
            AudioSourceControl.Instance.mute();
            sound_txt.text = "sound: off";
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

    private void soundToggle() {
        if (AudioSourceControl.Instance.Mute) {
            AudioSourceControl.Instance.unmute();
            sound_txt.text = "sound: on";
            GlobalDataHandler.Instance.sound = true;
        }
        else {
            AudioSourceControl.Instance.mute();
            sound_txt.text = "sound: off";
            GlobalDataHandler.Instance.sound = false;
        }
    }

    private void OnEnable() {
        playerName.text = GlobalDataHandler.Instance.playerName;
    }
}