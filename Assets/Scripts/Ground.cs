using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [HideInInspector] public MainCharacter Player;
    public bool IsLowestGround;

    void Start()
    {
        //node = new AStarNode[3];
        //box = GetComponent<BoxCollider2D>();
        //node[0] = new AStarNode();
        //node[1] = new AStarNode();
        //node[2] = new AStarNode();
        //node[0].Position = new Vector2(box.bounds.min.x , box.bounds.max.y);
        //node[1].Position = new Vector2(transform.position.x , box.bounds.max.y);
        //node[2].Position = new Vector2(box.bounds.max.x , box.bounds.max.y);
        //node[0].G = node[1].G = node[2].G = 0;
        //node[0].H = node[1].H = node[2].H = 0;
        //node[0].F = node[1].F = node[2].F = 0;
        //node[0].Check = node[1].Check = node[2].Check = false;
        
    }

    void Update()
    {
        
    }
    public void DateUpdate(Enemy _enemy)
    {
        
    }
    private void OnDrawGizmos()
    {
        //if (node !=null)
        //{
        //    Gizmos.DrawWireSphere(node[0].Position, 0.5f);
        //    Gizmos.DrawWireSphere(node[1].Position, 0.5f);
        //    Gizmos.DrawWireSphere(node[2].Position, 0.5f);
        //}
        
        //Gizmos.DrawWireSphere(leftEndPoint, 1.0f);
        //Gizmos.DrawWireSphere(rightEndPoint, 1.0f);
    }

}
