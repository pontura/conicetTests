using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseUserAdd : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] InputField nameField;
    [SerializeField] InputField ageField;
    [SerializeField] InputField textField;
    [SerializeField] Dropdown sexField;
    DatabaseUsersUI databaseUsersUI ;

    private void Awake()
    {
        databaseUsersUI = GetComponent<DatabaseUsersUI>();
    }
    public void Init()
    {
        panel.SetActive(true);
    }
    public void Add()
    {
        DatabaseUser user = new DatabaseUser();
        user.games = new List<DatabaseUserGame>();
        user.name = nameField.text;
        if(ageField.text != "")
            user.age = int.Parse(ageField.text);
        user.text = textField.text;
        user.id = textField.text;
        user.gender = sexField.value.ToString();
        user.GenerateID();
        databaseUsersUI.AddNewUser(user);
        Close();
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
