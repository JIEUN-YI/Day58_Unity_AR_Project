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
    public bool isMonster; // ������ ���� ���� �Ǵ�

    [Header("Raycast")]
    [SerializeField] Transform monsterPoint; // �ٴ����� �߻��� ����ĳ��Ʈ�� ������
    private float range = 50f;

    private void Awake()
    {
        isMonster = false;
    }

    private void Update()
    {
        Debug.DrawRay(monsterPoint.position, Vector3.down * range, Color.red);
        Ray ray = new Ray(monsterPoint.position, Vector3.down * range); // ���� ����Ʈ���� �ٴ����� ����ĳ��Ʈ �߻�
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
                Debug.Log("����");
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
    /// ������ ������ ǥ���ϴ� �ڷ�ƾ
    /// ���� �� ���� ���������� �ð��� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonsterDied() 
    {
        Debug.Log("�ڷ�ƾ");
        Destroy(nowMonster);
        curMonsterHp = maxMonsterHp;
        yield return new WaitForSeconds(5f);
        isMonster = false;
    }
}
