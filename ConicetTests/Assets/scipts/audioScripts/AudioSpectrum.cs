using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    AudioSource audioSource;
    public bool isOn;
    public float result;

    public void Start()
    {
        isOn = true;
    }
    public void SetOff()
    {
        isOn = false;
        result = 0;
    }
    public void SetAudioSource(AudioSource au)
    {
        audioSource = au;
    }
    void Update()
    {
        if (!isOn || audioSource == null || !audioSource.isPlaying)
            return;

        float[] spectrum = new float[256];

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        int frag = (int)(spectrum.Length / 4);
        result = spectrum[(frag * 0)] + spectrum[(frag * 0) + 1] + spectrum[(frag * 0) + 2];

    }
}

//		result3 = result1 * (Random.Range (0, 50) - 100) / 60;
//		result4 = result1 * (Random.Range (0, 50) - 100) / 60;
//		result5 = result1 * (Random.Range (0, 50) - 100) / 60;
//		//result2 = spectrum [(frag*1)]+ spectrum [(frag*1)+1]+ spectrum [(frag*1)+2];
//		//result3 = spectrum [(frag*2)]+ spectrum [(frag*2)+1]+ spectrum [(frag*2)+2];
//		//result4 = spectrum [(frag*3)]+ spectrum [(frag*3)+1]+ spectrum [(frag*3)+2];
//		//result5 = spectrum [(frag*4)-3]+ spectrum [(frag*4)-2]+ spectrum [(frag*4)-1];
//		//a /= spectrum.Length;
//		//print(spectrum.Length);
//	//	result = (int)Mathf.Lerp (1, 100, (a / spectrum.Length) * 1500);

//	}
//}