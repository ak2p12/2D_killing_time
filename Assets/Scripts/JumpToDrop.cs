using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//public enum 

public class JumpToDrop : MonoBehaviour
{
    Vector2 collisionPoint;
    public DIRECTION direction;
    float radius;

    Vector2 upDiagonalDirection;
    Vector2 downDiagonalDirection;
    

    void Start()
    {
        collisionPoint = Vector2.zero;
        radius = GetComponent<CircleCollider2D>().radius * 9.425f;
        if (direction == DIRECTION.LEFT)
        {
            //upDiagonalDirection = new Vector2(-1,1);
            //downDiagonalDirection = new Vector2(-1,-1));
        }
        else if (direction == DIRECTION.RIGHT)
        {
            upDiagonalDirection = Vector3.Normalize(new Vector3(1, 1, 0));
            downDiagonalDirection = Vector3.Normalize(new Vector3(1, -1, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == DIRECTION.LEFT)
        {
            if (collisionPoint != Vector2.zero)
            {
               
                //Mathf.Cos
            }
        }//
        else if (direction == DIRECTION.RIGHT)
        {
            if (collisionPoint != Vector2.zero)
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y);
                float a1 = Vector2.Dot(Vector2.up , Vector2.right);
                float a2 = Vector2.Dot(Vector2.up, (collisionPoint - pos).normalized );
                a1 = Mathf.Acos(a1); // 90
                a2 = Mathf.Acos(a2);

                a1 *= 180.0f / Mathf.PI;
                a2 *= 180.0f / Mathf.PI;

                Debug.Log("a1 : " + a1.ToString());
                Debug.Log("a2 : " + a2.ToString());
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<BoxCollider2D>().name != collision.gameObject.name &&
            LayerMask.NameToLayer("NormalGround") == collision.gameObject.layer)
        {
            collisionPoint = collision.ClosestPoint(transform.position);
            //Debug.Log(collisionPoint.ToString());
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (GetComponentInParent<BoxCollider2D>().name != collision.gameObject.name &&
        //    LayerMask.NameToLayer("NormalGround") == collision.gameObject.layer)
        //{
        //    collisionPoint = collision.ClosestPoint(transform.position);
        //    //Debug.Log(collisionPoint.ToString());//
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (collisionPoint != Vector2.zero)
            Gizmos.DrawLine(transform.position , new Vector3(collisionPoint.x, collisionPoint.y , 0));

        float a1 = Vector2.Dot(Vector2.up, Vector2.right);
        float a2 = Vector2.Dot(Vector2.up, (collisionPoint - new Vector2(transform.position.x, transform.position.y)).normalized);
        a1 = Mathf.Acos(a1); // 90
        a2 = Mathf.Acos(a2);

        a1 *= 180.0f / Mathf.PI;
        a2 *= 180.0f / Mathf.PI;

        //Debug.Log("a1 : " + a1.ToString());
       //Debug.Log("a2 : " + a2.ToString());

        //Vector3 temp = new Vector3(collisionPoint.x, collisionPoint.y, 0) - transform.position;
        //float dot2 = Vector3.Dot(Vector3.right, temp.normalized) * (180 / Mathf.PI);

        if (a1 < a2)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * radius));
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * radius));
          
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * radius));
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * radius));
        }

        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * radius));
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * radius));
    }

    //
}
