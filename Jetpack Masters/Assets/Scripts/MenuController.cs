using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public string gameScene;
    public AudioSource newGameSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        StartCoroutine(NewGameRoutine());
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator NewGameRoutine()
    {

        newGameSound.Play();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(gameScene);
    }
}
