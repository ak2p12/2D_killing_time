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

    SpriteRenderer spriteRenderer;
    Animator animator;

    public Animator EnemyAnimator
    { 
        get { return animator; }
    }
    public SpriteRenderer EnemyRenderer
    {
        get { return spriteRenderer; }
    }

    bool isRun; //움직이고 있는지
    bool isDead;

    public bool Dead
    {
        get { return isDead; }
    }

    BehaviorTree BT; //메인루프
    Sequence SQ_1; //하나라도 false 면 false 반환
    Action_Dead action_Dead;
    // Start is called before the first frame update
    void Start()
    {
        BT = new BehaviorTree();
        SQ_1 = new Sequence();

        action_Dead = new Action_Dead();

        SQ_1.AddNode(action_Dead);
        BT.Init(SQ_1);


        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        StartCoroutine(Update_Coroutine());
    }

    IEnumerator Update_Coroutine()
    {
        while (true)
        {   
            if (!BT.Result(this))
            {
                //AI 종료
            }
            //isRun = true;
            //animator.SetBool("Run" , isRun);
            //transform.position += Vector3.right * (MoveSpeed * Time.deltaTime);
            //spriteRenderer.flipX = false;
            //Debug.Log(spriteRenderer.color.linear.ToString());

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


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

//적을 찾는 행위
public class Action_Roaming : ActionNode
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
        _enemy.EnemyAnimator.SetTrigger("Dead");
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
        _enemy.EnemyRenderer.color = new Color(1,1,1, currentTime);

        if (currentTime <= 0.0f)
            return true;

        return false;
    }
    public override bool OnEnd(Enemy _enemy)
    {
        _enemy.EnemyRenderer.color = new Color(1, 1, 1, 0);
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

