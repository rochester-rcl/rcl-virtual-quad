using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class camshake : MonoBehaviour
{
    public GameObject camparent;
    public float tilt;
    public Boundary boundary;
    private Quaternion quaternion;
    public float speed;
    private float timeCount = 0.0f;
    public float shakeStrength = 5;
    public float shake = 1;

    Vector3 originalPosition;

    void Start()
    {
        camparent = GameObject.FindGameObjectWithTag("ch");
        quaternion = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        originalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float step = speed * Time.deltaTime;
        shake = shakeStrength;
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        transform.localPosition = originalPosition + (Random.insideUnitSphere * shake);

        shake = Mathf.MoveTowards(shake, 0, Time.deltaTime * shakeStrength);

        if (shake == 0)
        {
            transform.localPosition = originalPosition;
        }

        if (timeCount > 2.0f)
        {
            // Every two seconds generate a random rotation for
            // the cube. The rotation is a quaternion.
            //quaternion = Random.rotation;
            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, -5.0f);
            //transform.rotation = quaternion;
            //transform.localPosition = originalPosition + (Random.insideUnitSphere * shake);
            //shake = Mathf.MoveTowards(shake, 0, Time.deltaTime * shakeStrength);
            // timeCount = 0.0f;
        }

        timeCount = timeCount + Time.deltaTime;

    }
}
