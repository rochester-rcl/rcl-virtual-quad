using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightOn : MonoBehaviour
{
    // Start is called before the first frame update
    public float sec = 5f;
    private Light myLight;
    void Start()
    {
        //if (gameObject.activeInHierarchy)
        //    gameObject.SetActive(false);
        myLight = GetComponent<Light>();
        StartCoroutine(LateCall());
    }

    IEnumerator LateCall()
    {

        yield return new WaitForSeconds(sec);
        myLight.enabled = true;
        //gameObject.SetActive(true);
        print("light should be on now...");
    }
}
