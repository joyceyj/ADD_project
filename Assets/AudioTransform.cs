using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTransform : MonoBehaviour
{
    // audio
    public AudioSource thisAudioSource;
    // the numbers of fountain, should be 2^n between 64 to 8192
    public int fountainNum = 64;
    // spectrum data from audio, reflect to fountain
    // private List<float> spectrumData = new List<float>(fountainNum);
    private float[] spectrumData = new float[64];
    //fountain object - use particle system
	public GameObject cubePrototype;
    // the given start point for pose the fountain
	public Transform startPoint;
    // the scale transform of the fountain
	private Transform[] fountainTransforms=new Transform[64];
    private Vector3[] fountatinPosition= new Vector3[64];
    // Start is called before the first frame update
    void Start()
    {
        //generate cube and pose as a circle(should be replace as fountain)
		Vector3 p=startPoint.position;

		for(int i=0;i<fountainNum;i++){
			p=new Vector3(p.x+1.5f,p.y,p.z);
            GameObject cube=Object.Instantiate(cubePrototype,p,cubePrototype.transform.rotation)as GameObject;
			fountainTransforms[i]=cube.transform;
            
		}

		p=startPoint.position;

		float a=2f*Mathf.PI/(fountainNum/2);

		for(int i=0;i<(fountainNum/2);i++){
			fountainTransforms[i].position=new Vector3(p.x+Mathf.Cos(a)*131,p.y,p.z+131*Mathf.Sin(a));
			a+=2f*Mathf.PI/(fountainNum/2);
            fountatinPosition[i]=fountainTransforms[i].position;
			fountainTransforms[i].parent=startPoint;
		}

        //delay to play audio
        thisAudioSource.PlayDelayed(2f);
    }

    // Update is called once per frame
    void Update()
    {
        Spectrum2Scale();
        // Spectrum2Color();
    }
    //thisAudioSource 
    void Spectrum2Scale(){
        thisAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        for (int i = 0; i < (fountainNum/2); i++)
        {
            fountainTransforms[i].localScale = new Vector3(0.15f, Mathf.Lerp(fountainTransforms[i].localScale.y, spectrumData[i] * 10000f, 0.5f), 0.15f);
            // Debug.Log("the transform: " + fountainTransforms[i].localScale);
        }
    }
}
