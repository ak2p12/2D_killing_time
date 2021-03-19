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

    //========== 캐릭터 정보 ==========//
    public float MoveSpeed; //캐릭터 이동속도
    public float JumpPower; //점프 힘
    public float AttackSpeed; //공격속도
    public float MeleeDamage; //공격력
    public float MaxHP; //최대 체력
    public float CurrentHP; //현재 체력

    float horizontal;   //수평
    float vertical; //수직

    bool leftorRight; //현재 캐릭터 방향이 왼쪽인지 오른쪽인지

    bool isGround;    //땅위에 서있는지 아닌지
    bool isJump;    //점프 했는지
    bool isFirstAttack;
    bool isAttack;  //공격중인지

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
        if (Input.GetKey(Key_Left) && !isAttack)
        {
            horizontal = Input.GetAxis("Horizontal");
            transform.position += new Vector3(horizontal, 0, 0) * (MoveSpeed * Time.deltaTime);
            animater.SetInteger("AnimState", 1);
            leftorRight = spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(Key_Right) && !isAttack)
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
        if ((Input.GetKeyDown(Key_Up) || Input.GetKeyDown(Key_Jump)) && false == isJump)
        {
            animater.SetTrigger("Jump");
            rigid.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
            isGround = false;
            isJump = true;
        }

        animater.SetFloat("G_Acceleration", rigid.velocity.y);
        //==========================//
    }

    void Attack()
    {
        //========== 공격 ==========//
        if (Input.GetKeyDown(Key_Attack) && !isAttack && !isFirstAttack && !isJump)
        {
            MeleeAttack();
            animater.SetFloat("AttackSpeed", AttackSpeed);
            animater.SetTrigger("Attack_1");
            isAttack = true;
        }
        else if (Input.GetKeyDown(Key_Attack) && !isAttack && isFirstAttack && !isJump)
        {
            MeleeAttack();
            animater.SetFloat("AttackSpeed", AttackSpeed);
            animater.SetTrigger("Attack_2");
            isAttack = true;
        }
        //==========================//
    }
    void FirstAttack() //애니메이션에서 함수 호출
    {
        isFirstAttack = true;
        isAttack = false;
    }

    void Attack_End() //애니메이션에서 함수 호출
    {
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
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌 객체가 땅
        if ((collision.gameObject.layer == LayerMask.NameToLayer("NormalGround")))
        {
            isGround = true;
            isJump = false;
            animater.SetBool("Ground", isGround);
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //충돌한 객체가 땅이면서 땅과 충돌한 객체가 발인경우
        if ((collision.gameObject.layer == LayerMask.NameToLayer("NormalGround")))
        {
            if (!isJump)
            {
                isGround = false;
                animater.SetBool("Ground", isGround);
            }
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
