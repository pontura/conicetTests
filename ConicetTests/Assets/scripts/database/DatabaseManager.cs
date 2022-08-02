using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseManager : MonoBehaviour
{
    string hashPassword = "pontura";
    string url = "http://localhost/conicet/";
    

    public void SaveUser(DatabaseUser userdata, System.Action OnSaved)
    {
        string hash = Md5Test.Md5Sum(userdata.id + hashPassword);
        string urlReal = url
            + "setUser.php?name=" + userdata.name
            + "&age=" + userdata.age
            + "&gender=" + userdata.gender
            + "&text=" + userdata.text
            + "&userID=" + userdata.id
            + "&hash=" + hash;
        print("save: " + urlReal);
        StartCoroutine(RequestDone(urlReal, OnSaved));
    }
    public void SaveGame(string userID, DatabaseUserGame gameData, System.Action OnSaved)
    {
        string hash = Md5Test.Md5Sum(gameData.gameID + hashPassword);
        string urlReal = url
            + "setGame.php?gameID=" + gameData.gameID
            + "&game=" + gameData.game
            + "&userID=" + userID
            + "&duration=" + gameData.duration
            + "&correct=" + gameData.correct
            + "&incorrect=" + gameData.incorrect             
            + "&lang=" + gameData.lang
            + "&cuento=" + gameData.cuento
            + "&day=" + gameData.day
            + "&hash=" + hash;
        print("save: " + urlReal);
        StartCoroutine(RequestDone(urlReal, OnSaved));
    }
    public void SaveWords(string gameID, DatabaseUserWords word, string game, System.Action OnSaved)
    {
        string hash = Md5Test.Md5Sum(word.gameID + hashPassword);
        string urlReal = url
            + "setWord.php?gameID=" + word.gameID
            + "&word=" + word.word
            + "&correct=" + word.correct
            + "&hash=" + hash;
        print("save: " + urlReal);
        StartCoroutine(RequestDone(urlReal, OnSaved));
    }
    IEnumerator RequestDone(string url, System.Action OnSaved)
    {
        print(url);

        WWW www = new WWW(url);
        yield return www;
        print("SAVE response: " + www.text);
        if (www.text == "ok")
        {
            Debug.Log("SAVED: " + www.text);
            OnSaved();
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }
    }
}
