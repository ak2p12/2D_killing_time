using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //========== 캐릭터 정보 ==========//
    public float MoveSpeed; //캐릭터 이동속도
    public float JumpPower; //점프 힘

    float g_Acceleration; //중력 가속도
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Update_Coroutine());
    }

    IEnumerator Update_Coroutine()
    {
        while (true)
        {
            yield return null;
        }
    }
}
