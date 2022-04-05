using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StudentController : MonoBehaviour
{
    public GameObject myPrefab;
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    public ParticleSystem mainJetpack;
    public ParticleSystem miniJetpack;

    private Rigidbody2D playerRigidbody;

    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator studentAnimator;

    private uint coins = 0;
    public Text coinsCollectedLabel;

    public Transform studentTransform;
    private Vector3 initialPosition;
    private int distanceTravelled;
    public Text totalDistanceTravelledLabel;

    public bool bossBattleActive;
    public bool isBossSpawned;

    GameObject shield;

    public GameObject countdownPanel;
    private bool running = false;
    public GameObject gameOverPanel;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
        bossBattleActive = false;
        isBossSpawned = false;
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();
        studentAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        initialPosition = studentTransform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (running)
        {
            if(distanceTravelled > 20)
            {
                bossBattleActive = true;
            }
            if (distanceTravelled > 50)
            {

                playerRigidbody.angularVelocity = 0.0f;
                jetpackActive = Input.GetButton("Fire1");
                if (jetpackActive)
                {
                    playerRigidbody.AddForce(new Vector2(0, jetpackForce));
                }
                UpdateGroundedStatus();
                AdjustJetpack(jetpackActive);
                if (!isBossSpawned)
                {
                    Instantiate(myPrefab, new Vector2(playerRigidbody.position.x + 10, playerRigidbody.position.y), Quaternion.identity);
                    isBossSpawned = true;
                }

            }
            else
            {
                jetpackActive = Input.GetButton("Fire1");
                if (jetpackActive)
                {
                    playerRigidbody.AddForce(new Vector2(0, jetpackForce));
                }
                Vector2 newVelocity = playerRigidbody.velocity;
                newVelocity.x = forwardMovementSpeed;
                playerRigidbody.velocity = newVelocity;

                UpdateGroundedStatus();
                AdjustJetpack(jetpackActive);
            }
        }
        else
        {
            var mainJetpackEmission = mainJetpack.emission;
            var miniJetpackEmission = miniJetpack.emission;
            mainJetpackEmission.enabled = false;
            miniJetpackEmission.enabled = false;
            if (transform.eulerAngles.z <= 270.0f && transform.eulerAngles.z > 10.0f)
            {
                playerRigidbody.angularVelocity = 0.0f;
                transform.eulerAngles = new Vector3(0, 0, -90.0f);
            }
            else
            {
                var mainJetpackEmission = mainJetpack.emission;
                var miniJetpackEmission = miniJetpack.emission;
                mainJetpackEmission.enabled = false;
                miniJetpackEmission.enabled = false;
                if (transform.eulerAngles.z <= 270.0f && transform.eulerAngles.z > 10.0f)
                {
                    playerRigidbody.angularVelocity = 0.0f;
                    transform.eulerAngles = new Vector3(0, 0, -90.0f);
                }
                else
                    playerRigidbody.angularVelocity = -150.0f;
            }
            distanceTravelled = Mathf.RoundToInt(Vector3.Distance(studentTransform.position, initialPosition));
            totalDistanceTravelledLabel.text = distanceTravelled.ToString() + " m";
        }
    }

    IEnumerator Countdown()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(2);
        for (int i = 3; i >= 0; i--)
        {
            countdownPanel.transform.Find("Display Text").gameObject.GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownPanel.SetActive(false);
        shield = transform.Find("Shield").gameObject;
        studentAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        initialPosition = studentTransform.position;
        running = true;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    void ActivateShield()
    {
        shield.SetActive(true);
    }

    void DeactivateShield()
    {
        shield.SetActive(false);
    }

    bool HasSHield()
    {
        return shield.activeSelf;
    }

    void UpdateGroundedStatus()
    {
        //1
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        //2
        studentAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var mainJetpackEmission = mainJetpack.emission;
        var miniJetpackEmission = miniJetpack.emission;
        mainJetpackEmission.enabled = !isGrounded;
        miniJetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
        {
            mainJetpackEmission.rateOverTime = 300.0f;
            miniJetpackEmission.rateOverTime = 300.0f;
        }
        else
        {
            mainJetpackEmission.rateOverTime = 75.0f;
            miniJetpackEmission.rateOverTime = 300.0f;
        }
    }

    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        coinsCollectedLabel.text = coins.ToString();
        Destroy(coinCollider.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins"))
        {
            CollectCoin(collider);
        }
        else if (collider.gameObject.CompareTag("Zapper") || collider.gameObject.CompareTag("Puddle"))
        {
            if (HasSHield())
            {
                DeactivateShield();
            }
            else
            {
                Die();
            }
        }
        else if (collider.gameObject.CompareTag("Shield"))
        {
            PowerUpScript powerUp = collider.GetComponent<PowerUpScript>();
            if (powerUp)
            {
                if (powerUp.activateShield)
                {
                    ActivateShield();
                    Destroy(collider.gameObject);
                }

            }
        }
    }

    void Die()
    {
        studentAnimator.SetBool("isDead", true);
        playerRigidbody.freezeRotation = false;
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        long score = distanceTravelled + coins;
        gameOverPanel.transform.Find("Score").gameObject.GetComponent<Text>().text = "Score: " + score;
        gameOverPanel.SetActive(true);
    }
}
