using System.Collections.Generic;
using System;

[Serializable]
public class TestData
{
    public int version;
    public string test;
    public string timeStamp;
    public string userid;
    public List<float> time;

    public void Init(string _userid)
    {
        this.version = Data.Instance.databaseContent.version;
        this.test = Data.Instance.databaseContent.GetActive().name;
        this.userid = _userid;
        this.timeStamp = System.DateTime.Now.ToString();
        time = new List<float>();
    }
    public void Add(float _time)
    {
        time.Add(_time);
    }
}
