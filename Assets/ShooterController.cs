using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    MonsterController monsterController;
    [SerializeField] Animator animator;

    [SerializeField] Transform fireCamPoint; // ������ ���� ���� = ī�޶�
    public float damange = 100;
    public float range = 50f; // ���� �Ÿ�
    [SerializeField] LayerMask layerMask; // �ε��� �浹ü ���̾� ����
    [SerializeField] private GameObject impactEffect; // �� �ǰ� ��ƼŬ

    [Header("Magzine")]
    [SerializeField] public int curMagzine; // ���� źâ��
    [SerializeField] public int maxMagazine; // �ִ� źâ��
    public bool isReload = false; // ������ �Ǵܿ���

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
        Debug.DrawRay(fireCamPoint.position, fireCamPoint.forward * range, Color.red); // ����ĳ��Ʈ�� �����ִ� �Լ�
    }

    public void Shoot()
    {
        Ray ray = new Ray(fireCamPoint.position, fireCamPoint.forward * range);
        RaycastHit hit;
        curMagzine--; // �߻�� ���� źȯ �� ����

        if (curMagzine > 0) // źȯ���� 0 �̻��̸�
        {
            StartCoroutine(ShootAnimation());
            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.collider.tag == "Monster")
                {
                    Debug.Log($" {hit.collider.name} ");
                    StartCoroutine(OnFlashEffect(hit));
                    monsterController.curMonsterHp -= damange; // ������ ü�¿� ������
                    Debug.Log($"���� ���� ü��(shoot) :{ monsterController.curMonsterHp}");
                }
            }

        }
        else if (curMagzine <= 0)
        {
            Debug.Log("�������� �ʿ��մϴ�.");
            curMagzine = 0;
            isReload = true; // ������ �ʿ�
        }
    }

    public void Reload()
    {
        StartCoroutine(OnReload());
        isReload = false;
    }
    /// <summary>
    /// �Ѿ� �浹 ����Ʈ �߻� �ڷ�ƾ
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    public IEnumerator OnFlashEffect(RaycastHit hit)
    {
        GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.identity); // hit ����Ʈ �߻�
        yield return new WaitForSeconds(0.2f);
        Destroy(impact); // 0.2���� ����
    }

    /// <summary>
    /// �Ѿ� ���� �ڷ�ƾ
    /// UI�� ������ ���ڰ� �ö󰡴� ���� ���̰� �����Ǳ� ���ؼ� �ڷ�ƾ���� ����
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
