using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
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

    float horizontal;   //수평
    float vertical; //수직
    bool isGround;    //땅위에 서있는지 아닌지
    bool isJump;    //점프 했는지

    bool isFirstAttack;
    bool isSecondAttack;

    bool isAttack;  //공격중인지
    
    SpriteRenderer spriteRenderer;
    Animator animater;
    Rigidbody2D rigid;

    public BoxCollider2D HeadCollider;
    public BoxCollider2D FootCollider;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animater = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        HeadCollider.gameObject.SetActive(false);
        StartCoroutine(Update_Coroutine());
    }

    void Run()
    {
        //========== 좌우 이동 계산
        if (Input.GetKey(Key_Left) && !isAttack)
        {
            horizontal = Input.GetAxis("Horizontal");
            transform.position += new Vector3(horizontal, 0, 0) * (MoveSpeed * Time.deltaTime);
            animater.SetInteger("AnimState", 1);
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(Key_Right) && !isAttack)
        {
            horizontal = Input.GetAxis("Horizontal");
            transform.position += new Vector3(horizontal, 0, 0) * (MoveSpeed * Time.deltaTime);
            animater.SetInteger("AnimState", 1);
            spriteRenderer.flipX = false;
        }
        else
        {
            animater.SetInteger("AnimState", 0);
        }
    }

    void Jump()
    {
        //========== 점프
        if ((Input.GetKeyDown(Key_Up) || Input.GetKeyDown(Key_Jump)) && false == isJump)
        {
            //점프 할 때 머리 충돌 활성화 (발 충돌 비활성화)
            HeadCollider.gameObject.SetActive(true);
            FootCollider.gameObject.SetActive(false);

            animater.SetTrigger("Jump");
            rigid.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
            isGround = false;
            isJump = true;
        }

        //땅에 떨어질때 발 충돌 활성화 (머리 충돌 비활성화)
        if (rigid.velocity.y <= 0)
        {
            HeadCollider.gameObject.SetActive(false);
            FootCollider.gameObject.SetActive(true);
        }


        animater.SetFloat("G_Acceleration", rigid.velocity.y);
    }

    void Attack()
    {
        if (Input.GetKeyDown(Key_Attack) && !isAttack && !isFirstAttack && !isJump)
        {
            animater.SetTrigger("Attack_1");
            isAttack = true;
        }
        else if (Input.GetKeyDown(Key_Attack) && !isAttack && isFirstAttack && !isJump)
        {
            animater.SetTrigger("Attack_2");
            isAttack = true;
        }
    }
    void FirstAttack()
    {
        isFirstAttack = true;
        isAttack = false;
    }

    void Attack_End()
    {
        isAttack = false;
        isFirstAttack = false;
        Debug.Log("Attack End");
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
        //충돌한 객체가 기본 땅이면서 기본 땅과 충돌한 객체가 발인경우
        if ((collision.gameObject.layer == LayerMask.NameToLayer("NormalGround")) &&
            (collision.otherCollider.name == "Foot"))
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
        if ((collision.gameObject.layer == LayerMask.NameToLayer("NormalGround")) &&
           (collision.otherCollider.name == "Foot"))
        {
            if (!isJump)
            {
                isGround = false;
                animater.SetBool("Ground", isGround);
            }
        }
    }
}
