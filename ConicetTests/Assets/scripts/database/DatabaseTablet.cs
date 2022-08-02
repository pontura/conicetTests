using Conicet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseTablet : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] InputField nameField;
    DatabaseUsersUI databaseUsersUI;

    private void Awake()
    {
        databaseUsersUI = GetComponent<DatabaseUsersUI>();
    }
    public void Init()
    {
        panel.SetActive(true);
    }
    public void SetOff()
    {
        panel.SetActive(false);
    }
    public void Add()
    {
        if (nameField.text != "0" && nameField.text.Length > 0)
        {
            int tabletID = int.Parse(nameField.text);
            DatabaseUsersUI.Instance.tabletID = tabletID;
            PlayerPrefs.SetInt("tabletID", tabletID);
            UIMain.Instance.Init();
        }
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
