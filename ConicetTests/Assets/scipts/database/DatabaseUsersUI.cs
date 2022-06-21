using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseUsersUI : MonoBehaviour
{
    static DatabaseUsersUI mInstance = null;
    public int tabletID;
    public int userAutoIncrementID;

    public static DatabaseUsersUI Instance  {  get  { return mInstance;  }    }

    [SerializeField] DatabaseUserButton button;
    [SerializeField] GameObject panel;
    [SerializeField] Text tabletField;

    [SerializeField] Transform container;
    public DatabaseUser active;
    DatabaseUserAdd databaseUserAdd;

    public DatabaseData databaseData;
    public DatabaseManager databaseManager;
    public DatabaseTablet databaseTablet;

    void Awake()
    {
        mInstance = this;
    }
    void Start()
    {
        databaseManager = GetComponent<DatabaseManager>();
        databaseData = GetComponent<DatabaseData>();
        databaseUserAdd = GetComponent<DatabaseUserAdd>();
        databaseTablet = GetComponent<DatabaseTablet>();

        Events.OnStatsGameDone += OnStatsGameDone;

        databaseUserAdd.Close();

        tabletID = PlayerPrefs.GetInt("tabletID", 0);
        userAutoIncrementID = PlayerPrefs.GetInt("userAutoIncrementID", 0);

        if (tabletID == 0)
            databaseTablet.Init();
        else
        {
            Open();
        }
    }
    public void Open()
    {
        databaseTablet.SetOff();
        tabletField.text = "TABLET " + tabletID;
        panel.SetActive(true);
        RefreshList();
    }
    public void Close()
    {
        panel.SetActive(false);
    }
    public void SelectUser(DatabaseUser user)
    {
        active = user;
        Close();
    }
    public void Add()
    {
        databaseUserAdd.Init();
    }
    public void AddNewUser(DatabaseUser user)
    {
        userAutoIncrementID++;
        PlayerPrefs.SetInt("userAutoIncrementID", userAutoIncrementID);
        active = user;
        databaseData.AddUser(user);
        RefreshList();
    }
    void RefreshList()
    {
        Utils.RemoveAllChildsIn(container);
        foreach(DatabaseUser user in databaseData.users)
        {
            DatabaseUserButton b = Instantiate(button, container);
            b.Init(user);
        }
    }
    public void Save()
    {
        int id = 0;
        foreach (DatabaseUser user in databaseData.users)
        {
            id++;
            user.arrayID = id;
            if (!user.IsSavedToDatabase())
                databaseManager.SaveUser(user, user.SavedToDatabase);
           
            user.SaveGames(OnAllGamesSaved);
        }
    }
    void OnAllGamesSaved()
    {
        print("OnAllGamesSaved");
        RefreshList();
    }
    DatabaseUser UserActive()
    {
        foreach (DatabaseUser user in databaseData.users)
        {
            if (user.id == active.id)
                return user;
        }
        return null;
    }
    void OnStatsGameDone(int duration, List<string> correctList, List<string> incorrectList)
    {
        DatabaseUser userActive = UserActive();
        DatabaseUserGame dbUserGame = new DatabaseUserGame();
        dbUserGame.gameID = userActive.id+"_"+ userActive.games.Count+Random.Range(0,10000);
        dbUserGame.correct = correctList.Count;
        dbUserGame.incorrect = incorrectList.Count;
        dbUserGame.duration = duration;

        userActive.AddGame(dbUserGame);

        databaseData.SetGamesData(userActive, userActive.games.Count, dbUserGame);

        int id = 0;
        foreach (string s in correctList)
        {
            OnStatsAddWord(dbUserGame, s, true);
            id++;
            databaseData.SetWordsData(id, dbUserGame.gameID, s, true, dbUserGame.game);
        }

        foreach (string s in incorrectList)
        {
            OnStatsAddWord(dbUserGame, s, false);
            id++;
            databaseData.SetWordsData(id, dbUserGame.gameID, s, false, dbUserGame.game);
        }

    }

    void OnStatsAddWord(DatabaseUserGame game, string word, bool isCorrect)
    {
        DatabaseUserWords dbUserData = new DatabaseUserWords();
        dbUserData.gameID = game.gameID;
        if(isCorrect)
            dbUserData.correct = 1;
        dbUserData.word = word;
        DatabaseUser userActive = UserActive();
        game.AddWord(dbUserData);
    }

}
