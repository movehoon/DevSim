using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public Program program;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("OnCollisionEnter with " + collision.gameObject.name);
        if (collision.gameObject.name.Equals("GuardLeft"))
        {
            program.OnHitLeft(true);
        }
        else if (collision.gameObject.name.Equals("GuardRight"))
        {
            program.OnHitRight(true);
        }
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        Debug.Log("OnCollisionExit with " + collision.gameObject.name);
        if (collision.gameObject.name.Equals("GuardLeft"))
        {
            program.OnHitLeft(false);
        }
        else if (collision.gameObject.name.Equals("GuardRight"))
        {
            program.OnHitRight(false);
        }
    }
}
