using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFind : MonoBehaviour
{
    Enemy enemy;
    void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Player") )
        //{
        //    Debug.Log(collision.gameObject.name.ToString());
        //}   
        Debug.Log("1 + : " + collision.gameObject.name.ToString());
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("2 + : " + collision.gameObject.name.ToString());
    }
}
