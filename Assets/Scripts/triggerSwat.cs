using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSwat : MonoBehaviour
{
    public Animator anim;
    public bool enter = true;

    void Awake()
    {
        
        anim.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        anim.enabled = true;
    }

}
