using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    JumpToDrop leftJumpToDrop;
    JumpToDrop rightJumpToDrop;
    BoxCollider2D box;
    [HideInInspector] public MainCharacter Player;

    Vector3 leftEndPoint;
    Vector3 rightEndPoint;

        //nearest
    void Start()
    {
        Component[] com = GetComponentsInChildren<JumpToDrop>();
        foreach ( Component _com in com )
        {
            if (_com.gameObject.name == "Left")
                leftJumpToDrop = _com.GetComponent<JumpToDrop>();
            else if (_com.gameObject.name == "Right")
                rightJumpToDrop = _com.GetComponent<JumpToDrop>();
        }

        box = GetComponent<BoxCollider2D>();
        //leftEndPoint.Set(box.bounds.min.x , box.bounds.max.y , 0.0f);
        //rightEndPoint.Set(box.bounds.max.x , box.bounds.max.y , 0.0f);

    }

    void Update()
    {
        
    }
    public void DateUpdate(Enemy _enemy)
    {
        leftEndPoint.Set(box.bounds.min.x, _enemy.transform.position.y, 0.0f);
        rightEndPoint.Set(box.bounds.max.x, _enemy.transform.position.y, 0.0f);
        float leftdist = Vector3.Distance(leftEndPoint , _enemy.transform.position);
        float rightdist = Vector3.Distance(rightEndPoint, _enemy.transform.position);

        //타겟이 더 높은곳에 있다면
        if (_enemy.target.transform.position.y > _enemy.transform.position.y)
        {
            //왼쪽이 더 가깝다면
            if (leftdist < rightdist)
            {
                //점프가 가능하다면
                if (leftJumpToDrop.EnemyJump)
                {
                    _enemy.isTrace_Left = true;
                    _enemy.isTrace_Right = false;
                }
                else
                {
                    _enemy.isTrace_Left = false;
                    _enemy.isTrace_Right = true;
                }
                
            }
            //오른쪽이 더 가깝다면
            else
            {
                //점프가 가능하다면
                if (leftJumpToDrop.EnemyJump)
                {
                    _enemy.isTrace_Left = false;
                    _enemy.isTrace_Right = true;
                }
                else
                {
                    _enemy.isTrace_Left = true;
                    _enemy.isTrace_Right = false;
                }
            }
        }
        //타겟이 더 낮은곳에 있다면
        else
        {
            //왼쪽이 더 가깝다면
            if (leftdist < rightdist)
            {
                //떨어질 수 있다면
                if (leftJumpToDrop.EnemyDrop)
                {
                    _enemy.isTrace_Left = true;
                    _enemy.isTrace_Right = false;
                }
                else
                {
                    _enemy.isTrace_Left = false;
                    _enemy.isTrace_Right = true;
                }

            }
            //오른쪽이 더 가깝다면
            else
            {
                //떨어질 수 있다면
                if (leftJumpToDrop.EnemyDrop)
                {
                    _enemy.isTrace_Left = false;
                    _enemy.isTrace_Right = true;
                }
                else
                {
                    _enemy.isTrace_Left = true;
                    _enemy.isTrace_Right = false;
                }
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(leftEndPoint, 1.0f);
        //Gizmos.DrawWireSphere(rightEndPoint, 1.0f);
    }

}
