using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseData : MonoBehaviour
{
    public List<DatabaseUser> users;

    private void Awake()
    {
        LoadUsers();
    }
    void LoadUsers()
    {
        for (int a = 1; a < 1000; a++)
        {
            string key = "user" + a;
            string userDataParsed = PlayerPrefs.GetString(key, "");
            if (userDataParsed == "")
                return;
            DatabaseUser user = new DatabaseUser();
            users.Add(user);
            user.games = new List<DatabaseUserGame>();
            print("________________" + userDataParsed);
            string[] arr = userDataParsed.Split(":"[0]);
            if (arr.Length > 0)
            {

                if (arr[0] == null || arr[0].Length < 1)
                    user.saved = 0;
                else
                    user.saved = int.Parse(arr[0]);

                user.id = arr[1];
                user.name = arr[2];
                user.age = int.Parse(arr[3]);
                user.text = arr[4];
                user.gender = arr[5];
                LoadGamesFor(user);
            }
        }
    }
    void LoadGamesFor(DatabaseUser user)
    {
        for (int a = 1; a < 1000; a++)
        {
            string dataParsed = GetGamesData(user, a);
            if (dataParsed == "")
                return;
            DatabaseUserGame gameData = new DatabaseUserGame();
            gameData.words = new List<DatabaseUserWords>();
            user.games.Add(gameData);
            print("___________" + dataParsed);
            string[] arr = dataParsed.Split(":"[0]);
            if (arr.Length > 0)
            {
                gameData.gameID = arr[0];
                gameData.correct = int.Parse(arr[1]);
                gameData.incorrect = int.Parse(arr[2]);
                gameData.duration = int.Parse(arr[3]);
                gameData.lang = arr[4];
                gameData.cuento = arr[5];
                gameData.day = int.Parse(arr[6]);
                gameData.game = arr[7];
            }
            LoadWordsFor(gameData);
        }
    }
    void LoadWordsFor(DatabaseUserGame game)
    {
        for (int a = 1; a < 1000; a++)
        {
            string dataParsed = GetWordsData(game, a);
            if (dataParsed == "")
                return;
            DatabaseUserWords wData = new DatabaseUserWords();
            game.words.Add(wData);
            string[] arr = dataParsed.Split(":"[0]);
            if (arr.Length > 0)
            {
                wData.gameID = arr[0];
                wData.word = arr[1];
                wData.correct = int.Parse(arr[2]);
            }
        }
    }
    public void AddUser(DatabaseUser user)
    {
        users.Insert(0,user);
        user.Save(users.Count);
    }
    public void Delete(DatabaseUser user)
    {
        users.Remove(user);
    }
    public void DeleteAll()
    {
        users.Clear();
    }
    string GetGamesData(DatabaseUser user, int arrNum)
    {
        string key = "g_" + user.id + "_" + arrNum;
        return PlayerPrefs.GetString(key, "");
    }
    public void SetGamesData(DatabaseUser user, int arrNum, DatabaseUserGame dbUserGame)
    {
        string value = dbUserGame.gameID + ":";
        value += dbUserGame.correct + ":";
        value += dbUserGame.incorrect + ":";
        value += dbUserGame.duration + ":";
        value += dbUserGame.lang + ":";
        value += dbUserGame.cuento + ":";
        value += dbUserGame.day + ":";
        value += dbUserGame.game;

        string key = "g_" + user.id + "_" + arrNum;

        print("_" + dbUserGame.lang);
        print("____" + key);
        PlayerPrefs.SetString(key, value);
    }
    string GetWordsData(DatabaseUserGame game, int arrNum)
    {
        string key = "w_" + game.gameID + "_" + arrNum;
        return PlayerPrefs.GetString(key, "");
    }
    public void SetWordsData(int arrNum, string gameID, string word, bool isCorrect, string game )
    {
        string value = gameID + ":";
        value += word + ":";
        if (isCorrect)
            value += "1";
        else
            value += "0";

        string key = "w_" + gameID + "_" + arrNum;
        PlayerPrefs.SetString(key, value);
    }
    public void ResetGameData(string userID, int arrNum)
    {
        string key = "g_" + userID + "_" + arrNum;
        PlayerPrefs.DeleteKey(key);
    }
    public void ResetWordData(DatabaseUserWords word, int arrNum)
    {
        string key = "w_" + word.gameID + "_" + arrNum;
        PlayerPrefs.DeleteKey(key);
    }
}
