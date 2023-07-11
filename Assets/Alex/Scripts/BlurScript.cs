using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlurScript : MonoBehaviour
{
    public Volume blurVolume;
    public GameObject player;
    private float test;
    GameObject fearShit;
    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        fearShit = player.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        

        test = fearShit.GetComponent<fearArea>().distance;

        if (test != 10)
        {
            test = (-test + 1.5f) * 0.6f;
        } else
        {
            test = 0;
        }
        blurVolume.weight = test;

        sound.volume = test - 0.2f;

        
        
    }
}
