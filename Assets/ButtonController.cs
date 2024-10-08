using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void OnMenu()
    {
        Debug.Log("메뉴 클릭 성공");
    }
    public void OnShoot()
    {
        Debug.Log("총알 발사 클릭 성공");
    }
    public void OnReload()
    {
        Debug.Log("재장전 클릭 성공");
    }
}
