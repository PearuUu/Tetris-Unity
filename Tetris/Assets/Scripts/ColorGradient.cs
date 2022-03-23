using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradient : MonoBehaviour
{
    Camera mainCamera;

    [SerializeField] float timer;
    [SerializeField] Color[] colors;
    int i = 0;


    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        mainCamera.backgroundColor = colors[0];
    }
    // Update is called once per frame
    void Update()
    {
        Gradient();
    }

    void Gradient()
    {
        if(timer <= Time.deltaTime)
        {
            //Debug.Log(i);
            if(i == colors.Length-1)
            {
                i = 0;
            }
            mainCamera.backgroundColor = colors[i];

            i++;
            timer = 2.0f;
        }
        else
        {
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, colors[i], Time.deltaTime / timer);    

            timer -= Time.deltaTime;
        }
        
    }


}
