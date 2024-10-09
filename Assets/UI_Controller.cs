using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
public class UI_Controller : MonoBehaviour
{
    ShooterController shooterController;
    Animator animator; // Animation ����

    [SerializeField] TextMeshProUGUI magazineText; // źȯ�� Text
    [SerializeField] int curMangazine; // ���� źȯ��
    [SerializeField] int maxMangazine; // �ִ� źȯ��
    [SerializeField] GameObject UI_Magzine; // źȯ�� UI ������Ʈ
    [SerializeField] GameObject UI_reload; // ������ �޼���

    bool isMenuAnimation; // �޴� Ȱ��ȭ �Ǵܿ���
    bool isReload; // ������ �Ǵܿ���


    private void Awake()
    {
        shooterController = GameObject.Find("ShooterController").GetComponent<ShooterController>();
        animator = GameObject.Find("menu").GetComponent<Animator>();
        maxMangazine = shooterController.maxMagazine;
        isMenuAnimation = false;
    }
    private void Update()
    {
        isReload = shooterController.isReload; // ������ ���� �Ǵ� �ҷ�����
        curMangazine = shooterController.curMagzine; // ���� źȯ �� �ҷ�����
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
    /// �޴� �ִϸ��̼��� �۵�
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
    /// ���ø����̼� ����
    /// </summary>
    public void GameExit()
    {
        Application.Quit();
        Debug.Log("�� ����");
    }

    /// <summary>
    /// ������ �޼��� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnTextReload()
    {
        UI_reload.SetActive(true);
        yield return new WaitForSeconds(3f);
        UI_reload.SetActive(false);
    }
}
