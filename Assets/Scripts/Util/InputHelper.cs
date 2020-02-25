using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHelper
{

    Hashtable mButtonTimeout;

    public InputHelper()
    {
        mButtonTimeout = new Hashtable();
    }

    /**
     * Trigger logic once per press
     */
    public bool OnKeyPressed(KeyCode key)
    {
        if (Input.GetKey(key))
        {
            if (!mButtonTimeout.ContainsKey(key))
            {
                mButtonTimeout.Add(key, true);
                return true;
            }
        }
        else
            if (mButtonTimeout.ContainsKey(key))
                mButtonTimeout.Remove(key);
        return false;
    }
}
