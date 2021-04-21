using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : Unit
{
    //========== 캐릭터 정보 ==========//
    public float DodgeSpeed; //캐릭터 구르기 속도
    public float UseDodgePoint; //구르기 소모량

    bool leftorRight; //현재 캐릭터 방향이 왼쪽인지 오른쪽인지 구별 전용
    bool isFirstAttack; //첫번째 공격 모션과 두번째 공격모션 구별전용

    bool isGround;    //땅 위에서 있는지
    bool isJump;    //점프 했는지
    bool isAttack;  //공격 중인지
    bool isDodge; //회피 중인지

    //========== 입력 ==========//
    public KeyCode Key_Left;
    public KeyCode Key_Right;
    public KeyCode Key_Up;
    public KeyCode Key_Down;
    public KeyCode Key_Jump;
    public KeyCode Key_Attack;
    public KeyCode Key_Dodge;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigid;

    public MeleeAttack MeleeAttackBox;   //근접공격 충돌객체

    //[HideInInspector] public Ground groundInfo;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        MeleeAttackBox.gameObject.SetActive(false);
        StartCoroutine(Update_Coroutine());
        StartCoroutine(Recovery_Coroutine());
    }
    void Run()
    {
        //========== 좌우 이동 ==========//
        if (Input.GetKey(Key_Left) && !isAttack && !isDodge)
        {
            if (Mathf.Abs(rigid.velocity.x) <= MoveSpeed)
                rigid.AddForce(Vector3.left * MoveSpeed,ForceMode2D.Force);
            
            if (isGround)
                animator.SetInteger("AnimState", 1);

            leftorRight = spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(Key_Right) && !isAttack && !isDodge)
        {
            if (Mathf.Abs(rigid.velocity.x) <= MoveSpeed)
                rigid.AddForce(Vector3.right * MoveSpeed, ForceMode2D.Force);

            if (isGround)
                animator.SetInteger("AnimState", 1);

            leftorRight = spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }    
        //===================================//
    }
    void Jump()
    {
        //========== 점프 ==========//
        if ((Input.GetKeyDown(Key_Up) || Input.GetKeyDown(Key_Jump)) && !isJump && !isDodge)
        {
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            isGround = false;
            isJump = true;
            animator.SetTrigger("Jump");
            animator.SetBool("Ground", isGround);
            animator.SetBool("JumpState" , isJump);
            //Attack_End();
        }

        animator.SetFloat("G_Acceleration", rigid.velocity.y);

        //if (rigid.velocity.y < 0.0f)
        //    isLanding = true;
        //else
        //    isLanding = false;
        //==========================//
    }
    void Attack()
    {
        //========== 공격 ==========//
        if (Input.GetKeyDown(Key_Attack) && !isAttack && !isFirstAttack && !isJump && !isDodge)
        {
            MeleeAttack();
            animator.SetFloat("AttackSpeed", AttackSpeed);
            animator.SetTrigger("Attack_1");
            isAttack = true;
        }
        else if (Input.GetKeyDown(Key_Attack) && !isAttack && isFirstAttack && !isJump && !isDodge)
        {
            MeleeAttack();
            animator.SetFloat("AttackSpeed", AttackSpeed);
            animator.SetTrigger("Attack_2");
            isAttack = true;
        }
        //==========================//
    }
    void Action()
    {
        //구르기
        if (Input.GetKeyDown(Key_Dodge) && !isDodge && isGround && (CurrentSP >= UseDodgePoint))
        {
            if (Input.GetKey(Key_Left))
                leftorRight = spriteRenderer.flipX = true;
            else if (Input.GetKey(Key_Right))
                leftorRight = spriteRenderer.flipX = false;

            rigid.velocity = new Vector2(0, rigid.velocity.y);

            isDodge = true;
            animator.SetTrigger("Dodge");
            animator.SetBool("DodgeState" , isDodge);

            CurrentSP -= UseDodgePoint;
            if (CurrentSP <= 0.0f)
                CurrentSP = 0.0f;

            //오른쪽 구르기
            if (!leftorRight)
                rigid.AddForce(Vector2.right * DodgeSpeed , ForceMode2D.Impulse);
            //왼쪽 구르기
            else if (leftorRight)
                rigid.AddForce(Vector2.left * DodgeSpeed, ForceMode2D.Impulse);
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
    public void Dodge_End()//애니메이션에서 함수 호출
    {
        isDodge = false;
        animator.SetBool("DodgeState", isDodge);
        isAttack = false;
        isFirstAttack = false;
    }
    IEnumerator Update_Coroutine()
    {

        while (true)
        {
            //====== 플레이어 행동 ======//
            Run();
            Jump();
            Attack();
            Action();
            //==========================//

            yield return null;
        }
    }
    IEnumerator Recovery_Coroutine()
    {
        while (true)
        {
            Recovery();
            yield return new WaitForSeconds(RecoveryCycle);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            groundInfo = collision.gameObject.GetComponent<Ground>();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            isJump = false;
            isGround = true;
            animator.SetBool("JumpState", isJump);
            animator.SetBool("Ground", isGround);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            isGround = false;
            animator.SetBool("Ground", isGround);
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
    private void Recovery()
    {
        if (CurrentHP <= MaxHP)
            CurrentHP += RecoveryHP;
        if (CurrentMP <= MaxMP)
            CurrentMP += RecoveryMP;
        if (CurrentSP <= MaxSP)
            CurrentSP += RecoverySP;
    }
}
