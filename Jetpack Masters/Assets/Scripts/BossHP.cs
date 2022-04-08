using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{

	StudentController studentController;

	public int health = 100;


	public bool isInvulnerable = false;


	public GameObject bossHP;

	public Slider healthBar;

	public AudioSource bossExplosionSound;


	void Awake()
    {
		bossHP = GameObject.Find("Canvas").transform.Find("BossHP").gameObject;

		//Make the hp bar appear on the screen
		bossHP.SetActive(true);
		healthBar = bossHP.GetComponent<Slider>();
		studentController = GameObject.Find("Student").GetComponent<StudentController>();

		// Set the bar to max health
		healthBar.value = health;
	}

	void Update()
    {
	}

	public void TakeDamage(int damage)
	{
		if (isInvulnerable)
			return;

		health -= damage;

		StartCoroutine(DamageAnimation());

		if (health <= 51)
		{
			GetComponent<Animator>().SetBool("Enraged", true);
		}

		if (health <= 0)
		{
			Die();
			// Remove the HP bar from the screen
			bossHP.SetActive(false);
		}
	}

	void Die()
	{
		GetComponent<Animator>().SetBool("IsDeath", true);
		StartCoroutine(DestroyBoss(gameObject));
		bossExplosionSound.Play();
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		if (hitInfo.gameObject.CompareTag("Ammo"))
		{
			TakeDamage(10);
			healthBar.value = health;
		}
		
	}

	IEnumerator DestroyBoss(GameObject boss)
    {
		// Wait for the animation to finish
		yield return new WaitForSeconds(3.5f);
		Destroy(boss);
		studentController.ResumeRunning();
    }

	// Make the boss blink when hit by a shot
	IEnumerator DamageAnimation()
	{
		SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

		for (int i = 0; i < 3; i++)
		{
			foreach (SpriteRenderer sr in srs)
			{
				Color c = sr.color;
				c.a = 0;
				sr.color = c;
			}

			yield return new WaitForSeconds(.1f);

			foreach (SpriteRenderer sr in srs)
			{
				Color c = sr.color;
				c.a = 1;
				sr.color = c;
			}

			yield return new WaitForSeconds(.1f);
		}
	}

}
