using UnityEngine;
using System.Collections;

public class movecam : MonoBehaviour
{
    // Speed in units per sec.
    public float speed;
    private Vector3 temploc;
    private Vector3 zeros;
    private float timeCount = 0.0f;

    // The target marker.
    //public Transform target;
    // target.transform.position = new Vector3(520f, 5f, 440f);
    private void Start()
    {
        //print("the current position is: " + transform.position);
        //print("the current localposition is: " + transform.localPosition);
        temploc = new Vector3(transform.localPosition.x + Random.Range(-3.0f, 3.0f), transform.localPosition.y + Random.Range(-3.0f, 3.0f), 0.0f);
        //print("the temploc is:" + temploc);
        zeros = new Vector3(0.0f, 0.0f, 0.0f);
    }
    void Update()
    {
		
        // The step size is equal to speed times frame time.
        float step = speed * Time.deltaTime;
        //temploc = new Vector3(transform.position.x + Random.Range(-10.0f, 10.0f), transform.position.y + Random.Range(-10.0f, 10.0f), transform.position.z + Random.Range(-10.0f, 10.0f));
        //print("moving toward: " + temploc);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, temploc, step);

        //if (Vector3.Distance(transform.position, temploc) < 0.001f)
        //{
        //   
        //}

        if (timeCount > 1.0f)
        {
            // Every one second generate a random local location for
            // the bee or zero out. 
            if (Vector3.Distance(transform.localPosition, zeros) < 0.001f) {
                //print("the current position is: " + transform.localPosition);
            temploc = new Vector3(transform.localPosition.x + Random.Range(-3.0f, 3.0f), transform.localPosition.y + Random.Range(-3.0f, 3.0f), 0.0f);
            //print("the temploc is:" + temploc);
            timeCount = 0.0f;
            }
            else
            {
            temploc = zeros;
            //print("the temploc is being zeroed out:" + temploc);
            }
         }

        timeCount = timeCount + Time.deltaTime;
    }
}