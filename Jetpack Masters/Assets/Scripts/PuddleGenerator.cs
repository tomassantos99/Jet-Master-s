using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleGenerator : MonoBehaviour
{
    public List<GameObject> puddles;
    private List<GameObject> currentPuddles;
    private float currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPuddles = new List<GameObject>();
        GameObject puddle = GeneratePuddle();
        currentPuddles.Add(puddle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (transform.position.x - currentPuddles[0].transform.position.x > 2.3f)
        {
            GameObject puddle = currentPuddles[0];

            currentPuddles.RemoveAt(0);

            Destroy(puddle);

            puddle = GeneratePuddle();

            currentPuddles.Add(puddle);
        }
    }

    GameObject GeneratePuddle()
    {
        GameObject puddle = (GameObject)Instantiate(puddles[Random.Range(0, puddles.Count)]);
        float puddleX = transform.position.x + Random.Range(10, 34);

        puddle.transform.position = new Vector3(puddleX, -3.05f, 0.0f);

        return puddle;
    }
}
