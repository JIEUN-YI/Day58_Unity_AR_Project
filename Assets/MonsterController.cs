using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    private float creatTime = 10f;
    [SerializeField] private float curTime = 0;

    private void OnEnable()
    {

    }
    private void Update()
    {
        if(curTime < creatTime)
        {
            curTime += Time.deltaTime;
        }
        else if(curTime > creatTime)
        {
            CreateMonster();
            curTime = 0;
        }
    }
    private void CreateMonster()
    {
        Instantiate(monsterPrefab, transform.position, transform.rotation);
    }
}
