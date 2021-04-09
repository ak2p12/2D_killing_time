using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    //========== 캐릭터 정보 ==========//
    public float MoveSpeed; //캐릭터 이동속도
    public float JumpPower; //점프 힘
    public float AttackSpeed; //공격속도
    public float MeleeDamage; //공격력
    public float MaxHP; //최대 체력
    public float CurrentHP; //현재 체력

    public float DestroyTime; //소멸 시간
    public float FindRadius; //인식 범위 (반지름)
    [HideInInspector] public bool isRun; //움직이고 있는지

    //============== AI ==============//
    [HideInInspector] public MainCharacter target;
    [HideInInspector] public bool isTrace_Left; //왼쪽으로 추격
    [HideInInspector] public bool isTrace_Right;//오른쪽으로 추격

    BehaviorTree bt; //메인루프
    Sequence sequene_1; //하나라도 false 면 false 반환
    Selecter selecter_1; //하나라도 true 면 true 반환
    Condition_IsDead condition_IsDead; //사망 유무 검사 
    Condition_NotFind condition_NotFind; //타겟 발견 
    Action_Dead action_Dead; //사망 행동 실행
    Action_Roaming action_Roaming;

    [HideInInspector] public bool isRoaming; //적 순찰


    [HideInInspector] public Ground groundInfo;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rigid;

    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isFind;
    [HideInInspector] public bool isJump;

    void Start()
    {
        bt = new BehaviorTree();
        sequene_1 = new Sequence();
        selecter_1 = new Selecter();
        condition_NotFind = new Condition_NotFind();
        condition_IsDead = new Condition_IsDead();
        action_Dead = new Action_Dead();
        action_Roaming = new Action_Roaming();

        condition_NotFind.SetNode(action_Roaming);
        selecter_1.AddNode(condition_NotFind);
        //condition_IsDead.SetNode(action_Dead);
        sequene_1.AddNode(selecter_1);
        //sequene_1.AddNode(condition_IsDead);
        bt.Init(sequene_1);


        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        StartCoroutine(Update_Coroutine());
    }

    IEnumerator Update_Coroutine()
    {
        while (true)
        {
            if (groundInfo != null)
            {
                groundInfo.DateUpdate(this);
                if (!bt.Result(this))
                {
                    //AI 종료
                }
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJump = false;
            groundInfo = collision.gameObject.GetComponent<Ground>();
            //if (isTrace_Left)
            //    Debug.Log("왼쪽추격");
            //else if (isTrace_Right)
            //    Debug.Log("오른쪽 추격");
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    public override bool Hit(float _damege)
    {
        CurrentHP -= _damege;
        if (CurrentHP <= 0.0f)
            CurrentHP = 0.0f;
        return true;
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
//타켓 
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
    bool LEFT_RIGHT;
    bool notMoveing;
    float currentTime;
    float originTime;
    float timeDelay;

    public override void OnStart(Enemy _enemy)
    {
        LEFT_RIGHT = notMoveing = false;

        currentTime = originTime = 0.0f;
        isStart = true;

        int random = Random.Range(0, 6);
        //왼쪽
        if (random >= 0 && random <= 2)
        {
            //Debug.Log("왼쪽으로 이동");
            
            LEFT_RIGHT = false;
            timeDelay = Random.Range(1.0f, 3.0f);
            _enemy.isRun = true;
        }
        //오른쪽
        else if (random >= 3 && random <= 5)
        {
            //Debug.Log("오른쪽으로 이동");
            LEFT_RIGHT = true;
            timeDelay = Random.Range(1.0f, 3.0f);
            _enemy.isRun = true;
        }
        //안움직임
        //else
        //{
        //    notMoveing = true;
        //    timeDelay = 1.0f;
        //    _enemy.isRun = false;
        //}
        originTime = Time.time;


    }
    public override bool OnUpdate(Enemy _enemy)
    {
        currentTime += Time.time - originTime;
        originTime = Time.time;

        if (currentTime >= timeDelay)
        {
            notMoveing = false;
            isStart = false;
            return false;
        }

        //대기z
        if (notMoveing)
            return false;
        //왼쪽
        else if (!LEFT_RIGHT)
        {
            _enemy.animator.SetBool("Run", _enemy.isRun);
            
            _enemy.spriteRenderer.flipX = true;

            //점프할 때 이동속도 증가
            if (_enemy.isJump)
                _enemy.transform.position += Vector3.left * ( (_enemy.MoveSpeed * 2.0f) * Time.deltaTime);
            else
                _enemy.transform.position += Vector3.left * ( _enemy.MoveSpeed * Time.deltaTime);

            if (_enemy.groundInfo.leftPointDistance <= 0.9f)
            {
                if (_enemy.groundInfo.leftJumpToDrop.EnemyJump && !_enemy.isJump)
                {
                    timeDelay += 2.0f;
                    _enemy.isJump = true;
                    _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                }
                else if (!_enemy.groundInfo.leftJumpToDrop.EnemyDrop && !_enemy.isJump)
                {
                    timeDelay += 2.0f;
                    LEFT_RIGHT = !LEFT_RIGHT;
                }

                return false;
            }
        }
        //오른쪽
        else if (LEFT_RIGHT)
        {
            _enemy.animator.SetBool("Run", _enemy.isRun);
            _enemy.spriteRenderer.flipX = false;

            if (_enemy.isJump)
                _enemy.transform.position += Vector3.right * ( (_enemy.MoveSpeed * 2.0f) * Time.deltaTime);
            else
                _enemy.transform.position += Vector3.right * ( _enemy.MoveSpeed * Time.deltaTime);

            //점프할 때 이동속도 증가
            if (_enemy.groundInfo.rightPointDistance <= 0.9f)
            {
                if (_enemy.groundInfo.rightJumpToDrop.EnemyJump && !_enemy.isJump)
                {
                    timeDelay += 2.0f;
                    _enemy.isJump = true;
                    _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                }
                else if (!_enemy.groundInfo.rightJumpToDrop.EnemyDrop && !_enemy.isJump)
                {
                    timeDelay += 2.0f;
                    LEFT_RIGHT = !LEFT_RIGHT;
                }

                return false;
            }
        }
        
        return false;
    }
    public override bool OnEnd(Enemy _enemy)
    {
        _enemy.isRun = false;
        _enemy.animator.SetBool("Run", _enemy.isRun);
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
        isStart = true;
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

