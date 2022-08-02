using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    private void Awake()
    {
        SetOff();
    }
    public void Init()
    {
        panel.SetActive(true);
    }
    public void Clkicked()
    {
        SetOff();
    }
    void SetOff()
    {
        DatabaseUsersUI.Instance.Add();
        panel.SetActive(false);
    }
}
