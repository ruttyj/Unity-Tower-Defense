using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Copy serialized fields
    public virtual void Copy(Status status){
        throw new Exception("Must be overriten!");
    }

    /**
     * Method called when the status gets applied to a target
     */
    public virtual void Apply(GameObject target, GameObject projectile) {
        throw new Exception("Must be overriten!");
    }

    /**
     * Method called when target already has the effect active and gets reapplied the status
     */
    public virtual void Refresh() {
        throw new Exception("Must be overriten!");
    }

}
