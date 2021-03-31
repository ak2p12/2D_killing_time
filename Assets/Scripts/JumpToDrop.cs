using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//public enum 

public class JumpToDrop : MonoBehaviour
{
    Vector2 collisionPoint;
    public DIRECTION direction;
    //float radius;

    Vector2 direction_1;
    Vector2 direction_2;
    Vector2 direction_3;

    public float AngleZ_1;
    public float AngleZ_2;
    public float AngleZ_3;

    public bool EnemyJump; //점프 가능 유무
    public bool EnemyDrop; //


    void Start()
    {
        collisionPoint = Vector2.zero;
        //radius = GetComponent<CircleCollider2D>().radius * 9.425f;

        Quaternion angle = Quaternion.Euler(0, 0, AngleZ_1);
        direction_1 = (angle * Vector3.up).normalized;

        angle = Quaternion.Euler(0, 0, AngleZ_2);
        direction_2 = (angle * Vector3.up).normalized;

        angle = Quaternion.Euler(0, 0, AngleZ_3);
        direction_3 = (angle * Vector3.up).normalized;

    }

    // Update is called once per frame
    void Update()
    {
        if (direction == DIRECTION.LEFT)
        {
            if (collisionPoint != Vector2.zero)
            {
                float dot_1 = Vector2.Dot(direction_1, direction_2);
                float dot_2 = Vector2.Dot(direction_1, (collisionPoint - (Vector2)transform.position).normalized);
                dot_1 = Mathf.Acos(dot_1);
                dot_2 = Mathf.Acos(dot_2);
                dot_1 *= 180.0f / Mathf.PI;
                dot_2 *= 180.0f / Mathf.PI;

                float dot_3 = Vector2.Dot(direction_2, direction_3);
                float dot_4 = Vector2.Dot(direction_2, (collisionPoint - (Vector2)transform.position).normalized);
                dot_3 = Mathf.Acos(dot_3);
                dot_4 = Mathf.Acos(dot_4);
                dot_3 *= 180.0f / Mathf.PI;
                dot_4 *= 180.0f / Mathf.PI;

                Vector3 cross_1 = Vector3.Cross(direction_1, (collisionPoint - (Vector2)transform.position).normalized);
                Vector3 cross_2 = Vector3.Cross(direction_2, (collisionPoint - (Vector2)transform.position).normalized);

                if (cross_1.z > 0.0f)
                {
                    if (dot_1 > dot_2)
                        EnemyJump = true;
                    else
                        EnemyJump = false;
                }

                if (cross_2.z > 0.0f)
                {
                    if (dot_3 > dot_4)
                        EnemyDrop = true;
                    else
                        EnemyDrop = false;
                }
            }
        }
        else if (direction == DIRECTION.RIGHT)
        {
            if (collisionPoint != Vector2.zero)
            {
                float dot_1 = Vector2.Dot(direction_1, direction_2);
                float dot_2 = Vector2.Dot(direction_1, (collisionPoint - (Vector2)transform.position).normalized );
                dot_1 = Mathf.Acos(dot_1);
                dot_2 = Mathf.Acos(dot_2);
                dot_1 *= 180.0f / Mathf.PI;
                dot_2 *= 180.0f / Mathf.PI;

                float dot_3 = Vector2.Dot(direction_2, direction_3);
                float dot_4 = Vector2.Dot(direction_2, (collisionPoint - (Vector2)transform.position).normalized);
                dot_3 = Mathf.Acos(dot_3);
                dot_4 = Mathf.Acos(dot_4);
                dot_3 *= 180.0f / Mathf.PI;
                dot_4 *= 180.0f / Mathf.PI;

                Vector3 cross_1 = Vector3.Cross(direction_1, (collisionPoint - (Vector2)transform.position).normalized);
                Vector3 cross_2 = Vector3.Cross(direction_2, (collisionPoint - (Vector2)transform.position).normalized);

                    if (cross_1.z < 0.0f)
                    {
                        if (dot_1 > dot_2)
                            EnemyJump = true;
                        else
                            EnemyJump = false;
                    }

                    if (cross_2.z < 0.0f)
                    {
                        if (dot_3 > dot_4)
                            EnemyDrop = true;
                        else
                            EnemyDrop = false;
                    }
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<BoxCollider2D>().name != collision.gameObject.name &&
            LayerMask.NameToLayer("Ground") == collision.gameObject.layer)
        {
            collisionPoint = collision.ClosestPoint(transform.position);
        }

    }
    private void OnDrawGizmos()
    {
        //Vector3 dir_1 = direction_1;
        //Vector3 dir_2 = direction_2;
        //Vector3 dir_3 = direction_3;

        //Gizmos.color = Color.white;
        //if (collisionPoint != Vector2.zero)
        //    Gizmos.DrawLine(transform.position , new Vector3(collisionPoint.x, collisionPoint.y , 0));

        //float a1 = Vector2.Dot(direction_1, direction_2);
        //float a2 = Vector2.Dot(direction_1, (collisionPoint - new Vector2(transform.position.x, transform.position.y)).normalized);
        //a1 = Mathf.Acos(a1); // 90
        //a2 = Mathf.Acos(a2);

        //a1 *= 180.0f / Mathf.PI;
        //a2 *= 180.0f / Mathf.PI;
        //Vector3 temp = Vector3.Cross(direction_1, (collisionPoint - new Vector2(transform.position.x, transform.position.y)).normalized);

        //if (direction == DIRECTION.RIGHT)
        //{
        //    if (temp.z < 0.0f)
        //    {
        //        if (a1 > a2)
        //            Handles.color = Color.red;
        //        else
        //            Handles.color = Color.green;
        //    }
        //    else
        //        Handles.color = Color.green;
        //}
        //else
        //{
        //    if (temp.z > 0.0f)
        //    {
        //        if (a1 > a2)
        //            Handles.color = Color.red;
        //        else
        //            Handles.color = Color.green;
        //    }
        //    else
        //        Handles.color = Color.green;
        //}
        
           

        

        //Gizmos.DrawLine(transform.position, (transform.position + dir_1 * radius) );
        //Gizmos.DrawLine(transform.position, (transform.position + dir_2 * radius) );
        //Gizmos.DrawLine(transform.position, (transform.position + dir_3 * radius) );

        //Handles.DrawSolidArc(transform.position, Vector3.forward, direction_1, AngleZ_2 - AngleZ_1, radius);
        //Handles.DrawSolidArc(transform.position, Vector3.forward, direction_2, AngleZ_3 - AngleZ_2, radius);

        
        
    }

    //
}
