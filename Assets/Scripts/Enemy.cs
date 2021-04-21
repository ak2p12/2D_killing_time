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
    
    private int movingRandomCount;
    private float movingTimeCount;

    [HideInInspector] public bool isRun; //움직이고 있는지

    //============== AI ==============//
    [HideInInspector] public Unit target;

    BehaviorTree bt; //메인루프
    Sequence sequene_1; //하나라도 false 면 false 반환
    Selecter selecter_1; //하나라도 true 면 true 반환
    Condition_IsDead condition_IsDead; //사망 유무 검사 
    Condition_NotFind condition_NotFind; //타겟 발견 
    Condition_TargetDistance condition_TargetDistance; //타겟과의 거리
    Action_Dead action_Dead; //사망 행동 실행
    Action_Roaming action_Roaming; //적 찾기
    Action_Trace action_Trace; //추적


    [HideInInspector] public bool isRoaming; //적 순찰

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rigid;

    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isFind;
    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isAttack;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        //
        StartCoroutine(Update_Coroutine());
    }

    IEnumerator Update_Coroutine()
    {
        while (true)
        {
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
        if (isFind)
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
        if (_enemy.isDead)
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
        if (!_enemy.isFind)
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
    bool leftMoving;
    bool rightMoving;
    bool notMoving;
    bool notJumping;

    float currentTime;
    float originTime;
    float timeDelay;

    public override void OnStart(Enemy _enemy)
    {
        leftMoving = rightMoving = notMoving = notJumping = false;
        currentTime = originTime = 0.0f;
        _enemy.isJump = false;
        isStart = true;

        int random = Random.Range(0, 10);
        //왼쪽
        if (random >= 0 && random <= 2)
        {
            leftMoving = true;
            _enemy.isRun = true;
            timeDelay = Random.Range(3.0f, 5.0f);

        }
        //오른쪽
        else if (random >= 3 && random <= 5)
        {
            rightMoving = true;
            _enemy.isRun = true;
            timeDelay = Random.Range(3.0f, 5.0f);
        }
        //안움직임
        else
        {
            _enemy.isRun = false;
            notMoving = true;
            timeDelay = 1.0f;
        }

        originTime = Time.time;


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


