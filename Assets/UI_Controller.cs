using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
public class UI_Controller : MonoBehaviour
{
    ShooterController shooterController;
    Animator animator; // Animation 제어

    [SerializeField] TextMeshProUGUI magazineText; // 탄환수 Text
    [SerializeField] int curMangazine; // 현재 탄환수
    [SerializeField] int maxMangazine; // 최대 탄환수
    [SerializeField] GameObject UI_Magzine; // 탄환수 UI 오브젝트
    [SerializeField] GameObject UI_reload; // 재장전 메세지

    bool isMenuAnimation; // 메뉴 활성화 판단여부
    bool isReload; // 재장전 판단여부


    private void Awake()
    {
        shooterController = GameObject.Find("ShooterController").GetComponent<ShooterController>();
        animator = GameObject.Find("menu").GetComponent<Animator>();
        maxMangazine = shooterController.maxMagazine;
        isMenuAnimation = false;
    }
    private void Update()
    {
        isReload = shooterController.isReload; // 재장전 여부 판단 불러오기
        curMangazine = shooterController.curMagzine; // 현재 탄환 수 불러오기
        magazineText.text = $"{curMangazine.ToString()}/{maxMangazine.ToString()}";
    }
    public void OnShootReload()
    {
        if(isReload == true)
        {
            StartCoroutine(OnTextReload());
        }
    }

    /// <summary>
    /// 메뉴 애니메이션의 작동
    /// </summary>
    public void OnMenu()
    {
        if (isMenuAnimation == false)
        {
            animator.SetBool("Menu", true);
            isMenuAnimation = true;
        }
        else if (isMenuAnimation == true)
        {
            animator.SetBool("Menu", false);
            isMenuAnimation = false;
        }
    }
    /// <summary>
    /// 어플리케이션 종료
    /// </summary>
    public void GameExit()
    {
        Application.Quit();
        Debug.Log("앱 종료");
    }

    /// <summary>
    /// 재장전 메세지 등장 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnTextReload()
    {
        UI_reload.SetActive(true);
        yield return new WaitForSeconds(3f);
        UI_reload.SetActive(false);
    }
}
