using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class parentmover_lookat : MonoBehaviour
{
    private Vector3 beestop;
    public float speed;
    public GameObject theTarget;
    private GameObject[] locations;
    private bool firsttime;
    private int i;
    private FullscreenFade fader;

    void Start()
    {
        locations = GameObject.FindGameObjectsWithTag("kf");
        firsttime = true;
        i = 0;
        //fader = gameObject.AddComponent<FullscreenFade>();
        //fader.triggerFadeIn();
 
    }

    IEnumerator DoPause()
    {
        yield return new WaitForSecondsRealtime(2);
       
        print("the location index is: " + i);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        //print("the step is: " + step);
       
        beestop = new Vector3(locations[i].transform.position.x, locations[i].transform.position.y, locations[i].transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, beestop, step);
        if (theTarget)
        {
            transform.LookAt(theTarget.transform.position);
        }
        else
        {
            var targetRotation = Quaternion.LookRotation(locations[i].transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step / 20);
        }
        

        if (Vector3.Distance(transform.position, beestop) < 0.001f)
        {
            //DoPause();
            if (i < locations.Length - 1) { 
                i++;
            }
            else{
                //fader.triggerFadeOut();
                i = 0;
             }
        }
      
    }
}
