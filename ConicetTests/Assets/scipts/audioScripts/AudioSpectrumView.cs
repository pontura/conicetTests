using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSpectrumView : MonoBehaviour {

    public float minSensibilityValue = .001f;
    public float multiplier = 6;
    bool isTalking;
	public Image[] images;
    float lastValue;

    void OnEnable()
    {
        ResetAll();
        images[0].enabled = true;
        lastValue = -1;
        Invoke("Loop", 0.1f);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    void Loop () {
        //SetImage(Data.Instance.audioSpectrum.result);
        Invoke("Loop", 0.1f);
	}
    void ResetAll()
    {
        foreach (Image image in images)
            image.enabled = false;
    }

	void SetImage(float value)
	{
        if (lastValue == value)
        {
            ShutMouth();
        }
        else
        {
            lastValue = value;

            if (value < minSensibilityValue)
                ShutMouth();
            else
                RandomMouth();
        }
    }
    void ShutMouth()
    {
        ResetAll();
        images[0].enabled = true;
    }
    void RandomMouth()
    {
        ResetAll();
        images[UnityEngine.Random.Range(0, images.Length)].enabled = true;
    }
}
