using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{

    
    public GameObject sampleObjectPrefab;
    GameObject[] sampleObject = new GameObject[512];
    public float maxScale;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 512; i++)
        {
            // cycle through each sample object
            // create a new object for each sample object
            // set parent and object position to the object's transform
            // change the name of each sample object
            // change the rotation of each sample object
            // 360/512 = .07
            GameObject instanceSampleObject = (GameObject)Instantiate (sampleObjectPrefab);
            instanceSampleObject.transform.position = this.transform.position;
            instanceSampleObject.transform.parent = this.transform;
            instanceSampleObject.name = "SampleObject" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instanceSampleObject.transform.position = Vector3.forward * 100;
            sampleObject[i] = instanceSampleObject;
            Debug.Log("sample " + sampleObject[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 512; i++)
        {   
            if(sampleObject != null)
            {
                
                sampleObject[i].transform.localScale = new Vector3(10, (AudioSignal.samples [i] * maxScale) + 2, 10);
            }
        }
        
    }
}
