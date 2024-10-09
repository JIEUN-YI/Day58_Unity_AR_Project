using UnityEngine;

public class MonsterController : MonoBehaviour
{
    ShooterController shooterController;

    [SerializeField] private GameObject monsterPrefab;
    private float creatTime = 3f;
    [SerializeField] private float curTime = 0;
    GameObject nowMonster;

    public float monsterHp;
    bool isMonster; // ������ ���� ���� �Ǵ�


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
            Debug.Log($"���� ü��(����) : {(int)monsterHp}");
        }
        else if (monsterHp <= 0)
        {
            isMonster = false;
            Destroy(nowMonster); // ���͸� ����
            curTime = 0; // ���� ���� ī��Ʈ 0
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
            Debug.Log($"���� ü��(����) : {(int)monsterHp}");
            isMonster = true;
        }
        else if (monsterHp <= 0)
        {
            isMonster = false;
            Destroy(nowMonster); // ���͸� ����
            curTime = 0; // ���� ���� ī��Ʈ 0
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
