using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootButton : MonoBehaviour {
    public GameObject shootImage;
    GameObject player;

    void Start() {
        gameObject.SetActive( false );
        player = GameObject.FindGameObjectWithTag( "Player" );
    }

    public void Clicked() {
        player.SendMessage("ShootGun");
    }

    public void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
        shootImage.SetActive( !shootImage.activeSelf );
    }
}
