using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//public enum 

public class JumpToDrop : MonoBehaviour
{
    Transform parentTransform;
    Vector2 collisionPoint_1;
    Vector2 collisionPoint_2;
    public DIRECTION direction;
    float radius;
    int count;
    Vector2 direction_1;
    Vector2 direction_2;
    Vector2 direction_3;

    public float AngleZ_1;
    public float AngleZ_2;
    public float AngleZ_3;

    public bool EnemyJump; //점프 가능 유무
    public bool EnemyDrop; //

    public bool LeftJump;
    public bool RightJump;

    bool collisionPointCheck;

    void Start()
    {
        collisionPoint_1 = new Vector2();
        collisionPoint_2 = new Vector2();
        parentTransform = transform.parent.GetComponent<Transform>();
        float scale = (parentTransform.localScale.x > parentTransform.localScale.y) ?
            parentTransform.localScale.x : parentTransform.localScale.y;

        radius = GetComponent<CircleCollider2D>().radius * scale;

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
        //if (collisionPoint_1 == Vector2.zero &&
        //    collisionPoint_2 == Vector2.zero)
        //    return;

        //float scale_1 = Vector2.Distance(collisionPoint_1 , (Vector2)transform.position );
        //float scale_2 = Vector2.Distance(collisionPoint_2 , (Vector2)transform.position );
        //if (scale_1 > radius)
        //    collisionPoint_1 = Vector2.zero;
        //if (scale_2 > radius)
        //    collisionPoint_2 = Vector2.zero;

        //if (direction == DIRECTION.LEFT)
        //{
        //    if (collisionPoint_1 != Vector2.zero)
        //    {
        //        float dot_1 = Vector2.Dot(direction_1, direction_2);
        //        float dot_2 = Vector2.Dot(direction_1, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //        dot_1 = Mathf.Acos(dot_1);
        //        dot_2 = Mathf.Acos(dot_2);
        //        dot_1 *= 180.0f / Mathf.PI;
        //        dot_2 *= 180.0f / Mathf.PI;

        //        float dot_3 = Vector2.Dot(direction_2, direction_3);
        //        float dot_4 = Vector2.Dot(direction_2, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //        dot_3 = Mathf.Acos(dot_3);
        //        dot_4 = Mathf.Acos(dot_4);
        //        dot_3 *= 180.0f / Mathf.PI;
        //        dot_4 *= 180.0f / Mathf.PI;

        //        Vector3 cross_1 = Vector3.Cross(direction_1, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //        Vector3 cross_2 = Vector3.Cross(direction_2, (collisionPoint_1 - (Vector2)transform.position).normalized);

        //        if (cross_1.z > 0.0f)
        //        {
        //            if (dot_1 > dot_2)
        //                EnemyJump = true;
        //        }

        //        if (cross_2.z > 0.0f)
        //        {
        //            if (dot_3 > dot_4)
        //                EnemyDrop = true;
        //        }
        //    }
        //    else if (collisionPoint_2 != Vector2.zero)
        //    {
        //        float dot_1 = Vector2.Dot(direction_1, direction_2);
        //        float dot_2 = Vector2.Dot(direction_1, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //        dot_1 = Mathf.Acos(dot_1);
        //        dot_2 = Mathf.Acos(dot_2);
        //        dot_1 *= 180.0f / Mathf.PI;
        //        dot_2 *= 180.0f / Mathf.PI;

        //        float dot_3 = Vector2.Dot(direction_2, direction_3);
        //        float dot_4 = Vector2.Dot(direction_2, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //        dot_3 = Mathf.Acos(dot_3);
        //        dot_4 = Mathf.Acos(dot_4);
        //        dot_3 *= 180.0f / Mathf.PI;
        //        dot_4 *= 180.0f / Mathf.PI;

        //        Vector3 cross_1 = Vector3.Cross(direction_1, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //        Vector3 cross_2 = Vector3.Cross(direction_2, (collisionPoint_2 - (Vector2)transform.position).normalized);

        //        if (cross_1.z > 0.0f)
        //        {
        //            if (dot_1 > dot_2)
        //                EnemyJump = true;
        //        }

        //        if (cross_2.z > 0.0f)
        //        {
        //            if (dot_3 > dot_4)
        //                EnemyDrop = true;
        //        }
        //    }
        //}
        //else if (direction == DIRECTION.RIGHT)
        //{
        //    if (collisionPoint_1 != Vector2.zero)
        //    {
        //        float dot_1 = Vector2.Dot(direction_1, direction_2);
        //        float dot_2 = Vector2.Dot(direction_1, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //        dot_1 = Mathf.Acos(dot_1);
        //        dot_2 = Mathf.Acos(dot_2);
        //        dot_1 *= 180.0f / Mathf.PI;
        //        dot_2 *= 180.0f / Mathf.PI;

        //        float dot_3 = Vector2.Dot(direction_2, direction_3);
        //        float dot_4 = Vector2.Dot(direction_2, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //        dot_3 = Mathf.Acos(dot_3);
        //        dot_4 = Mathf.Acos(dot_4);
        //        dot_3 *= 180.0f / Mathf.PI;
        //        dot_4 *= 180.0f / Mathf.PI;

        //        Vector3 cross_1 = Vector3.Cross(direction_1, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //        Vector3 cross_2 = Vector3.Cross(direction_2, (collisionPoint_1 - (Vector2)transform.position).normalized);

        //        if (cross_1.z < 0.0f)
        //        {
        //            if (dot_1 > dot_2)
        //                EnemyJump = true;
        //        }
        //        //
        //        if (cross_2.z < 0.0f)
        //        {
        //            if (dot_3 > dot_4)
        //                EnemyDrop = true;
        //        }
        //    }
        //    else if (collisionPoint_2 != Vector2.zero)
        //    {
        //        float dot_1 = Vector2.Dot(direction_1, direction_2);
        //        float dot_2 = Vector2.Dot(direction_1, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //        dot_1 = Mathf.Acos(dot_1);
        //        dot_2 = Mathf.Acos(dot_2);
        //        dot_1 *= 180.0f / Mathf.PI;
        //        dot_2 *= 180.0f / Mathf.PI;

        //        float dot_3 = Vector2.Dot(direction_2, direction_3);
        //        float dot_4 = Vector2.Dot(direction_2, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //        dot_3 = Mathf.Acos(dot_3);
        //        dot_4 = Mathf.Acos(dot_4);
        //        dot_3 *= 180.0f / Mathf.PI;
        //        dot_4 *= 180.0f / Mathf.PI;

        //        Vector3 cross_1 = Vector3.Cross(direction_1, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //        Vector3 cross_2 = Vector3.Cross(direction_2, (collisionPoint_2 - (Vector2)transform.position).normalized);

        //        if (cross_1.z < 0.0f)
        //        {
        //            if (dot_1 > dot_2)
        //                EnemyJump = true;
        //        }

        //        if (cross_2.z < 0.0f)
        //        {
        //            if (dot_3 > dot_4)
        //                EnemyDrop = true;
        //        }
        //    }

        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<BoxCollider2D>().name != collision.gameObject.name &&
            LayerMask.NameToLayer("Ground") == collision.gameObject.layer)
        {
            if (collision.gameObject.name == "Ground_Lowest")
            {
                EnemyDrop = true;
                return;
            }

            //if (!collisionPointCheck)
            //{
            //    collisionPoint_1 = collision.ClosestPoint(transform.position);
            //    collisionPointCheck = true;
            //}
            //else
            //{
            //    collisionPoint_2 = collision.ClosestPoint(transform.position);
            //    collisionPointCheck = false;

            //}
        }
    }
    private void OnDrawGizmos()
    {
        //if (collisionPoint_1 != Vector2.zero)
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawLine(transform.position, collisionPoint_1);
        //}
        //if (collisionPoint_2 != Vector2.zero)
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(transform.position, collisionPoint_2);
        //}
        ////
        //bool point_U = false;
        //bool point_D = false;
        //float dot_1 = 0.0f;
        //float dot_2 = 0.0f;
        //float dot_3 = 0.0f;
        //float dot_4 = 0.0f;
        //Vector3 cross_1 = Vector3.zero;
        //Vector3 cross_2 = Vector3.zero;

        //if (collisionPoint_1 != Vector2.zero)
        //{
        //    dot_1 = Vector2.Dot(direction_1, direction_2);
        //    dot_2 = Vector2.Dot(direction_1, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //    dot_1 = Mathf.Acos(dot_1);
        //    dot_2 = Mathf.Acos(dot_2);
        //    dot_1 *= 180.0f / Mathf.PI;
        //    dot_2 *= 180.0f / Mathf.PI;
        //    cross_1 = Vector3.Cross(direction_1, (collisionPoint_1 - (Vector2)transform.position).normalized);

        //    if (direction == DIRECTION.LEFT)
        //    {
        //        if (cross_1.z > 0.0f && dot_1 > dot_2)
        //            point_U = true;
        //    }
        //    else if (direction == DIRECTION.RIGHT)
        //    {
        //        if (cross_1.z < 0.0f && dot_1 > dot_2)
        //            point_U = true;
        //    }

        //    dot_3 = Vector2.Dot(direction_2, direction_3);
        //    dot_4 = Vector2.Dot(direction_2, (collisionPoint_1 - (Vector2)transform.position).normalized);
        //    dot_3 = Mathf.Acos(dot_3);
        //    dot_4 = Mathf.Acos(dot_4);
        //    dot_3 *= 180.0f / Mathf.PI;
        //    dot_4 *= 180.0f / Mathf.PI;

        //    cross_2 = Vector3.Cross(direction_2, (collisionPoint_1 - (Vector2)transform.position).normalized);

        //    if (direction == DIRECTION.LEFT)
        //    {
        //        if (cross_2.z > 0.0f && dot_3 > dot_4)
        //            point_D = true;
        //    }
        //    else if (direction == DIRECTION.RIGHT)
        //    {
        //        if (cross_2.z < 0.0f && dot_3 > dot_4)
        //            point_D = true;
        //    }
        //}
        //if (collisionPoint_2 != Vector2.zero)
        //{
        //    dot_1 = Vector2.Dot(direction_1, direction_2);
        //    dot_2 = Vector2.Dot(direction_1, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //    dot_1 = Mathf.Acos(dot_1);
        //    dot_2 = Mathf.Acos(dot_2);
        //    dot_1 *= 180.0f / Mathf.PI;
        //    dot_2 *= 180.0f / Mathf.PI;
        //    cross_1 = Vector3.Cross(direction_1, (collisionPoint_2 - (Vector2)transform.position).normalized);

        //    if (direction == DIRECTION.LEFT)
        //    {
        //        if (cross_1.z > 0.0f && dot_1 > dot_2)
        //            point_U = true;
        //    }
        //    else if (direction == DIRECTION.RIGHT)
        //    {
        //        if (cross_1.z < 0.0f && dot_1 > dot_2)
        //            point_U = true;
        //    }

        //    dot_3 = Vector2.Dot(direction_2, direction_3);
        //    dot_4 = Vector2.Dot(direction_2, (collisionPoint_2 - (Vector2)transform.position).normalized);
        //    dot_3 = Mathf.Acos(dot_3);
        //    dot_4 = Mathf.Acos(dot_4);
        //    dot_3 *= 180.0f / Mathf.PI;
        //    dot_4 *= 180.0f / Mathf.PI;
        //    //
        //    cross_2 = Vector3.Cross(direction_2, (collisionPoint_2 - (Vector2)transform.position).normalized);

        //    if (direction == DIRECTION.LEFT)
        //    {
        //        if (cross_2.z > 0.0f && dot_3 > dot_4)
        //            point_D = true;
        //    }
        //    else if (direction == DIRECTION.RIGHT)
        //    {
        //        if (cross_2.z < 0.0f && dot_3 > dot_4)
        //            point_D = true;
        //    }
        //}

        //if (point_U && point_D)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_1 * radius));
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_2 * radius));
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_3 * radius));
        //    Gizmos.color = Color.white;
        //}
        //else if (point_U)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_1 * radius));
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_2 * radius));
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_3 * radius));
        //}
        //else if (point_D)
        //{
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_1 * radius));
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_2 * radius));
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_3 * radius));
        //}
        //else
        //{
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_1 * radius));
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_2 * radius));
        //    Gizmos.DrawLine(transform.position, (transform.position + (Vector3)direction_3 * radius));
        //}


        
        //Handles.DrawSolidArc(transform.position, Vector3.forward, direction_1, AngleZ_2 - AngleZ_1, radius);
        //Handles.DrawSolidArc(transform.position, Vector3.forward, direction_2, AngleZ_3 - AngleZ_2, radius);
    }

    //
}
