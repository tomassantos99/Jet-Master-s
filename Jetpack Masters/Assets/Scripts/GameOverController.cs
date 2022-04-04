using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{

    public string mainMenuScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        Scene gameScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(gameScene.name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
