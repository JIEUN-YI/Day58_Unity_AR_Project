using UnityEngine;

public class MonsterController : MonoBehaviour
{
    ShooterController shooterController;

    [SerializeField] private GameObject monsterPrefab;
    private float creatTime = 3f;
    [SerializeField] private float curTime = 0;
    GameObject nowMonster;

    public float monsterHp;
    bool isMonster; // 몬스터의 생성 여부 판단


    private void Awake()
    {
        shooterController = GameObject.Find("ShooterController").GetComponent<ShooterController>();
        isMonster = false;
    }

    /*
        private void Update()
    {  
    while (isMonster == false)
        {
            curTime += Time.deltaTime;
            if (curTime >= creatTime)
            {
                CreateMonster();
                curTime = 0;
                isMonster = true;
            }
        }
        monsterHp = shooterController.monsterHp;

        if (monsterHp > 0)
        {
            Debug.Log($"몬스터 체력(몬스터) : {(int)monsterHp}");
        }
        else if (monsterHp <= 0)
        {
            isMonster = false;
            Destroy(nowMonster); // 몬스터를 삭제
            curTime = 0; // 몬스터 스폰 카운트 0
        }
    }
     */
    private void Update()
    {
        curTime += Time.deltaTime;
        monsterHp = shooterController.monsterHp;

        if (curTime >= creatTime && isMonster == false)
        {
            CreateMonster();
            curTime = 0;
            isMonster = true;
        }
        if (monsterHp > 0 && isMonster == true)
        {
            Debug.Log($"몬스터 체력(몬스터) : {(int)monsterHp}");
            isMonster = true;
        }
        else if (monsterHp <= 0)
        {
            isMonster = false;
            Destroy(nowMonster); // 몬스터를 삭제
            curTime = 0; // 몬스터 스폰 카운트 0
        }

    }
    private void CreateMonster()
    {
        nowMonster = Instantiate(monsterPrefab, transform.position, transform.rotation);
    }

    /* private void Update()
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
     }*/
}
