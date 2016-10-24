using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardRoomTrigger : MonoBehaviour {

    public Text text;
    public GameObject restartButton;

    void Awake() {
        text.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log( "You win!" );
        text.enabled = true;
        GameManager.gameOver = true;
        restartButton.SetActive( true );
    }
}
