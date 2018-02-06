using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = 2.0f;
    public float maxSpeed = 5.0f;
    private Vector3 jump;
    public float jumpForce = 2.0f;
    public float dashSpeed = 4;

    public GameObject gameModel;
    public Renderer modelRen;

    public GameObject teleportObjectOut;
    public GameObject teleportObjectIn;

    public Animator cameraAnim;

    public int lives = 3;

    public GameObject deathCollider;

    //Death Stuff
    public GameObject lastCheckPoint;
    public ObstacleSpawning curLevel;

    public Image dashFill;
    public Text healthNum;
    

    [Range(0, 100)]
    public float dashCharge;
    bool isCharging = false;

    bool isGrounded;

    Rigidbody rb;
    bool alive = true;
    bool killed = false;
    bool atPuzzle = false;

    float aliveTimer;
    const float aliveTimerMax = 1.0f;

    public Animator playerAnim;

    private AudioSource audioSource;
    [Header("Audio")]
    public AudioClip teleportSound;
    public AudioClip jumpSound;
    public AudioClip puzzleTurn;
    public AudioClip deathSound;
    public AudioClip pickUpSound;
    

	// Use this for initialization
	void Start ()
    {

        audioSource = GetComponent<AudioSource>();
        lives = lives + 1;
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        dashCharge = 100;

        //lastCheckPoint = Instantiate(new GameObject("Checkpoint"), transform.position, Quaternion.identity);
        //lastCheckPoint.transform.parent = curLevel.transform;

        lastCheckPoint = curLevel.checkpoint;

        Vector3 movement = new Vector3(0, 0.0f, 6);

        rb.velocity = movement;

        modelRen = gameModel.GetComponent<Renderer>();
    }

    void Update()
    {
        if (alive)
        {
            if (dashCharge < 100 && !isCharging)
            {
                InvokeRepeating("Charge", 2.0f, 0.5f);
                isCharging = true;
            }
        }
        dashFill.fillAmount = dashCharge / 100;
        healthNum.text = lives.ToString();

        if (Input.GetButtonDown("Action") && pieceToTurn)
        {
            pieceToTurn.transform.Rotate(0, pieceToTurn.transform.rotation.y + 90, 0);
            audioSource.PlayOneShot(puzzleTurn);

        }

        if(!atPuzzle)
            if (Input.GetButtonDown("Dash") && dashCharge > 0 && dashCharge >= 25) Dash();

        if (lives < 0)
            Death();


    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (alive)
        {
            float h = speed * Input.GetAxis("Horizontal");
            float v = speed * Input.GetAxis("Vertical");

            if (!atPuzzle)
            {
                maxSpeed = 6;
                v = 0;
            }
            else
                maxSpeed = 3;


            Vector3 movement = new Vector3(h, 0.0f, v);

            rb.AddForce(movement * speed);

            if (rb.velocity.z > maxSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
            }

            if (rb.velocity.z < -maxSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxSpeed);
            }

            if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);
            }

            if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, rb.velocity.z);
            }

            float Angle = 2.5f;

           Vector3 newVel = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
            if (atPuzzle)
                newVel = rb.velocity;

           Vector3 Axis = Vector3.Cross(newVel, -transform.up);
           gameModel.transform.Rotate(Axis, Angle, Space.World);


            if (!isGrounded && rb.velocity.y == 0)
            {
                isGrounded = true;
            }

            if (Input.GetButton("Jump") && isGrounded && !atPuzzle)
            {
                playerAnim.SetTrigger("Jump");
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                audioSource.PlayOneShot(jumpSound, 1.0f);
                isGrounded = false;
            }

            if (transform.position.y > 1)
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);


            //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
        }
    }

    void LateUpdate()
    {
        if (!modelRen.isVisible && alive)
        {
            alive = false;
            lives--;
            audioSource.PlayOneShot(deathSound, 0.5f);
            transform.position = lastCheckPoint.transform.position;
            dashCharge = 100;
            killed = true;
            PlatformController.moving = false;
        }

        if (modelRen.isVisible && !alive)
        {
            aliveTimer += Time.deltaTime;
            if (aliveTimer >= aliveTimerMax)
            {
                alive = true;
                aliveTimer = 0;
                PlatformController.moving = true;
            }
        }
    }

    void Dash()
    {
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //movement = Vector3.ClampMagnitude(movement, 1);

        playerAnim.SetTrigger("Teleport");
        audioSource.PlayOneShot(teleportSound, 0.5f);

       // Instantiate(teleportObjectOut, new Vector3( transform.position.x , 1.0f, transform.position.z), Quaternion.Euler(90 , 20, 50));

        transform.position = transform.position + new Vector3(0,0,1) * dashSpeed;

        if(transform.position.x > 4.3f)
        {
            transform.position = new Vector3(4.0f, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -4.3f)
        {
            transform.position = new Vector3(-4.0f, transform.position.y, transform.position.z);
        }

        dashCharge -= 25;

        Instantiate(teleportObjectIn, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.Euler(90, 20, 50), gameObject.transform);


    }

    void Charge()
    {
        if (dashCharge < 100)
            dashCharge += 0.5f;
        else
        {
            isCharging = false;
            CancelInvoke("Charge");
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            PlatformController.moving = false;
            cameraAnim.SetTrigger("Top");
            atPuzzle = true;
            playerAnim.SetBool("Idle", true);
            other.transform.parent.gameObject.GetComponent<ObstacleSpawning>().backWall.SetActive(true);
            //deathCollider.SetActive(false);
            if(!PlatformController.moving)
                other.transform.parent.gameObject.GetComponent<ObstacleSpawning>().SpawnNext();
        }


        if (other.CompareTag("Win"))
        {
            PlatformController.moving = true;
            SceneManager.LoadScene("winMenu");
        }

        if(other.CompareTag("Health"))
        {
            lives++;
            if (lives > 9)
                lives = 9;
            audioSource.PlayOneShot(pickUpSound, 0.5f);
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Dash"))
        {
            dashCharge += 25;
            audioSource.PlayOneShot(pickUpSound, 0.5f);
            Destroy(other.gameObject);
        }

    }

    //Puzzle piece to turn
    private GameObject pieceToTurn;

    void OnTriggerStay(Collider other)
    {
        //Puzzle
        if (other.CompareTag("PuzzlePiece"))
        {
            pieceToTurn = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            //lastCheckPoint.transform.position = transform.position;
            //cameraAnim.SetTrigger("Follow");
            //lastCheckPoint.transform.parent = curLevel.transform;
            other.isTrigger = false;
            atPuzzle = false;
            playerAnim.SetBool("Idle", false);
            //deathCollider.SetActive(true);

            PlatformController.moving = true;
        }

        if (other.gameObject.GetComponent<PuzzlePiece>())
        {
            pieceToTurn = null;
        }
    }

    void Death()
    {
        SceneManager.LoadScene("menu");
    }

}
