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

    //========== 캐릭터 정보 ==========//
    public float MoveSpeed; //캐릭터 이동속도
    public float JumpPower; //점프 힘

    float g_Acceleration; //중력 가속도
    float horizontal;   //수평
    float vertical; //수직
    bool isGround;    //캐릭터가 땅위에 서있는지 아닌지
    bool isJump;    //캐릭터가 점프 했는지

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
        horizontal = Input.GetAxis("Horizontal");

        //========== 좌우 이동 계산
        transform.position += new Vector3(horizontal, 0, 0) * (MoveSpeed * Time.deltaTime);

        //========== 오른쪽 이동
        if (horizontal > 0)
        {
            animater.SetInteger("AnimState", 1);
            spriteRenderer.flipX = false;
        }
        //========== 왼쪽 이동
        else if (horizontal < 0)
        {
            animater.SetInteger("AnimState", 1);
            spriteRenderer.flipX = true;
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

    IEnumerator Update_Coroutine()
    {

        while (true)
        {
            Run();
            Jump();

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
