using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GAUGEBAR
{
    NULL,
    HP,
    MP,
    SP
}
public class GaugeBar : MonoBehaviour
{
    public GameObject TargetObject; //정보표시할 객체
    public GAUGEBAR GaugeBarInfo;   //정보 종류
    public float TargetPosY;    //표시할 객체 로부터 위치

    Slider gaugeBar;
    // Start is called before the first frame update
    void Start()
    {
        gaugeBar = GetComponent<Slider>();
        if (TargetObject.tag == "Player")
        {
            switch (GaugeBarInfo)
            {
                case GAUGEBAR.HP:
                    gaugeBar.maxValue = TargetObject.GetComponent<MainCharacter>().MaxHP;
                    gaugeBar.minValue = 0;
                    gaugeBar.value = TargetObject.GetComponent<MainCharacter>().CurrentHP;
                    break;
                case GAUGEBAR.MP:
                    gaugeBar.maxValue = TargetObject.GetComponent<MainCharacter>().MaxMP;
                    gaugeBar.minValue = 0;
                    gaugeBar.value = TargetObject.GetComponent<MainCharacter>().CurrentMP;
                    break;
                case GAUGEBAR.SP:
                    gaugeBar.maxValue = TargetObject.GetComponent<MainCharacter>().MaxSP;
                    gaugeBar.minValue = 0;
                    gaugeBar.value = TargetObject.GetComponent<MainCharacter>().CurrentSP;
                    break;
            }
        }
        else if (TargetObject.tag == "Enemy")
        {
            gaugeBar.maxValue = TargetObject.GetComponent<Enemy>().MaxHP;
            gaugeBar.minValue = 0;
            gaugeBar.value = TargetObject.GetComponent<Enemy>().CurrentHP;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gaugeBar.transform.position = new Vector3(
            TargetObject.transform.position.x ,
            TargetObject.transform.position.y + TargetPosY,
            TargetObject.transform.position.z);

        if (TargetObject.tag == "Player")
        {
            switch (GaugeBarInfo)
            {
                case GAUGEBAR.HP:
                    gaugeBar.maxValue = TargetObject.GetComponent<MainCharacter>().MaxHP;
                    gaugeBar.minValue = 0;
                    gaugeBar.value = TargetObject.GetComponent<MainCharacter>().CurrentHP;
                    break;
                case GAUGEBAR.MP:
                    gaugeBar.maxValue = TargetObject.GetComponent<MainCharacter>().MaxMP;
                    gaugeBar.minValue = 0;
                    gaugeBar.value = TargetObject.GetComponent<MainCharacter>().CurrentMP;
                    break;
                case GAUGEBAR.SP:
                    gaugeBar.maxValue = TargetObject.GetComponent<MainCharacter>().MaxSP;
                    gaugeBar.minValue = 0;
                    gaugeBar.value = TargetObject.GetComponent<MainCharacter>().CurrentSP;
                    break;
            }
        }
        else if (TargetObject.tag == "Enemy")
        {
            gaugeBar.maxValue = TargetObject.GetComponent<Enemy>().MaxHP;
            gaugeBar.minValue = 0;
            gaugeBar.value = TargetObject.GetComponent<Enemy>().CurrentHP;
        }
    }
}
