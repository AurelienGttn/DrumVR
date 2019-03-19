using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingMenuItem : MonoBehaviour
{
    public Camera m_camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2 * transform.position - m_camera.transform.position);
    }
}
