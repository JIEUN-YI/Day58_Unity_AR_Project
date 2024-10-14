using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MonsterController : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager; // AR Plane Manager를 사용

    [Header("Monster")]
    [SerializeField] private GameObject monsterPrefab;
    GameObject nowMonster;

    public float maxMonsterHp;
    public float curMonsterHp;
    public bool isMonster; // 몬스터의 생성 여부 판단

    [SerializeField] LayerMask PlaneMask; // 땅만 인식하도록 하는 레이어 마스크 범위를 설정 
    [SerializeField] Transform MainCamPoint; // 플레이어의 시선 메인 카메라

    [Header("Raycast")]
    [SerializeField] Transform monsterPoint; // 바닥으로 발사할 레이캐스트의 시작점
    private float range = 5f;

    /* 기본 몬스터 출력법 
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
            yield return new WaitForSeconds(3f);
            isMonster = false;
        }
    */
    /* 바닥인식을 통한 출력*/

    private void Awake()
    {
        isMonster = false;
    }

    private void Update()
    {
        Debug.Log("게임시작");
        if (SelectPlane() != null)
        {
            if (isMonster == false) // 몬스터가 없으면
            {
                MonsterCreate(); // 몬스터 생성
            }
            else if (isMonster == true) // 몬스터가 있으면
            {
                if (curMonsterHp <= 0) // 체력이 0 이하일 때
                {
                    StartCoroutine(MonsterDied()); // 삭제 코루틴 시작
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
        ARPlane plane = SelectPlane(); // 바닥 하나를 무작위로 선정
        Vector3 center = plane.center; // 바닥의 가운데 지점
        Vector2 size = plane.size; // 바닥의 사각형 사이즈 (x, y) 사이즈
        Vector3 monsterPos; // 몬스터가 출력될 위치

        // 몬스터의 위치를 랜덤으로 지정
        monsterPos
            = new Vector3(center.x + Random.Range(-size.x * 0.4f, size.x * 0.4f), // 바닥의 사이즈 안에서 랜덤으로 
                          center.y,
                          center.z + Random.Range(-size.y * 0.4f, size.y * 0.4f));
        // 1. 바닥이 없는 경우에는 함수 종료 => 다시 바닥을 탐색
        // 몬스터의 위치보다 높은 위치에서, 아래방향으로, 충돌체를, 지정범위안에, PlaneMask레이어층에 있는 충돌체 확인
        if (Physics.Raycast(monsterPos + Vector3.up, Vector3.down, out hit, range, PlaneMask) == false)
        {
            return;
        }
        // 2. 바닥의 위치가 벽 뒤에 있는 경우 함수 종료 => 다시 바닥을 탐색
        // 플레이어의 시야 위치에서, 몬스터의 지정위치보다 조금 위의 위치한, PlaneMask레이어에 해당하는 충돌확인
        if (Physics.Linecast(MainCamPoint.transform.position, monsterPos + Vector3.up * 0.1f, PlaneMask))
        {
            return;
        }
        CreateMonster(monsterPos);
        isMonster = true; // 몬스터는 하나만 생성되므로 제어

    }

    private ARPlane SelectPlane()
    {
        List<ARPlane> horizontalPlane = new List<ARPlane>(); // ARPlane을 가지는 리스트 생성
        // PlanManager에서 바닥면인 Plane을 전부 리스트에 포함
        foreach (ARPlane plane in planeManager.trackables)
        {
            // 만약 Plane의 선 종류가 Horizontal위의 있는 경우
            if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                horizontalPlane.Add(plane); // 리스트에 추가
            }
        }
        if (horizontalPlane.Count == 0)
        {
            return null;
        }
        // 리스트 안의 plane을 랜덤하게 반환
        return horizontalPlane[Random.Range(0, horizontalPlane.Count)];
    }

    private void CreateMonster(Vector3 monsterPos)
    {
        nowMonster = Instantiate(monsterPrefab, monsterPos, Quaternion.LookRotation(MainCamPoint.transform.position));
        isMonster = true;
    }

    /// <summary>
    /// 몬스터의 삭제를 표현하는 코루틴
    /// 삭제 후 새로 생성까지의 시간을 조절
    /// </summary>
    /// <returns></returns>
    private IEnumerator MonsterDied()
    {
        Destroy(nowMonster);
        curMonsterHp = maxMonsterHp;
        yield return new WaitForSeconds(5f); // 새로운 몬스터 생성까지의 시간을 제어
        isMonster = false;
    }

}
