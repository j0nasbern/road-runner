using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personaje : MonoBehaviour {

    public float aceleracion = 0.001f;
    public float startSpeed = 3;
    private float realSpeed;
    public float speed = 8;
    public float realLeftRightSpeed = 8;
    private int segundosInmune;
    private int vidaMaxima;
    public GameObject hitFX;
    public AudioSource hitSound;

    public int vidaActual;
    private Rigidbody rigidBody;
    private Animator animator;

    private bool isInmune = false;

    private float nextTimeKickInmune = 0;

    private float speedMultiply = 0;

    public float speedBoost = 2.5f;
    private bool boostActive = false;
    private float timeSinceBoost = 0;
    private int boostTimeAmount = 5;

    public int currentCoinAmount;
    private int coinValue;
    private int startingCoinAmount;

    public GameObject scoreManagerObj;
    private ScoreManager scoreManager;

    public Text playerLivesText, shieldTimerText, boostTimerText, coinAmountText;
    public GameObject shieldUI, boostUI;
    // Use this for initialization
    void Start()
    {
        scoreManager = scoreManagerObj.GetComponent<ScoreManager>();

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        
        rigidBody.freezeRotation = true;

        //Changes the power-ups based on the player's upgrades
        vidaMaxima = 3 + PlayerPrefs.GetInt("HeartLevel", 0);
        vidaActual =  vidaMaxima;

        segundosInmune = 4 + PlayerPrefs.GetInt("ShieldLevel", 0);
        
        coinValue = 1 + PlayerPrefs.GetInt("CoinLevel", 0);
        
        //Saves the amount of coins the player had when the game started and sets the amount of coins for the current run as 0
        startingCoinAmount = PlayerPrefs.GetInt("CoinAmount", 0);
        currentCoinAmount = 0;
        

        playerLivesText.text = vidaActual.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Only updates the value of the realSpeed of the character if it isn't dead
        if (!EstaMuerto())
        {
            //Vamos a ir incrementando la velocidad a la que tiene que ir en intervalos regulares hasta alcanzar la velocidad que le corresponde
            speedMultiply += aceleracion;
            if (speedMultiply > 1) speedMultiply = 1;

            realSpeed = startSpeed + speedMultiply * speed;
        }

        Vector3 movimiento = Vector3.forward * realSpeed * Time.deltaTime;

        if (Input.GetKey("left"))
        {
            movimiento += Vector3.left * realLeftRightSpeed * Time.deltaTime;
        }

        if (Input.GetKey("right"))
        {
            movimiento += Vector3.right * realLeftRightSpeed * Time.deltaTime;
        }

        transform.position += movimiento;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8, 8), transform.position.y, transform.position.z);

        if (isInmune)
        {
            if (Time.timeSinceLevelLoad >= nextTimeKickInmune)
            {
                AcabarInmunidad();
                
            }
            shieldTimerText.text = "" + (int)(nextTimeKickInmune + 1 - Time.timeSinceLevelLoad);
        }

        if (boostActive)
        {
            if (Time.timeSinceLevelLoad >= timeSinceBoost + boostTimeAmount)
            {
                speed -= speedBoost;
                boostActive = false;
                boostUI.SetActive(false);
            }
            boostTimerText.text = "" + (int)(boostTimeAmount + timeSinceBoost + 1 - Time.timeSinceLevelLoad);
        }

        coinAmountText.text = (startingCoinAmount + currentCoinAmount).ToString();

    }

    public void DetenerPersonaje()
    {
        boostActive = false;
        realSpeed = 0;
        realLeftRightSpeed = 0;
        animator.SetTrigger("Detener");
    }

    public void RestarVida()
    {
        if (!isInmune)
        {
            hitFX.SetActive(true);
            hitSound.pitch = Random.Range(0.8f, 1.2f);
            hitSound.Play();
            vidaActual--;
            playerLivesText.text = vidaActual.ToString();
        }
    }

    public bool EstaMuerto()
    {
        if (vidaActual <= 0) return true;
        else return false;
    }

    public void AcabarInmunidad()
    {
        //rigidBody.isKinematic = false;
        isInmune = false;
        shieldUI.SetActive(false);
    }
    public void EmpezarInmunidad()
    {
        //rigidBody.isKinematic = true;
        isInmune = true;
        nextTimeKickInmune = Time.timeSinceLevelLoad + segundosInmune;
        shieldUI.SetActive(true);
    }

    public void BloqueGolpeado()
    {
        RestarVida();
        if (!isInmune)
        {
            EmpezarInmunidad();
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if (!EstaMuerto())
            {
                BloqueGolpeado();
            }
            
        } 
        //If the player gets a coin, it adds score and it adds the value of the coin to the current coin amount of the run
        else if (other.tag == "Coin")
        {
            currentCoinAmount += coinValue;
            scoreManager.AumentarScoreCoin();
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        } 
        //If the player isn't full HP, it adds another health point to the player
        else if (other.tag == "Heart")
        {
            if (vidaActual < vidaMaxima) vidaActual++;
            playerLivesText.text = vidaActual.ToString();
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        } 
        //If the player gets a lightning, it gives a speed boost to the player
        else if (other.tag == "Lightning")
        {
            if (!boostActive)
            {
                boostActive = true;
                speed += speedBoost;
                boostUI.SetActive(true);
            }
            timeSinceBoost = Time.timeSinceLevelLoad;
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        //If the player gets a shield, it gives imunity to the player
        else if (other.tag == "Shield")
        {
            EmpezarInmunidad();
            
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
