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
			Die();
			bossHP.SetActive(false);
		}
	}

	void Die()
	{
		GetComponent<Animator>().SetBool("IsDeath", true);
		StartCoroutine(DestroyBoss(gameObject));
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		TakeDamage(10);
		healthBar.value = health;
	}

	IEnumerator DestroyBoss(GameObject boss)
    {
		yield return new WaitForSeconds(3.5f);
		Destroy(boss);
		studentController.ResumeRunning();
    }

}
