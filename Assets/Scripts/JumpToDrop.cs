using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpToDrop : MonoBehaviour
{
    BoxCollider2D boxcollider;
    Vector3 boxColliderMax;
    Vector3 boxColliderMin;
    Vector3 boxColliderSize;

    Vector3 right;
    Vector3 rightDownDiagonal;
    Vector3 rightUpDiagonal;
    Vector3 rightUp;
    Vector3 rightDown;

    Vector3 left;
    Vector3 leftDownDiagonal;
    Vector3 leftUpDiagonal;
    Vector3 leftUp;
    Vector3 leftDown;

    RaycastHit2D rayHit;
    Ray2D ray;

    void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        boxColliderMax = boxcollider.bounds.max;
        boxColliderMin = boxcollider.bounds.min;
        boxColliderSize = boxcollider.bounds.size;

        right = new Vector3(boxColliderMax.x + 0.1f , boxColliderMax.y - (boxColliderSize.y * 0.5f), 0);
        rightDownDiagonal = new Vector3(boxColliderMax.x + 0.1f, boxColliderMin.y - 0.1f, 0);
        rightUpDiagonal = new Vector3(boxColliderMax.x + 0.1f, boxColliderMax.y + 0.1f, 0);
        rightUp = new Vector3(boxColliderMax.x - (boxColliderSize.x * 0.05f), boxColliderMax.y + 0.1f, 0);
        rightDown = new Vector3(boxColliderMax.x - (boxColliderSize.x * 0.05f), boxColliderMin.y - 0.1f, 0);

        left = new Vector3(boxColliderMin.x - 0.1f, boxColliderMax.y - (boxColliderSize.y * 0.5f), 0);
        leftDownDiagonal = new Vector3(boxColliderMin.x - 0.1f, boxColliderMin.y - 0.1f, 0);
        leftUpDiagonal = new Vector3(boxColliderMin.x - 0.1f, boxColliderMax.y + 0.1f, 0);
        leftUp = new Vector3(boxColliderMin.x + (boxColliderSize.x * 0.05f), boxColliderMax.y + 0.1f, 0);
        leftDown = new Vector3(boxColliderMin.x + (boxColliderSize.x * 0.05f), boxColliderMin.y - 0.1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp1 = new Vector3(GetComponent<BoxCollider2D>().bounds.max.x + 0.1f, GetComponent<BoxCollider2D>().bounds.min.y - 0.1f, 0.0f);
        rayHit = Physics2D.Raycast(temp1, Vector3.down , LayerMask.NameToLayer("NormalGround"));
        if (rayHit.collider != null)
        {
            //Debug.Log(transform.position.ToString());
            //Debug.Log(rayHit.collider.ToString());
        }

        Debug.Log(boxColliderMax.ToString());

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(right, 0.1f) ;
        Gizmos.DrawRay(right , Vector3.right * 2.0f);
        Gizmos.DrawWireSphere(rightDownDiagonal, 0.1f);
        Gizmos.DrawWireSphere(rightUpDiagonal, 0.1f);
        Gizmos.DrawWireSphere(rightUp, 0.1f);
        Gizmos.DrawWireSphere(rightDown, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(left, 0.1f);
        Gizmos.DrawWireSphere(leftDownDiagonal, 0.1f);
        Gizmos.DrawWireSphere(leftUpDiagonal, 0.1f);
        Gizmos.DrawWireSphere(leftUp, 0.1f);
        Gizmos.DrawWireSphere(leftDown, 0.1f);
    }
}
