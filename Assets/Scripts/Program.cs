using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program : MonoBehaviour
{
    public Transform sphere;

    int direction_command;

    public void OnButtonLeft()
    {
        direction_command = -1;
    }

    public void OnButtonStop()
    {
        direction_command = 0;
    }

    public void OnButtonRight()
    {
        direction_command = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (direction_command == -1)
        {
            sphere.Translate(-Time.deltaTime, 0, 0);
        }
        else if (direction_command == 1)
        {
            sphere.Translate(Time.deltaTime, 0, 0);
        }
    }
}
