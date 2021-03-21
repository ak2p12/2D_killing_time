using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Unit
{
    //========== 입력 ==========//
    public KeyCode Key_Left;
    public KeyCode Key_Right;
    public KeyCode Key_Up;
    public KeyCode Key_Down;
    public KeyCode Key_Jump;
    public KeyCode Key_Attack;
    public KeyCode Key_Dodge;

    //========== 캐릭터 정보 ==========//
    public float MoveSpeed; //캐릭터 이동속도
    public float DodgeSpeed; //캐릭터 구르기 속도
    public float JumpPower; //점프 힘
    public float AttackSpeed; //공격속도
    public float MeleeDamage; //공격력
    public float MaxHP; //최대 체력
    public float CurrentHP; //현재 체력
    public float MaxSP; //최대 지구력
    public float CurrentSP; //현재 지구력

    float horizontal;   //수평
    float vertical; //수직

    bool leftorRight; //현재 캐릭터 방향이 왼쪽인지 오른쪽인지 구별 전용
    bool isFirstAttack; //첫번째 공격 모션과 두번째 공격모션 구별전용

    bool isGround;    //땅 위에서 있는지
    bool isJump;    //점프 했는지
    bool isAttack;  //공격 중인지
    bool isDodge; //회피 중인지
    bool isLanding; //떨어지는 중인지


    SpriteRenderer spriteRenderer;
    Animator animater;
    Rigidbody2D rigid;

    public MeleeAttack MeleeAttackBox;   //근접공격 충돌객체

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animater = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        MeleeAttackBox.gameObject.SetActive(false);
        StartCoroutine(Update_Coroutine());
    }

    void Run()
    {
        //========== 좌우 이동 ==========//
        if (Input.GetKey(Key_Left) && !isAttack && !isDodge)
        {
            horizontal = Input.GetAxis("Horizontal");
            transform.position += new Vector3(horizontal, 0, 0) * (MoveSpeed * Time.deltaTime);
            animater.SetInteger("AnimState", 1);
            leftorRight = spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(Key_Right) && !isAttack && !isDodge)
        {
            horizontal = Input.GetAxis("Horizontal");
            transform.position += new Vector3(horizontal, 0, 0) * (MoveSpeed * Time.deltaTime);
            animater.SetInteger("AnimState", 1);
            leftorRight = spriteRenderer.flipX = false;
        }
        else
            animater.SetInteger("AnimState", 0);
        //===================================//
    }
    void Jump()
    {
        //========== 점프 ==========//
        if ((Input.GetKeyDown(Key_Up) || Input.GetKeyDown(Key_Jump)) && !isJump && !isDodge)
        {
            animater.SetTrigger("Jump");
            rigid.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
            isGround = false;
            isJump = true;

            Attack_End();
        }

        animater.SetFloat("G_Acceleration", rigid.velocity.y);

        if (rigid.velocity.y < 0.0f)
            isLanding = true;
        else
            isLanding = false;
        //==========================//
    }
    void Attack()
    {
        //========== 공격 ==========//
        if (Input.GetKeyDown(Key_Attack) && !isAttack && !isFirstAttack && !isJump && !isDodge)
        {
            MeleeAttack();
            animater.SetFloat("AttackSpeed", AttackSpeed);
            animater.SetTrigger("Attack_1");
            isAttack = true;
        }
        else if (Input.GetKeyDown(Key_Attack) && !isAttack && isFirstAttack && !isJump && !isDodge)
        {
            MeleeAttack();
            animater.SetFloat("AttackSpeed", AttackSpeed);
            animater.SetTrigger("Attack_2");
            isAttack = true;
        }
        //==========================//
    }
    void Action()
    {
        //구르기
        if (Input.GetKeyDown(Key_Dodge) && !isDodge && isGround)
        {
            isDodge = true;
            if (Input.GetKey(Key_Left))
                leftorRight = spriteRenderer.flipX = true;
            else if (Input.GetKey(Key_Right))
                leftorRight = spriteRenderer.flipX = false;

            animater.SetTrigger("Dodge");

        }

        //구르기 중
        if (isDodge)
        {
            //왼쪽 구르기
            if (leftorRight)
                transform.position += new Vector3(-1, 0, 0) * ( DodgeSpeed * Time.deltaTime);
            //오른쪽 구르기
            else
                transform.position += new Vector3(1, 0, 0) * ( DodgeSpeed * Time.deltaTime);
        }
    }
    void FirstAttack_End() //애니메이션에서 함수 호출
    {
        isFirstAttack = true;
        isAttack = false;
    }
    void SecondAttack_End() //애니메이션에서 함수 호출
    {
        isFirstAttack = false;
        isAttack = false;
    }
    void Attack_End() //애니메이션에서 함수 호출
    {
        isAttack = false;
        isFirstAttack = false;
    }
    void Dodge_End()//애니메이션에서 함수 호출
    {
        isDodge = false;
        isAttack = false;
        isFirstAttack = false;
    }

    IEnumerator Update_Coroutine()
    {

        while (true)
        {
            Run();
            Jump();
            Attack();
            Action();

            //Debug.Log(Vector3.Distance(Vector3.zero , transform.position));
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌 객체가 땅
        if ((collision.gameObject.layer == LayerMask.NameToLayer("NormalGround")))
        {
            isGround = true;

            if (isLanding)
                isJump = false;

            animater.SetBool("Ground", isGround);

            isAttack = false;
            isFirstAttack = false;
            isDodge = false;
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("NormalGround")))
            isGround = true;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //충돌한 객체가 땅이면서 땅과 충돌한 객체가 발인경우
        if ((collision.gameObject.layer == LayerMask.NameToLayer("NormalGround")))
        {
            isGround = false;
            animater.SetBool("Ground", isGround);
        }
    }

    public override bool Hit(float _damege)
    {
        return true;
    }

    private void MeleeAttack()
    {
        if (leftorRight)
            MeleeAttackBox.SetUp(new Vector2(transform.position.x - 1f, transform.position.y + 0.8f), MeleeDamage, "Enemy");
        else
            MeleeAttackBox.SetUp(new Vector2(transform.position.x + 1f, transform.position.y + 0.8f), MeleeDamage, "Enemy");
    }
}
