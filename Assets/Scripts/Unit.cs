using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour , IDamege
{
    [HideInInspector] public Ground groundInfo;
    public float MoveSpeed; //캐릭터 이동속도
    public float JumpPower; //점프 힘
    public float AttackSpeed; //공격속도
    public float MeleeDamage; //공격력
    public float MaxHP; //최대 체력
    public float CurrentHP; //현재 체력
    public float RecoveryHP; //체력회복력
    public float MaxMP; //최대 마력
    public float CurrentMP; //현재 마력
    public float RecoveryMP; //체력회복력
    public float MaxSP; //최대 지구력
    public float CurrentSP; //현재 지구력
    public float RecoverySP; //체력회복력
    public float RecoveryCycle; //회복속도
    public bool IsDead;

    public virtual bool Hit(float _damege)
    {
        Debug.Log("Unit Class : Hit();"); 
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
