using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text lives;
    public GameObject youWin;
    public GameObject youLose;
    public Vector2 newposition;
    private int scoreValue;
    private int livesValue;
    public float jump;
    private bool goodgameover;
    private bool badgameover;
    private bool level2;
    public AudioClip bgaudio;
    public AudioClip victoryaudio;
    public AudioSource musicSource;
    //Animations:
    //0 = Idle
    //1 = Walk
    //2 = Jump
    private Animator anim;
    private bool facingRight;
    private bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score.text = "Score: " + scoreValue.ToString();
        youWin.SetActive(false);
        youLose.SetActive(false);
        livesValue = 3;
        lives.text = "Lives: " + livesValue.ToString();
        goodgameover = false;
        badgameover = false;
        level2 = false;
        musicSource.clip = bgaudio;
        musicSource.Play();
        musicSource.loop = true;
        facingRight = true;
        onGround = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (goodgameover == false && badgameover == false) {
            float hozMovement = Input.GetAxis("Horizontal");
            float vertMovement = Input.GetAxis("Vertical");
            rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
            anim.SetInteger("State", 0);
            if (facingRight == false && hozMovement > 0) {
                Flip();
            }
            if (facingRight == true && hozMovement < 0) {
                Flip();
            }
            if (hozMovement == 0 && onGround == false) {
                anim.SetInteger("State", 2);
            }
            if (hozMovement < 0 || hozMovement > 0) {
                anim.SetInteger("State", 1);
                if (onGround == false) {
                    anim.SetInteger("State", 2);
                }
            }
        }
        else {
            anim.enabled = false;
        }
       
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (scoreValue >= 4) {
            if (level2 == false) {
                 transform.position = newposition;
                 livesValue = 3;
                 lives.text = "Lives: " + livesValue.ToString();
                 level2 = true;
            }
        }
        if (scoreValue >= 8) {
            youWin.SetActive(true);
            goodgameover = true;
        }
        if (livesValue <= 0) {
            youLose.SetActive(true);
            badgameover = true;
            
        }
       

    }

    void Flip()
    {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }

    void BGMPlayer() {
        if (scoreValue >= 8) {
            musicSource.clip = victoryaudio;
            musicSource.Play();
            musicSource.loop = false;
        }
        else if (livesValue <= 0) {
            musicSource.Stop();
        }

    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Coin") {
                Destroy(collision.collider.gameObject);
                scoreValue += 1;
                score.text = "Score: " + scoreValue.ToString();
                BGMPlayer();
        }
        if (collision.collider.tag == "Enemy") {
                Destroy(collision.collider.gameObject);
                livesValue -= 1;
                lives.text = "Lives: " + livesValue.ToString();
                BGMPlayer();
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.tag == "Ground") {
            onGround = true;
            if (goodgameover == false && badgameover == false) {
                if (Input.GetKey(KeyCode.W)) {
                    rd2d.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
                    onGround = false;
                }
            }
            
        }
    }
}
