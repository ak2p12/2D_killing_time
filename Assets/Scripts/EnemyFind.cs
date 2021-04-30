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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") )
        {
            enemy.target = collision.gameObject.GetComponent<Unit>();
            enemy.IsFind = true;
        }   
        
    }
}
