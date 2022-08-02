using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text tabletField;
    [SerializeField] Text testTitleField;
    [SerializeField] Text dataField;

    private void Awake()
    {
        SetOff();
    }
    public void Init()
    {
        panel.SetActive(true);

        dataField.text = "";
        tabletField.text = "TABLET " + DatabaseUsersUI.Instance.tabletID;

        if (Data.Instance.databaseContent.activeText == null)
            testTitleField.text = "";
        else
            testTitleField.text = Data.Instance.databaseContent.activeText.name;
    }
    public void Clkicked()
    {
        SetOff();
        GetComponent<Conicet.UI.UIGame>().Init();
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
}
