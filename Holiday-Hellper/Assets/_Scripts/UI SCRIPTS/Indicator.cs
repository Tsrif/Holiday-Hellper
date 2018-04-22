using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {

    public Transform arrow;
    public Transform playerTransform;
    public Transform[] targets;
    public Transform target;
    public int count;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        count = HouseCompletionCounter.completionNum();
        Debug.Log("count is: " + count);

        if (targets != null)
        {
            if (count == 0)
            {
                Vector3 dir = playerTransform.InverseTransformPoint(targets[0].position);
                float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                a += 180;
                arrow.transform.localEulerAngles = new Vector3(0, 180, a);
            }
            else if(count == 1)
            {
                Vector3 dir = playerTransform.InverseTransformPoint(targets[1].position);
                float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                a += 180;
                arrow.transform.localEulerAngles = new Vector3(0, 180, a);
            }
            else if (count == 2)
            {
                Vector3 dir = playerTransform.InverseTransformPoint(targets[2].position);
                float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                a += 180;
                arrow.transform.localEulerAngles = new Vector3(0, 180, a);
            }
            else if (count == 3)
            {
                Vector3 dir = playerTransform.InverseTransformPoint(targets[3].position);
                float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                a += 180;
                arrow.transform.localEulerAngles = new Vector3(0, 180, a);
            }
            else if (count == 4)
            {
                Vector3 dir = playerTransform.InverseTransformPoint(targets[4].position);
                float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                a += 180;
                arrow.transform.localEulerAngles = new Vector3(0, 180, a);
            }


        }

	}

}
