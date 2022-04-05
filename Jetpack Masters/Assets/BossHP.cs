using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{

	public int health = 100;

	public GameObject deathEffect;

	public bool isInvulnerable = false;

	public void TakeDamage(int damage)
	{
		if (isInvulnerable)
			return;

		health -= damage;

		if (health <= 50)
		{
			GetComponent<Animator>().SetBool("Enraged", true);
		}

		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

}