using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundsSensor : MonoBehaviour
{

    public bool isGrounded;

    
    void OntriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            isGrounded = true;
        } 
    }

    
    void OntriggeStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        isGrounded = false;
    }
}
