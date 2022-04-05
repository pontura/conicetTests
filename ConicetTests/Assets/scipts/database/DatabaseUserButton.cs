using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseUserButton : MonoBehaviour
{
    [SerializeField] Text namefield;
    [SerializeField] Text ageField;
    [SerializeField] Text sexField;
    [SerializeField] Text gamesQtyField;
    [SerializeField] Image circle;
    DatabaseUser data;

    private void Awake()
    {
        Events.OnUpdateDatabaseUserData += OnUpdateDatabaseUserData;
    }
    private void OnDestroy()
    {
        Events.OnUpdateDatabaseUserData -= OnUpdateDatabaseUserData;
    }
    void OnUpdateDatabaseUserData(DatabaseUser _data)
    {
        if(data != null && _data != null && data.id == _data.id)
            Init(data);
    }
    public void Init(DatabaseUser data)
    {
        this.data = data;
        namefield.text = data.name;
        ageField.text = data.age.ToString();
        gamesQtyField.text = data.GetTotalGames().ToString();

        if (data.gender == "0")
        {
            sexField.text = "NENE";
            sexField.color = Color.blue;
        }
        else
        {
            sexField.text = "NENA";
            sexField.color = Color.red;
        }

        if (data.text != "")
        {
            namefield.text += "(" + data.text + ")";
        }
        if (data.saved == 0)
            circle.color = Color.red;
        else
            circle.color = Color.green;
    }
    public void OnSelected()
    {
        GetComponentInParent<DatabaseUsersUI>().SelectUser(data);
    }
}
