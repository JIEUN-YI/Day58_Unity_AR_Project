using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private GameObject monsterPrefab;
    private float creatTime = 3f;
    [SerializeField] private float curTime = 0;
    GameObject nowMonster;

    public float maxMonsterHp;
    public float curMonsterHp;
    public bool isMonster; // 몬스터의 생성 여부 판단

    [Header("Raycast")]
    [SerializeField] Transform monsterPoint; // 바닥으로 발사할 레이캐스트의 시작점
    private float range = 50f;

    private void Awake()
    {
        isMonster = false;
    }

    private void Update()
    {
        Debug.DrawRay(monsterPoint.position, Vector3.down * range, Color.red);
        Ray ray = new Ray(monsterPoint.position, Vector3.down * range); // 몬스터 포인트에서 바닥으로 레이캐스트 발사
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            if (isMonster != true)
            {
                CreateMonster();
                curMonsterHp = maxMonsterHp;
                isMonster = true;
            }
        }
        if(isMonster)
        {
            if(curMonsterHp <= 0)
            {
                Debug.Log("삭제");
                StartCoroutine(MonsterDied());
            }
        }
    }

    private void CreateMonster()
    {
        nowMonster = Instantiate(monsterPrefab, monsterPoint.transform.position, monsterPoint.transform.rotation);
        isMonster = true;
    }

    /// <summary>
    /// 몬스터의 삭제를 표현하는 코루틴
    /// 삭제 후 새로 생성까지의 시간을 조절
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonsterDied() 
    {
        Debug.Log("코루틴");
        Destroy(nowMonster);
        curMonsterHp = maxMonsterHp;
        yield return new WaitForSeconds(5f);
        isMonster = false;
    }
}
