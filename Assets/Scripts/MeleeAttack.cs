using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    string targetLayerName;
    float survivalTime;
    float damage;

    public void SetUp(Vector2 _pos , float _damage,  string _targetLayerName)
    {
        transform.position = new Vector3(_pos.x, _pos.y, 0.0f);
        damage = _damage;
        targetLayerName = _targetLayerName;

        this.gameObject.SetActive(true);

        survivalTime = 0.08f;
        
        StartCoroutine(Update_Coroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName) &&
            (this.gameObject.activeSelf == true))
        {
            collision.gameObject.GetComponent<Unit>().Hit(damage);
        }
    }
    IEnumerator Update_Coroutine()
    {
        while (true)
        {
            survivalTime -= Time.deltaTime;
            if (survivalTime <= 0.0f)
            {
                StopCoroutine(Update_Coroutine());
                this.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
