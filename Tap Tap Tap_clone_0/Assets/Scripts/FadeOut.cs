using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour{
    Image black;
    private void Start() {
        black = GetComponent<Image>();
    }
    private void Update() {
        if(black.color.a > 0){
            black.color = new Color(0f,0f,0f,black.color.a - (Time.deltaTime*0.5f));
        }else{
            GameObject.Destroy(this.gameObject);
        }
    }


}