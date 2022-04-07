using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class StudentController : MonoBehaviour
{
    public GameObject myPrefab;
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    public ParticleSystem mainJetpack;
    public ParticleSystem miniJetpack;
    private bool jetpackActive;

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

    private bool spedUp;
    private double speedUpStart;
    private int startDistance;

    private ShootingController shootingController;

    public AudioClip coinCollectSound;
    public AudioSource jetpackAudio;
    public int studentHealth = 30;
    public int spawnedBosses = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
        bossBattleActive = false;
        isBossSpawned = false;
        spedUp = false;
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();
        studentAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        initialPosition = studentTransform.position;
        shootingController = GetComponent<ShootingController>();
        jetpackActive = false;
        AdjustJetpackSound(jetpackActive);

        DisableSpedUpSprites();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (running)
        {

            if (!studentAnimator.GetBool("isDead"))
            {
                jetpackActive = false;

                jetpackActive = Input.GetButton("Fire1");

                if (jetpackActive)
                {
                    playerRigidbody.AddForce(new Vector2(0, jetpackForce));
                }

                if ((distanceTravelled + 30) / 60 > spawnedBosses)
                {
                    bossBattleActive = true;
                }


                if (distanceTravelled / 60 > spawnedBosses)
                {
                    if (spedUp)
                    {
                        EndSpeedUp();
                    }
                    playerRigidbody.angularVelocity = 0.0f;

                    Instantiate(myPrefab, new Vector2(playerRigidbody.position.x + 10, playerRigidbody.position.y), Quaternion.identity);
                    isBossSpawned = true;
                    spawnedBosses++;
                }

                else if (spedUp)
                {
                    SpeedUpMovement();
                    UpdateGroundedStatus();
                    AdjustJetpack(jetpackActive);
                }
                else
                {
                    if (!bossBattleActive || !isBossSpawned)
                    {
                        Vector2 newVelocity = playerRigidbody.velocity;
                        newVelocity.x = forwardMovementSpeed;
                        playerRigidbody.velocity = newVelocity;
                    }

                    UpdateGroundedStatus();
                    AdjustJetpack(jetpackActive);
                }
                AdjustJetpackSound(jetpackActive);
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

    public void ResumeRunning()
    {
        isBossSpawned = false;
        bossBattleActive = false;
        RegenHP();
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
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
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
        else if ((collider.gameObject.CompareTag("Zapper") || collider.gameObject.CompareTag("Puddle") || collider.gameObject.CompareTag("Drone")) && !spedUp)
        {
            if (HasSHield())
            {
                DeactivateShield();
            }
            else
            {
                collider.gameObject.GetComponent<AudioSource>().Play();
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

                    // Calls ResetEffect after "duration" seconds even if gameObject is inactive
                    Invoke(nameof(ResetEffect), 6f);
                }

            }
        }
        else if (collider.gameObject.CompareTag("BulletPhase1"))
        {
            if (HasSHield())
            {
                DeactivateShield();
            }
            else
            {
                if (studentHealth > 10)
                {
                    studentHealth -= 10;
                    //studentHP.value = studentHealth;
                }
                else
                {
                    Die();
                }

            }
        }
        else if (collider.gameObject.CompareTag("BulletPhase2"))
        {
            if (HasSHield())
            {
                DeactivateShield();
            }
            else
            {
                if (studentHealth > 25)
                {
                    studentHealth -= 25;
                    //studentHP.value = studentHealth;
                }
                else
                {
                    Die();
                }

            }
        }
        else if (collider.gameObject.CompareTag("Can"))

        {
            spedUp = true;
            speedUpStart = Time.realtimeSinceStartup;
            startDistance = distanceTravelled;
            playerRigidbody.gravityScale = 0;
            EnableSpedUpSprites();
            shootingController.freeToShoot = false;
            Destroy(collider.gameObject);
        }
    }

    private void ResetEffect()
    {
        DeactivateShield();
    }

    void Die()
    {
        studentAnimator.SetBool("isDead", true);
        playerRigidbody.freezeRotation = false;
        StartCoroutine(GameOver());
    }

    void RegenHP()
    {
        studentHealth = 100;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        long score = distanceTravelled + coins;
        long previousHighscore = ReadPreviousHighScore();
        if (score > previousHighscore)
        {
            WriteNewHighScore(score);
            previousHighscore = score;
        }
        gameOverPanel.transform.Find("Score").gameObject.GetComponent<Text>().text = "Score: " + score;
        gameOverPanel.transform.Find("High Score").gameObject.GetComponent<Text>().text = "High Score: " + previousHighscore;
        gameOverPanel.SetActive(true);
    }

    void SpeedUpMovement()
    {
        double timeElapsed = Time.realtimeSinceStartup - speedUpStart;
        if (timeElapsed >= 1f)
        {
            float Xvel = playerRigidbody.velocity.x;
            if (Xvel <= 20f)
                playerRigidbody.AddForce(new Vector2(50f, 0f));
            else
                playerRigidbody.velocity = new Vector2(40f, 0f);

            if (distanceTravelled - startDistance > 50)
            {
                EndSpeedUp();
            }
        }
        else
        {
            float Ypos = playerRigidbody.transform.position.y;
            if (Ypos > 0.5 || Ypos < -0.5)
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -1f * (Ypos / Mathf.Abs(Ypos)));
            else
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f);
            playerRigidbody.AddForce(new Vector2(-10f, 0f));

            Vector3 rot = new Vector3(0f, 90f - (90f * (float)timeElapsed), 0f);
            transform.Find("Wing").gameObject.transform.rotation = Quaternion.Euler(rot);
        }
    }

    void EnableSpedUpSprites()
    {
        transform.Find("SpdShield").gameObject.GetComponent<Renderer>().enabled = true;
        transform.Find("Wing").gameObject.GetComponent<Renderer>().enabled = true;
    }

    void DisableSpedUpSprites()
    {
        transform.Find("SpdShield").gameObject.GetComponent<Renderer>().enabled = false;
        transform.Find("Wing").gameObject.GetComponent<Renderer>().enabled = false;
    }

    void EndSpeedUp()
    {
        DisableSpedUpSprites();
        spedUp = false;
        playerRigidbody.gravityScale = 1;
        playerRigidbody.velocity = new Vector2(forwardMovementSpeed, 0f);
        shootingController.freeToShoot = true;
    }

    private long ReadPreviousHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.txt";
        try
        {
            StreamReader reader = new StreamReader(path);
            string fileContent = reader.ReadToEnd().Trim();
            if (string.IsNullOrEmpty(fileContent))
            {
                return 0;
            }
            long highScore = long.Parse(fileContent);
            reader.Close();
            reader.Dispose();
            return highScore;
        }
        catch (FileNotFoundException)
        {
            return 0;
        }
        finally
        {
        }
    }

    private void WriteNewHighScore(long newHighScore)
    {
        string path = Application.persistentDataPath + "/highscore.txt";

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(newHighScore.ToString());

        writer.Close();
        writer.Dispose();
    }

    void AdjustJetpackSound(bool jetpackActive)
    {
        jetpackAudio.enabled = !studentAnimator.GetBool("isDead") && !isGrounded;
        if (jetpackActive)
        {
            jetpackAudio.volume = 0.1f;
        }
        else
        {
            jetpackAudio.volume = 0.05f;
        }
    }
}
