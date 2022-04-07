using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject ammo;
    public List<GameObject> currentAmmo;
    public bool freeToShoot;
    public float shootingCooldown;
    private bool onCooldown;
    private float lastShot;
    private Rigidbody2D playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        freeToShoot = true;
        onCooldown = false;
        playerRigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (freeToShoot && !onCooldown)
            {
                GameObject keyboard = (GameObject)Instantiate(ammo);

                Vector3 startPosition = transform.position;

                keyboard.transform.position = new Vector3(startPosition.x + 0.5f, startPosition.y, 0);

                currentAmmo.Add(keyboard);

                onCooldown = true;
                lastShot = Time.realtimeSinceStartup;
            }
            else if (onCooldown)
            {
                double timeElapsed = Time.realtimeSinceStartup - lastShot;
                if (timeElapsed > shootingCooldown)
                {
                    onCooldown = false;
                }
            }
        }
    }

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            List<GameObject> ammoToRemove = new List<GameObject>();
            foreach (var ammo in currentAmmo)
            {
                if (ammo != null)
                {
                    float playerX = transform.position.x;
                    float lastAmmoX = ammo.transform.position.x;
                    float ammoY = ammo.transform.position.y;


                    if (playerX - lastAmmoX < -10 || ammoY > 50)
                    {
                        ammoToRemove.Add(ammo);
                    }
                }


            }

            foreach (var ammo in ammoToRemove)
            {
                currentAmmo.Remove(ammo);

                if (ammo != null)
                {
                    
                    Destroy(ammo);
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
