using System;
using System.Collections.Generic;

[Serializable]
public class DatabaseUserGame
{
    public string gameID;
    public string game;
    public int duration;
    public int correct;
    public int incorrect;
    public string lang;
    public string cuento;
    public int day;

    public List<DatabaseUserWords> words;

    public void AddWord(DatabaseUserWords wordData)
    {
        if (words == null)
            words = new List<DatabaseUserWords>();
        words.Add(wordData);
    }
    System.Action OnAllDone;
    public void SaveWords(System.Action OnAllDone)
    {
        this.OnAllDone = OnAllDone;
        if(words != null && words.Count > 0)
        {
            DatabaseUserWords word = words[words.Count - 1];
            DatabaseUsersUI.Instance.databaseManager.SaveWords(gameID, word, game, OnSaved);
        }
        else
            OnAllDone();
    }
    void OnSaved()
    {
        DatabaseUserWords word = words[words.Count - 1];
        DatabaseUsersUI.Instance.databaseData.ResetWordData(word, words.Count);
        words.Remove(word);
        SaveWords(OnAllDone);
    }
}
