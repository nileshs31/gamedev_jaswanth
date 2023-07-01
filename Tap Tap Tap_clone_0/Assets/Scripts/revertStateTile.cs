using UnityEngine;
using UnityEngine.UI;

public class revertStateTile : MonoBehaviour
{
    private void OnEnable() {
        this.GetComponent<Image>().sprite = GlobalDataHandler.Instance.tiles[GlobalDataHandler.Instance.userTile];
    }
}
