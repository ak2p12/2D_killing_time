using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [HideInInspector] public MainCharacter Player;
    [HideInInspector] public JumpToDrop leftJumpToDrop;
    [HideInInspector] public JumpToDrop rightJumpToDrop;
    [HideInInspector] public float leftPointDistance;
    [HideInInspector] public float rightPointDistance;

    [HideInInspector] public JumpToDrop[] jumpBox;

    BoxCollider2D box;

    public bool IsLowestGround;
    
    Vector3 point;
    [HideInInspector] public float pointDistance;
    [HideInInspector] public int count;

    Vector3 leftEndPoint;
    Vector3 rightEndPoint;
        
    void Start()
    {
        pointDistance = 9999;
        if (IsLowestGround)
        {
            JumpToDrop[] com = GetComponentsInChildren<JumpToDrop>();
            jumpBox = new JumpToDrop[com.Length];
            for (int i = 0; i < com.Length; ++i)
                jumpBox[i] = com[i];
        }
        else
        {
            Component[] com = GetComponentsInChildren<JumpToDrop>();
            foreach (Component _com in com)
            {
                if (_com.gameObject.name == "Left")
                    leftJumpToDrop = _com.GetComponent<JumpToDrop>();
                else if (_com.gameObject.name == "Right")
                    rightJumpToDrop = _com.GetComponent<JumpToDrop>();
            }
            
        }

        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
    }
    public void DateUpdate(Enemy _enemy)
    {
        pointDistance = 9999;
        //Ground 의 왼쪽끝 과 오른쪽끝을 _enemy.y 값으로 맞춤
        leftEndPoint.Set(box.bounds.min.x, _enemy.transform.position.y, 0.0f);
        rightEndPoint.Set(box.bounds.max.x, _enemy.transform.position.y, 0.0f);

        //Ground 의 왼쪽끝 과 오른쪽끝을 _enemy 와 거리 계산
        leftPointDistance = Vector3.Distance(leftEndPoint, _enemy.transform.position);
        rightPointDistance = Vector3.Distance(rightEndPoint, _enemy.transform.position);

        ////타겟을 못 찾았다면
        //if (_enemy.target == null)
        //    return;

        if (IsLowestGround)
        {
            for (int i = 0; i < jumpBox.Length; ++i)
            {
                point.Set(jumpBox[i].transform.position.x, _enemy.transform.position.y, 0.0f);
                float dist = Vector3.Distance(point, _enemy.transform.position);
                if (dist < pointDistance)
                {
                    pointDistance = dist;
                    count = i;
                }
            }
            
        }
        else
        {
            if (_enemy.target == null)
                return;
        }
            
        
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(leftEndPoint, 1.0f);
        //Gizmos.DrawWireSphere(rightEndPoint, 1.0f);
    }

}
