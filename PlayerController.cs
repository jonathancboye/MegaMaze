using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{

    public Transform firePoint;
    public Rigidbody2D[] breadCrumbs;

    private Animator anim;
    private Rigidbody2D rbody2D;
    private bool facingRight = true;
    private float maxSpeedX = 2f;
    private float maxSpeedY = 30f;
    private float maxvForce = 8f * 2;
    private float vForce = 15f;
    private int screenHeight;
    private int screenWidth;

    // Use this for initialization
    void Start() {
        anim = gameObject.GetComponent<Animator>();
        rbody2D = gameObject.GetComponent<Rigidbody2D>();
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    void FixedUpdate() {
        float moveX = Input.GetAxis( "Horizontal" );
        float moveY = Input.GetAxis( "Vertical" );     
        anim.SetFloat( "vSpeed", rbody2D.velocity.y );
        anim.SetFloat( "Speed", Mathf.Abs( rbody2D.velocity.x ) );
        rbody2D.velocity = new Vector2( maxSpeedX * moveX, rbody2D.velocity.y );

        if( moveX < 0 && facingRight || moveX > 0 && !facingRight ) {
            Flip();
        }
        if( Input.GetKeyDown( KeyCode.Q ) ) {
            ToggleGun();
        }
        if( Input.GetKeyDown( KeyCode.Space )) {
            ShootGun();
        }
        if( ( Input.GetKey( KeyCode.UpArrow ) || Input.GetKey( KeyCode.W ) )
            && rbody2D.velocity.y < maxSpeedY ) {
            rbody2D.AddForce( new Vector2( 0, vForce ) );
        }

        //androidFixedUpdate();
    }

    void androidFixedUpdate() {
        float move = 0;
        int leftWidth = screenWidth / 8;
        int rightWidth_topWidth = 7 * screenWidth / 8;
        if( Input.touchCount > 0 ) {
            Touch touch = Input.GetTouch( 0 );

            if( touch.position.x < ( leftWidth ) ) {
                move = -1;
            }
            if( touch.position.x > rightWidth_topWidth ) {
                move = 1;
            }
            if(    ( touch.phase == TouchPhase.Began && touch.position.x < ( leftWidth )     && facingRight )
                || ( touch.phase == TouchPhase.Began && touch.position.x > ( rightWidth_topWidth ) && !facingRight ) ) {
                Flip();
            }
            if( touch.position.y > screenHeight * 3 / 4
                && rbody2D.velocity.y < maxSpeedY ) {
                rbody2D.AddForce( new Vector2( 0, vForce ) );
            }
        }
        rbody2D.velocity = new Vector2( maxSpeedX * move, rbody2D.velocity.y );
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1f;
        transform.localScale = theScale;
    }

    void ShootGun() {
        if( anim.GetBool( "Gun" ) ) {
            int index = UnityEngine.Random.Range( 0, breadCrumbs.Length );
            Rigidbody2D clone = Instantiate( breadCrumbs[ index ], firePoint.position, Quaternion.identity ) as Rigidbody2D;
        }
    }

    void ToggleGun() {
        anim.SetBool( "Gun", !anim.GetBool( "Gun" ) );
    }
}
