using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    //========== 캐릭터 정보 ==========//
    public float JumpSpeed; //점프 속도
    public float FindRadius; //인식 범위 (반지름)
    public float AttackRange; //공격 거리
    public float DestroyTime; //소멸 시간
    float actionTime; //생성 후 행동 시간
    float currentActionTime; //생성 후 행동 시간
    bool isStart; //행동시작 
    private int movingRandomCount;
    private float movingTimeCount;

    
    [HideInInspector] public Unit target;

    //============== AI ==============//
    BehaviorTree bt; //메인루프
    Sequence sequene_1; //하나라도 false 면 false 반환
    Selecter selecter_1; //하나라도 true 면 true 반환
    Condition_IsDead condition_IsDead; //사망 유무 검사 
    Condition_NotFind condition_NotFind; //타겟 발견 
    Condition_TargetDistance condition_TargetDistance; //타겟과의 거리
    Action_Dead action_Dead; //사망 행동 실행
    Action_Roaming action_Roaming; //적 찾기
    Action_Trace action_Trace; //추적

    
    [HideInInspector] public Dijkstra dijkstra; //다익스트라
    
    //


    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rigid;

    [HideInInspector] public bool IsRun; //움직이고 있는지
    [HideInInspector] public bool IsFind;
    [HideInInspector] public bool IsJump;
    [HideInInspector] public bool IsAttack;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        dijkstra = GameObject.Find("Grounds").GetComponent<Dijkstra>();


        currentActionTime = actionTime = 0.3f;
        bt = new BehaviorTree();
        sequene_1 = new Sequence();
        selecter_1 = new Selecter();
        action_Roaming = new Action_Roaming();

        selecter_1.AddNode(action_Roaming);
        sequene_1.AddNode(selecter_1);
        bt.Init(sequene_1);

        StartCoroutine(Update_Coroutine());
    }

    IEnumerator Update_Coroutine()
    {
        while (true)
        {
            if (isStart)
            {
                if (bt.Result(this))
                {
                    Debug.Log("AI 종료");
                }
                else
                {
                    //IsRun = true;
                    //rigid.AddForce(Vector3.left * MoveSpeed, ForceMode2D.Force);
                    //animator.SetBool("Run", IsRun);
                    //spriteRenderer.flipX = true;
                }
            }
            else
            {
                currentActionTime -= Time.deltaTime;
                if (currentActionTime <= 0)
                {
                    isStart = true;
                    currentActionTime = actionTime;
                    Debug.Log("AI 시작");
                }    
            }
            
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundInfo = collision.gameObject.GetComponent<Ground>();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public override bool Hit(float _damege)
    {
        CurrentHP -= _damege;
        if (CurrentHP <= 0.0f)
            CurrentHP = 0.0f;
        return true;
    }

    private void OnDrawGizmos()
    {
        if (IsFind)
        {
            spriteRenderer.color = Color.red;
        }
    }
}

//사망 판별
public class Condition_IsDead : Condition
{
    public override bool ChackCondition(Enemy _enemy)
    {
        if (_enemy.IsDead)
            return true;

        return false;
    }

    public override bool Result(Enemy _enemy)
    {
        if (ChackCondition(_enemy))
        {
            if (childNode != null && childNode.Result(_enemy))
            {
                return true;
            }
        }
        return false;
    }

    public override void SetNode(Node _node)
    {
        childNode = _node;
    }
}
//타켓 발견 못했을때
public class Condition_NotFind : Condition
{
    public override bool ChackCondition(Enemy _enemy)
    {
        if (!_enemy.IsFind)
            return true;

        return false;
    }

    public override bool Result(Enemy _enemy)
    {
        if (ChackCondition(_enemy))
        {
            if (childNode != null && childNode.Result(_enemy))
            {
                return true;
            }
        }
        return false;
    }

    public override void SetNode(Node _node)
    {
        childNode = _node;
    }
}

public class Condition_TargetDistance : Condition
{
    public override bool ChackCondition(Enemy _enemy)
    {
        float dist = Vector3.Distance(_enemy.transform.position, _enemy.target.transform.position);
        if (dist > 2.0f)
            return true;

        return false;
    }

    public override bool Result(Enemy _enemy)
    {
        if (ChackCondition(_enemy))
        {
            if (childNode != null && childNode.Result(_enemy))
            {
                return true;
            }
        }
        return false;
    }

    public override void SetNode(Node _node)
    {
        childNode = _node;
    }
}
//시간 
public class TimeDelay : TimeOut
{
    public override bool ChackCondition(Enemy _enemy)
    {
        if (!isStart)
        {
            originTime = Time.time;
            isStart = true;
            return false;
        }

        currentTime += Time.time - originTime;
        originTime = Time.time;

        if (currentTime >= timeDelay)
            return true;

        return false;
    }

    public override bool Result(Enemy _enemy)
    {
        if (ChackCondition(_enemy))
        {
            if (childNode != null && childNode.Result(_enemy))
            {
                return true;
            }
        }
        return false;
    }

    public override void SetNode(Node _node)
    {
        childNode = _node;
    }

    public override void SetTime(float _time)
    {
        isStart = false;
        timeDelay = _time;
        currentTime = 0.0f;
        originTime = 0.0f;
    }
}
//적을 찾는 행위
public class Action_Roaming : ActionNode
{
    bool isArrival;
    int targetIndex;
    bool jumpCheck;

    public override void OnStart(Enemy _enemy)
    {
        isStart = true;
        isArrival = true;
        jumpCheck = false;
        targetIndex = 9999;

        float shortDist = 9999.0f;
        int index = 9999;
        for (int i = 0; i < _enemy.dijkstra.Nodes.Length; ++i)
        {
            float dist = Vector2.Distance(_enemy.transform.position , _enemy.dijkstra.Nodes[i].Position);
            if (dist < shortDist)
            {
                shortDist = dist;
                index = i;
            }
        }
        int random = Random.Range(0 , _enemy.dijkstra.Nodes.Length);
        //_enemy.dijkstra.StartDijkstra(index , random);
        _enemy.dijkstra.StartDijkstra(0 , 6);
        
    }
    public override bool OnUpdate(Enemy _enemy)
    {
        if (isArrival)
        {
            
            targetIndex = _enemy.dijkstra.FinalIndex_Stack.Pop();
            isArrival = false;
           
                
            Debug.Log(targetIndex.ToString());
        }
        else
        {
            //Vector3 center = (sunrise.position + sunset.position) * 0.5F;
            //center -= new Vector3(0, 1, 0);
            //Vector3 riseRelCenter = sunrise.position - center;
            //Vector3 setRelCenter = sunset.position - center;
            //float fracComplete = (Time.time - startTime) / journeyTime;
            //transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            //transform.position += center;


            Vector2 nodePosition = _enemy.dijkstra.Nodes[targetIndex].Position;
            Vector2 enemyPosition = _enemy.transform.position;
            Vector2 dist = nodePosition - enemyPosition;
            _enemy.IsRun = true;

            _enemy.transform.position += (Vector3)dist.normalized * (_enemy.MoveSpeed * Time.deltaTime);
            _enemy.animator.SetBool("Run" , _enemy.IsRun);

            if (dist.normalized.x < 0.0f)
                _enemy.spriteRenderer.flipX = true;
            else
                _enemy.spriteRenderer.flipX = false;
            

            float distance = Vector2.Distance(nodePosition , enemyPosition);

            if (distance < 0.2f)
            {
                if (_enemy.dijkstra.Nodes[targetIndex].IsJump == true)
                    jumpCheck = true;
                    
                isArrival = true;
            }

            if (jumpCheck == true)
            {
                _enemy.rigid.AddForce((Vector3.up + (Vector3)dist.normalized).normalized * _enemy.JumpPower, ForceMode2D.Impulse);
                jumpCheck = false;
            }

            //Debug.Log(dist.ToString());
            // Debug.Log(dist.normalized.ToString());
        }
        
        return false;
    }
    public override bool OnEnd(Enemy _enemy)
    {
        isStart = false;
        return true;
    }
    public override bool Result(Enemy _enemy)
    {
        if (!isStart)
            OnStart(_enemy);

        if (OnUpdate(_enemy))
        {
            return OnEnd(_enemy);
        }

        return false;
    }
}
//사망
public class Action_Dead : ActionNode
{
    float originTime;
    float currentTime;
    public override void OnStart(Enemy _enemy)
    {
        isStart = true;
        currentTime = _enemy.DestroyTime;
        _enemy.animator.SetTrigger("Dead");
        originTime = Time.time;
    }
    public override bool OnUpdate(Enemy _enemy)
    {
        //이전 프레임시간 에서 현재프레임시간 까지 걸린 시간을 계산
        float time = Time.time - originTime;

        //걸린시간을 현재시간에 더한다
        currentTime -= time;

        //현재 프레임 시간을 예전 프레임 시간으로 대입
        originTime = Time.time;
        _enemy.spriteRenderer.color = new Color(1, 1, 1, currentTime);

        if (currentTime <= 0.0f)
            return true;

        return false;
    }
    public override bool OnEnd(Enemy _enemy)
    {
        _enemy.spriteRenderer.color = new Color(1, 1, 1, 0);
        originTime = 0.0f;
        currentTime = 0.0f;
        isStart = false;
        return true;
    }
    public override bool Result(Enemy _enemy)
    {
        if (!isStart)
            OnStart(_enemy);

        if (OnUpdate(_enemy))
        {
            OnEnd(_enemy);
            return true;
        }

        return false;
    }
}

public class Action_Trace : ActionNode
{
    public override void OnStart(Enemy _enemy)
    {
        isStart = true;
    }
    public override bool OnUpdate(Enemy _enemy)
    {
        return false;
    }
    public override bool OnEnd(Enemy _enemy)
    {
        isStart = false;
        return true;
    }
    public override bool Result(Enemy _enemy)
    {
        if (!isStart)
            OnStart(_enemy);

        if (OnUpdate(_enemy))
        {
            OnEnd(_enemy);
            return true;
        }

        return false;
    }
}


