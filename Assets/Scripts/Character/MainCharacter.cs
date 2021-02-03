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

    //========== 캐릭터 정보 ==========//
    public float MoveSpeed; //캐릭터 이동속도
    float horizontal;
    float vertical;

    SpriteRenderer spriteRenderer;
    Animator animater;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animater = GetComponent<Animator>();
        StartCoroutine(Update_Coroutine());
    }

    IEnumerator Update_Coroutine()
    {
        
        while (true)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Horizontal");

            transform.Translate(Vector3.right * (horizontal * MoveSpeed) * Time.deltaTime, Space.World);

            //=====오른쪽 이동
            if (horizontal > 0)
            {
                animater.SetInteger("AnimState", 1);
                spriteRenderer.flipX = false;
            }

            //=====왼쪽 이동
            else if (horizontal < 0)
            {
                animater.SetInteger("AnimState", 1);
                spriteRenderer.flipX = true;
            }
            else
            {
                animater.SetInteger("AnimState", 0);
            }

            if (Input.GetKey(Key_Up))
            {
                //transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(Key_Down))
            {
                //transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime, Space.World);
            }

            //animater.SetInteger("AnimState" , 0);
            yield return null;
        }
    }
}
