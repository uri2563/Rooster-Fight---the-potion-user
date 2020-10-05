using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom : MonoBehaviour
{

    public Camera showCam;
    public Slider slider;
    private float basicField;

    // Start is called before the first frame update
    private void Awake()
    {
        basicField = showCam.fieldOfView;
    }
    void Start()
    {
        transform.LookAt(new Vector3(0,2.5f,0));
    }

    // Update is called once per frame
    void Update()
    {
        if (showCam.isActiveAndEnabled)
        {
            showCam.fieldOfView = basicField - slider.value;
        }
    }
}
