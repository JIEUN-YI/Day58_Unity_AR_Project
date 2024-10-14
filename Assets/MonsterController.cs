using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MonsterController : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager; // AR Plane Manager�� ���

    [Header("Monster")]
    [SerializeField] private GameObject monsterPrefab;
    GameObject nowMonster;

    public float maxMonsterHp;
    public float curMonsterHp;
    public bool isMonster; // ������ ���� ���� �Ǵ�

    [SerializeField] LayerMask PlaneMask; // ���� �ν��ϵ��� �ϴ� ���̾� ����ũ ������ ���� 
    [SerializeField] Transform MainCamPoint; // �÷��̾��� �ü� ���� ī�޶�

    [Header("Raycast")]
    [SerializeField] Transform monsterPoint; // �ٴ����� �߻��� ����ĳ��Ʈ�� ������
    private float range = 5f;

    /* �⺻ ���� ��¹� 
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
            yield return new WaitForSeconds(3f);
            isMonster = false;
        }
    */
    /* �ٴ��ν��� ���� ���*/

    private void Awake()
    {
        isMonster = false;
    }

    private void Update()
    {
        Debug.Log("���ӽ���");
        if (SelectPlane() != null)
        {
            if (isMonster == false) // ���Ͱ� ������
            {
                MonsterCreate(); // ���� ����
            }
            else if (isMonster == true) // ���Ͱ� ������
            {
                if (curMonsterHp <= 0) // ü���� 0 ������ ��
                {
                    StartCoroutine(MonsterDied()); // ���� �ڷ�ƾ ����
                }
                if(nowMonster.transform.position.y < -10)
                {
                    StartCoroutine(MonsterDied());
                }
            }
        }
    }
    private void MonsterCreate()
    {
        RaycastHit hit;
        ARPlane plane = SelectPlane(); // �ٴ� �ϳ��� �������� ����
        Vector3 center = plane.center; // �ٴ��� ��� ����
        Vector2 size = plane.size; // �ٴ��� �簢�� ������ (x, y) ������
        Vector3 monsterPos; // ���Ͱ� ��µ� ��ġ

        // ������ ��ġ�� �������� ����
        monsterPos
            = new Vector3(center.x + Random.Range(-size.x * 0.4f, size.x * 0.4f), // �ٴ��� ������ �ȿ��� �������� 
                          center.y,
                          center.z + Random.Range(-size.y * 0.4f, size.y * 0.4f));
        // 1. �ٴ��� ���� ��쿡�� �Լ� ���� => �ٽ� �ٴ��� Ž��
        // ������ ��ġ���� ���� ��ġ����, �Ʒ���������, �浹ü��, ���������ȿ�, PlaneMask���̾����� �ִ� �浹ü Ȯ��
        if (Physics.Raycast(monsterPos + Vector3.up, Vector3.down, out hit, range, PlaneMask) == false)
        {
            return;
        }
        // 2. �ٴ��� ��ġ�� �� �ڿ� �ִ� ��� �Լ� ���� => �ٽ� �ٴ��� Ž��
        // �÷��̾��� �þ� ��ġ����, ������ ������ġ���� ���� ���� ��ġ��, PlaneMask���̾ �ش��ϴ� �浹Ȯ��
        if (Physics.Linecast(MainCamPoint.transform.position, monsterPos + Vector3.up * 0.1f, PlaneMask))
        {
            return;
        }
        CreateMonster(monsterPos);
        isMonster = true; // ���ʹ� �ϳ��� �����ǹǷ� ����

    }

    private ARPlane SelectPlane()
    {
        List<ARPlane> horizontalPlane = new List<ARPlane>(); // ARPlane�� ������ ����Ʈ ����
        // PlanManager���� �ٴڸ��� Plane�� ���� ����Ʈ�� ����
        foreach (ARPlane plane in planeManager.trackables)
        {
            // ���� Plane�� �� ������ Horizontal���� �ִ� ���
            if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                horizontalPlane.Add(plane); // ����Ʈ�� �߰�
            }
        }
        if (horizontalPlane.Count == 0)
        {
            return null;
        }
        // ����Ʈ ���� plane�� �����ϰ� ��ȯ
        return horizontalPlane[Random.Range(0, horizontalPlane.Count)];
    }

    private void CreateMonster(Vector3 monsterPos)
    {
        nowMonster = Instantiate(monsterPrefab, monsterPos, Quaternion.LookRotation(MainCamPoint.transform.position));
        isMonster = true;
    }

    /// <summary>
    /// ������ ������ ǥ���ϴ� �ڷ�ƾ
    /// ���� �� ���� ���������� �ð��� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonsterDied()
    {
        Destroy(nowMonster);
        curMonsterHp = maxMonsterHp;
        yield return new WaitForSeconds(5f); // ���ο� ���� ���������� �ð��� ����
        isMonster = false;
    }

}
