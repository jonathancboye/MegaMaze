using UnityEngine;
using System.Collections;

public class GunButton : MonoBehaviour {

    public GameObject shootButton;
    GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag( "Player" );
    }

	public void Clicked() {
        player.SendMessage("ToggleGun");
    }
}
