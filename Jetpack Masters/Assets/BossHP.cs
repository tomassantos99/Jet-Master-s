using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{

	StudentController studentController;

	public int health = 100;

	public GameObject deathEffect;

	public bool isInvulnerable = false;

	public GameObject prefabHP;

	public GameObject bossHP;

	public Slider healthBar;



	void Awake()
    {
		bossHP = GameObject.Find("Canvas").transform.Find("BossHP").gameObject;
		bossHP.SetActive(true);
		healthBar = bossHP.GetComponent<Slider>();
		studentController = GameObject.Find("Student").GetComponent<StudentController>();
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

		if (health <= 51)
		{
			GetComponent<Animator>().SetBool("Enraged", true);
		}

		if (health <= 0)
		{
			studentController.ResumeRunning();
			Die();
			bossHP.SetActive(false);
		}
	}

	void Die()
	{
		//Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		TakeDamage(10);
		healthBar.value = health;
	}

}
