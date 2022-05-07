using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayableCharacterScript : MonoBehaviour
{
    [SerializeField] float _launchforce = 70;
    [SerializeField] float _maxDragDistance = 3;

    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float JumpForce = 30f;
    private bool isAlive;
    private bool isJumping;
    private float MoveVertical;
    private float MoveHorizontal;
    private bool CurrentButton;
    private bool CanPulse;

    private Animator Playeranimator;

    public Rigidbody2D _rigidbody2d;

    void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        Playeranimator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d.isKinematic = false;

        isAlive = true;
        _launchforce = 400;
        MoveSpeed = 1.5f;
        JumpForce = 20f;
        isJumping = false;
        CanPulse = true;
    }

    

    void Update()
    {
        // Checks is a horizontal or vertical input is selected
        MoveHorizontal = Input.GetAxisRaw("Horizontal");
        MoveVertical = Input.GetAxisRaw("Vertical");
        // Checks if the current button pressed is "fire 1"
        CurrentButton = Input.GetButton("Fire1");
    }

    void FixedUpdate()
    {
        //If move horizontal is activated, play the walking animation and move in the direction * movespeed, if not stop walking animation
      if(MoveHorizontal > 0.1f || MoveHorizontal < -0.1f)
        {
            Playeranimator.SetBool("IsWalking", true);
            _rigidbody2d.AddForce(new Vector2(MoveHorizontal * MoveSpeed, 0f), ForceMode2D.Impulse);
        }
      else
        {
            Playeranimator.SetBool("IsWalking", false);
        }
        //If move vertical is activated and the player is not currently jumping, move the player up * jump force
        if (!isJumping && MoveVertical > 0.1f)
        {
            _rigidbody2d.AddForce(new Vector2(0f, MoveVertical * JumpForce), ForceMode2D.Impulse);
        }

        /* If the player has pressed fire 1 and the player can pulse,
        move the player in the vector which is the difference between the player
        and the current mouse position, further the mouse is the stronger the force. 
        Then disable the pulse and activate the pulse timer coroutine,
        preventing the player from pulsing until the pulse timer end */
       Vector3 difference = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(CurrentButton && CanPulse)
        {
            _rigidbody2d.AddForce(difference * _launchforce);
            CanPulse = false;
            StartCoroutine(pulsetimer());
            
        }

        if(isAlive == false)
        {
            playerdeath();
        }
    }
    
    // If the player collides with an object tagged "Hazard", kill them
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            isAlive = false;
        }
    }

    // If the player is not currently colliding with an object tagged platform, disable jumping
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
        }
    }

    // Player death, it changes scene to the game over scene
    public void playerdeath()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    //If the player is no longer colliding with a platform object, set their status to jumping
    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Platform")
        {
            isJumping = true;
        }
    }

    // Waits 2 seconds before reenabling the pulse
    IEnumerator pulsetimer()
    {
        yield return new WaitForSeconds(2);

        CanPulse = true;
    }
}
