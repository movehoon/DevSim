using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("OnCollisionEnter with " + collision.gameObject.name);
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        Debug.Log("OnCollisionExit with " + collision.gameObject.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
