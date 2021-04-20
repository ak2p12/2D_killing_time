using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    //========== 캐릭터 정보 ==========//
    public float JumpSpeed; //점프 속도
    public float FindRadius; //인식 범위 (반지름)
    public float AttackRange; //공격 거리

    public float MoveSpeed; //캐릭터 이동속도
    public float JumpPower; //점프 힘
    public float AttackSpeed; //공격속도
    public float MeleeDamage; //공격력
    public float MaxHP; //최대 체력
    public float CurrentHP; //현재 체력
    public float DestroyTime; //소멸 시간
    

    private int movingRandomCount;
    private float movingTimeCount;

    [HideInInspector] public bool isRun; //움직이고 있는지

    //============== AI ==============//
    [HideInInspector] public Unit target;
    [HideInInspector] public bool isLeftMoving; //왼쪽으로 추격
    [HideInInspector] public bool isRightMoving;//오른쪽으로 추격

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
        bt = new BehaviorTree();
        sequene_1 = new Sequence();
        selecter_1 = new Selecter();
        condition_NotFind = new Condition_NotFind();
        condition_IsDead = new Condition_IsDead();
        condition_TargetDistance = new Condition_TargetDistance();
        action_Dead = new Action_Dead();
        action_Roaming = new Action_Roaming();
        action_Trace = new Action_Trace();
        condition_NotFind.SetNode(action_Roaming);
        selecter_1.AddNode(condition_NotFind);
        condition_TargetDistance.SetNode(action_Trace);
        selecter_1.AddNode(condition_TargetDistance);
        //selecter_1.AddNode();
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
        }
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
        //적을 찾으면 종료
        if (_enemy.isFind)
        {
            return true;
        }

        currentTime += Time.time - originTime;
        originTime = Time.time;

        //지정된 시간이 되면 종료
        if (currentTime >= timeDelay)
        {
            isStart = false;
            return false;
        }

        //대기
        if (notMoving)
        {
            _enemy.isRun = false;
            _enemy.animator.SetBool("Run", _enemy.isRun);
            return false;
        }

        //왼쪽
        else if (leftMoving)
        {
            //점프할 때 이동속도 증가
            if (_enemy.isJump)
                _enemy.transform.position += Vector3.left * ((_enemy.MoveSpeed * _enemy.JumpSpeed) * Time.deltaTime);
            else
                _enemy.transform.position += Vector3.left * (_enemy.MoveSpeed * Time.deltaTime);

            //왼쪽으로 이동
            _enemy.animator.SetBool("Run", _enemy.isRun);
            _enemy.spriteRenderer.flipX = true;

            //제일 밑에 있는 땅일 경우
            if (_enemy.groundInfo.IsLowestGround)
            {
                //점프박스와 거리 계산
                for (int i = 0; i < _enemy.groundInfo.jumpBox.Length; ++i)
                {
                    Vector3 jumpBoxPos = new Vector3(
                        _enemy.groundInfo.jumpBox[i].transform.position.x,
                        _enemy.transform.position.y);

                    //점프박스와 거리가 가까울 경우
                    float dist = Vector3.Distance(jumpBoxPos, _enemy.transform.position);
                    if (dist <= 0.9f)
                    {
                        //랜덤으로 점프 할지 안할지 결정
                        int random = Random.Range(0, 2);

                        //점프할 경우
                        if (random == 1 && !_enemy.isJump)
                        {
                            if (_enemy.groundInfo.jumpBox[i].RightJump)
                            {
                                leftMoving = false;
                                rightMoving = true;
                            }
                            else if (_enemy.groundInfo.jumpBox[i].LeftJump)
                            {
                                leftMoving = true;
                                rightMoving = false;
                            }

                            timeDelay += 1.0f;
                            _enemy.isJump = true;
                            _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (_enemy.groundInfo.leftPointDistance <= 0.9f)
                {
                    if (_enemy.groundInfo.leftJumpToDrop.EnemyJump &&
                       _enemy.groundInfo.leftJumpToDrop.EnemyDrop &&
                       !_enemy.isJump)
                    {
                        int randomJumpToDrop = Random.Range(0, 2);
                        //0 이면 점프
                        if (randomJumpToDrop == 0 && !_enemy.isJump)
                        {
                            timeDelay += 1.0f;
                            _enemy.isJump = true;
                            _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                        }
                        else
                        {
                            timeDelay += 1.0f;
                        }
                    }
                    else if (_enemy.groundInfo.leftJumpToDrop.EnemyJump && !_enemy.isJump)
                    {
                        timeDelay += 1.0f;
                        _enemy.isJump = true;
                        _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                    }
                    else if (!_enemy.groundInfo.leftJumpToDrop.EnemyDrop && !_enemy.isJump)
                    {
                        timeDelay += 1.0f;
                        leftMoving = false;
                        rightMoving = true;
                    }

                    return false;
                }
            }
        }
        //오른쪽
        else if (rightMoving)
        {
            if (_enemy.isJump)
                _enemy.transform.position += Vector3.right * ((_enemy.MoveSpeed * _enemy.JumpSpeed) * Time.deltaTime);
            else
                _enemy.transform.position += Vector3.right * (_enemy.MoveSpeed * Time.deltaTime);

            _enemy.animator.SetBool("Run", _enemy.isRun);
            _enemy.spriteRenderer.flipX = false;

            if (_enemy.groundInfo.IsLowestGround)
            {
                //점프박스와 거리 계산
                for (int i = 0; i < _enemy.groundInfo.jumpBox.Length; ++i)
                {
                    Vector3 jumpBoxPos = new Vector3(
                        _enemy.groundInfo.jumpBox[i].transform.position.x,
                        _enemy.transform.position.y);

                    //점프박스와 거리가 가까울 경우
                    float dist = Vector3.Distance(jumpBoxPos, _enemy.transform.position);
                    if (dist <= 0.9f)
                    {
                        //랜덤으로 점프 할지 안할지 결정
                        int random = Random.Range(0, 2);

                        //점프할 경우
                        if (random == 1 && !_enemy.isJump)
                        {
                            timeDelay += 1.0f;
                            _enemy.isJump = true;
                            _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (_enemy.groundInfo.rightPointDistance <= 0.9f)
                {
                    if (_enemy.groundInfo.rightJumpToDrop.EnemyJump &&
                        _enemy.groundInfo.rightJumpToDrop.EnemyDrop &&
                        !_enemy.isJump)
                    {
                        int randomJumpToDrop = Random.Range(0, 2);
                        //0 이면 점프
                        if (randomJumpToDrop == 0 && !_enemy.isJump)
                        {
                            timeDelay += 1.0f;
                            _enemy.isJump = true;
                            _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                        }
                        else
                        {
                            timeDelay += 1.0f;
                        }
                    }
                    else if (_enemy.groundInfo.rightJumpToDrop.EnemyJump && !_enemy.isJump)
                    {
                        timeDelay += 1.0f;
                        _enemy.isJump = true;
                        _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                    }
                    else if (!_enemy.groundInfo.rightJumpToDrop.EnemyDrop && !_enemy.isJump)
                    {
                        timeDelay += 1.0f;
                        leftMoving = true;
                        rightMoving = false;
                    }

                    return false;
                }
            }
            //점프할 때 이동속도 증가

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
        if (_enemy.isLeftMoving)
        {
            if (_enemy.isJump)
                _enemy.transform.position += Vector3.left * ((_enemy.MoveSpeed * _enemy.JumpSpeed) * Time.deltaTime);
            else
                _enemy.transform.position += Vector3.left * (_enemy.MoveSpeed * Time.deltaTime);

            _enemy.isRun = true;
            _enemy.animator.SetBool("Run", _enemy.isRun);
            _enemy.spriteRenderer.flipX = true;
        }
        else if (_enemy.isRightMoving)
        {
            if (_enemy.isJump)
                _enemy.transform.position += Vector3.right * ((_enemy.MoveSpeed * _enemy.JumpSpeed) * Time.deltaTime);
            else
                _enemy.transform.position += Vector3.right * (_enemy.MoveSpeed * Time.deltaTime);

            _enemy.isRun = true;
            _enemy.animator.SetBool("Run", _enemy.isRun);
            _enemy.spriteRenderer.flipX = false;
        }

        //타겟과 같은지형에 있다면
        if (_enemy.groundInfo == _enemy.target.groundInfo)
        {
            //타겟이 오른쪽에 있다면
            if (_enemy.transform.position.x < _enemy.target.transform.position.x)
            {
                Debug.Log("같은지형 오른쪽 이동");
                _enemy.isLeftMoving = false;
                _enemy.isRightMoving = true;
            }
            //타겟이 왼쪽에 있다면
            else if (_enemy.transform.position.x > _enemy.target.transform.position.x)
            {
                Debug.Log("같은지형 왼쪽 이동");
                _enemy.isLeftMoving = true;
                _enemy.isRightMoving = false;
            }
        }
        ////다른 지형에 있다면
        else
        {
            //타겟과 다른 지형이면서 제일 밑에 있는 지형에 있다면
            if (_enemy.groundInfo.IsLowestGround)
            {
                //점프 박스가 오른쪽에 있다면
                if (_enemy.transform.position.x < _enemy.groundInfo.jumpBox[_enemy.groundInfo.count].transform.position.x && !_enemy.isJump)
                {
                    Debug.Log("다른지형 오른쪽 이동");
                    if ((_enemy.groundInfo.pointDistance < 3.0f))
                    {
                        Debug.Log("밑 지형 오른쪽 점프");
                        _enemy.isJump = true;
                        _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                    }

                    _enemy.isRightMoving = true;
                    _enemy.isLeftMoving = false;
                }
                //점프 박스가 왼쪽에 있다면
                else if (_enemy.transform.position.x > _enemy.groundInfo.jumpBox[_enemy.groundInfo.count].transform.position.x && !_enemy.isJump)
                {
                    Debug.Log("다른지형 왼쪽 이동");
                    if ((_enemy.groundInfo.pointDistance < 3.0f))
                    {
                        Debug.Log("밑 지형 왼쪽 점프");
                        _enemy.isJump = true;
                        _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                    }
                    _enemy.isRightMoving = false;
                    _enemy.isLeftMoving = true;
                }
            }
            //타겟과 다른 지형이라면
            else
            {
                //타겟이 더 높은곳에 있다면
                if (_enemy.transform.position.y < _enemy.target.transform.position.y && !_enemy.isJump)
                {
                    //타겟이 오른쪽에 있다면
                    if (_enemy.transform.position.x < _enemy.target.transform.position.x)
                    {
                        //현재 지형의 오른쪽 끝이 점프가 가능한 지형이라면
                        if (_enemy.groundInfo.rightJumpToDrop.EnemyJump)
                        {
                            if ((_enemy.groundInfo.rightPointDistance < 1.0f))
                            {
                                _enemy.isJump = true;
                                _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                            }
                            _enemy.isRightMoving = true;
                            _enemy.isLeftMoving = false;
                        }
                    }
                    //타겟이 왼쪽에 있다면
                    else if (_enemy.transform.position.x >= _enemy.target.transform.position.x)
                    {
                        //현재 지형의 왼쪽 끝이 점프가 가능한 지형이라면
                        if (_enemy.groundInfo.leftJumpToDrop.EnemyJump)
                        {
                            if ((_enemy.groundInfo.leftPointDistance < 1.0f))
                            {
                                _enemy.isJump = true;
                                _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
                            }
                            _enemy.isRightMoving = false;
                            _enemy.isLeftMoving = true;
                        }
                        else if (!_enemy.groundInfo.leftJumpToDrop.EnemyJump)
                        {
                            _enemy.isRightMoving = true;
                            _enemy.isLeftMoving = false;
                        }
                    }
                }
                //타겟이 더 낮은곳에 있다면
                else if (_enemy.transform.position.y >= _enemy.target.transform.position.y && !_enemy.isJump)
                {
                    //타겟이 오른쪽에 있다면
                    if (_enemy.transform.position.x < _enemy.target.transform.position.x)
                    {
                        //현재 지형의 오른쪽 끝이 점프가 가능한 지형이라면
                        if (_enemy.groundInfo.rightJumpToDrop.EnemyDrop)
                        {
                            _enemy.isRightMoving = true;
                            _enemy.isLeftMoving = false;
                        }
                    }
                    //타겟이 왼쪽에 있다면
                    else if (_enemy.transform.position.x >= _enemy.target.transform.position.x)
                    {
                        //현재 지형의 왼쪽 끝이 점프가 가능한 지형이라면
                        if (_enemy.groundInfo.leftJumpToDrop.EnemyDrop)
                        {
                            _enemy.isRightMoving = false;
                            _enemy.isLeftMoving = true;
                        }
                    }
                }
            }
            //        //타겟이 더 아래에 있다면
            //        else
            //        {
            //            //타겟이 오른쪽에 있다면
            //            if (_enemy.transform.position.x < _enemy.target.transform.position.x)
            //            {
            //                //현재 지형의 오른쪽 끝이 점프가 가능한 지형이라면
            //                if (_enemy.groundInfo.rightJumpToDrop.EnemyJump)
            //                {
            //                    if ((_enemy.groundInfo.rightPointDistance < 3.0f) && !_enemy.isJump)
            //                    {
            //                        _enemy.isJump = true;
            //                        _enemy.rigid.AddForce(new Vector2(0, _enemy.JumpPower), ForceMode2D.Impulse);
            //                    }

            //                    if (_enemy.isJump)
            //                        _enemy.transform.position += Vector3.right * ((_enemy.MoveSpeed * _enemy.JumpSpeed) * Time.deltaTime);
            //                    else
            //                        _enemy.transform.position += Vector3.right * (_enemy.MoveSpeed * Time.deltaTime);

            //                    _enemy.isRun = true;
            //                    _enemy.animator.SetBool("Run", _enemy.isRun);
            //                    _enemy.spriteRenderer.flipX = false;

            //                }
            //            }
            //        }
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
            OnEnd(_enemy);
            return true;
        }

        return false;
    }
}


