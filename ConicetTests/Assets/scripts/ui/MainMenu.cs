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
    [SerializeField] Text testField;

    private void Awake()
    {
        SetOff();
    }
    public void Init()
    {
        panel.SetActive(true);

        dataField.text = "";
        tabletField.text = "TABLET " + Data.Instance.tabletID;

        if (Data.Instance.databaseContent.GetActive() == null)
            testTitleField.text = "";
        else
            Invoke("Delayed", 0.15f);
        testField.text = "EMPEZAR [" + Data.Instance.GetFileName(true) + "]";
    }
    void Delayed()
    {
        testTitleField.text = Data.Instance.databaseContent.GetActive().name;

    }
    public void Clkicked()
    {
        SetOff();
        Data.Instance.SetNewUser();
        GetComponent<Conicet.UI.UIGame>().Init();
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
}
