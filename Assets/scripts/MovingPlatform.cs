using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float topLimit = 6f;
    public float bottomLimit = 2f;

    private bool movingUp;

    void Start()
    {
        
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, bottomLimit, topLimit);
        transform.position = pos;

        
        if (Mathf.Approximately(pos.y, topLimit))
        {
            movingUp = false;
        }
        else if (Mathf.Approximately(pos.y, bottomLimit))
        {
            movingUp = true;
        }
        else
        {
            
            movingUp = true;
        }
    }

    void Update()
    {
        Vector3 position = transform.position;

        if (movingUp)
        {
            position.y += speed * Time.deltaTime;
            if (position.y >= topLimit)
            {
                position.y = topLimit;
                movingUp = false;
            }
        }
        else
        {
            position.y -= speed * Time.deltaTime;
            if (position.y <= bottomLimit)
            {
                position.y = bottomLimit;
                movingUp = true;
            }
        }

        transform.position = position;
    }
}

