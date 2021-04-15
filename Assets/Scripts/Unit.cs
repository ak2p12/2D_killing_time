using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour , IDamege
{
    [HideInInspector] public Ground groundInfo;

    public virtual bool Hit(float _damege)
    {
        Debug.Log("Unit Class : Hit();"); 
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
