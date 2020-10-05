using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGo : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);    
    }


}
