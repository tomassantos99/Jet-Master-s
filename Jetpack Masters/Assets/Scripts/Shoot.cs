using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public int normalAttackDamage = 10;

    public int transformationAttackDamage = 25;

	public List<GameObject> currentAmmo;

    GameObject player;

    public Transform firePoint;

	public GameObject bulletPrefabPhase1;

	public GameObject bulletPrefabPhase2;

	// Start is called before the first frame update
	void Start()
	{
        player = GameObject.Find("Student");
        StartCoroutine(GeneratorCheck());
	}


	public void Attack()
	{
		GameObject bullet = (GameObject) Instantiate(bulletPrefabPhase1, firePoint.position, firePoint.rotation);

		currentAmmo.Add(bullet);
	}

	public void EnragedAttack()
	{
		GameObject bullet = (GameObject) Instantiate(bulletPrefabPhase2, firePoint.position, firePoint.rotation);

		currentAmmo.Add(bullet);
	}

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            List<GameObject> ammoToRemove = new List<GameObject>();
            foreach (var ammo in currentAmmo)
            {
                float playerX = player.transform.position.x;

                if (ammo != null)
                {
                    float lastAmmoX = ammo.transform.position.x;
                    if (lastAmmoX - playerX < -10)
                    {
                        ammoToRemove.Add(ammo);
                    }
                }
            }
            foreach (var ammo in ammoToRemove)
            {
                currentAmmo.Remove(ammo);
                Destroy(ammo);
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

}
