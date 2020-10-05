using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Camera cam;

    private void Start()
    {
        //cam = Camera.main;
        cam = Camera.current;

    }

    void LateUpdate()
    {
        if (cam != null && cam.isActiveAndEnabled)
        {
            transform.LookAt(transform.position + cam.transform.forward);
        }
        else
        {
            cam = Camera.current;
        }
    }
}
