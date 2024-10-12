using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    MonsterController monsterController;
    [SerializeField] Animator animator;

    [SerializeField] Transform fireCamPoint; // 레이저 시작 지점 = 카메라
    public float damange = 100;
    public float range = 50f; // 사정 거리
    [SerializeField] LayerMask layerMask; // 부딪힐 충돌체 레이어 설정
    [SerializeField] private GameObject impactEffect; // 적 피격 파티클

    [Header("Magzine")]
    [SerializeField] public int curMagzine; // 현재 탄창수
    [SerializeField] public int maxMagazine; // 최대 탄창수
    public bool isReload = false; // 재장전 판단여부

    [Header("Sound")]
    [SerializeField] AudioSource ShootingSound;
    [SerializeField] AudioSource ReloadSound;
    
    private void Awake()
    {
        monsterController = GameObject.Find("MonsterController").GetComponent<MonsterController>();
    }
    private void Start()
    {
        fireCamPoint = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        curMagzine = maxMagazine;

    }

    private void Update()
    {
        Debug.DrawRay(fireCamPoint.position, fireCamPoint.forward * range, Color.red); // 레이캐스트를 보여주는 함수
    }

    public void Shoot()
    {
        Ray ray = new Ray(fireCamPoint.position, fireCamPoint.forward * range);
        RaycastHit hit;
        curMagzine--; // 발사시 현재 탄환 수 감소

        if (curMagzine > 0) // 탄환수가 0 이상이면
        {
            StartCoroutine(ShootAnimation());
            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.collider.tag == "Monster")
                {
                    Debug.Log($" {hit.collider.name} ");
                    StartCoroutine(OnFlashEffect(hit));
                    monsterController.curMonsterHp -= damange; // 몬스터의 체력에 데미지
                    Debug.Log($"현재 몬스터 체력(shoot) :{ monsterController.curMonsterHp}");
                }
            }

        }
        else if (curMagzine <= 0)
        {
            Debug.Log("재장전이 필요합니다.");
            curMagzine = 0;
            isReload = true; // 재장전 필요
        }
    }

    public void Reload()
    {
        StartCoroutine(OnReload());
        isReload = false;
    }
    /// <summary>
    /// 총알 충돌 이펙트 발사 코루틴
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public IEnumerator OnFlashEffect(RaycastHit hit)
    {
        GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.identity); // hit 이펙트 발생
        yield return new WaitForSeconds(0.2f);
        Destroy(impact); // 0.2초후 삭제
    }

    /// <summary>
    /// 총알 장전 코루틴
    /// UI의 장전을 숫자가 올라가는 것이 보이게 생성되기 위해서 코루틴으로 설정
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnReload()
    {
        while (curMagzine < maxMagazine)
        {
            curMagzine++;
            ReloadSound.Play();
            yield return new WaitForSeconds(0.05f);
        }
        curMagzine = maxMagazine;
    }

    public IEnumerator ShootAnimation()
    {
        animator.SetBool("isShoot", true);
        ShootingSound.Play();
        yield return new WaitForSeconds(0.05f);
        animator.SetBool("isShoot", false);
    }
}
