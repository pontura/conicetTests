using System.Collections.Generic;
using System;

[Serializable]
public class TestData
{
    public string userid;
    public List<float> time;

    public void Init(string _userid)
    {
        this.userid = _userid;
        time = new List<float>();
    }
    public void Add(float _time)
    {
        time.Add(_time);
    }
}
