using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    Camera mainCam;
    float speed;
    float BeforeCamSize;

    public float SizeMin;
    public float SizeMax;
    public Transform TargetTransform;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
        BeforeCamSize = mainCam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
            mainCam.orthographicSize -= speed * 0.5f *Time.deltaTime;
        float dist = Vector3.Distance(Vector3.zero, TargetTransform.position);
        //dist -= 2.0f;
        if (dist <= 0.0f)
            dist = 0.0f;

            speed = mainCam.orthographicSize - dist;

        if (mainCam.orthographicSize <= SizeMin)
            mainCam.orthographicSize = SizeMin;
        else if (mainCam.orthographicSize >= SizeMax)
            mainCam.orthographicSize = SizeMax;

        BeforeCamSize = mainCam.orthographicSize;
        //mainCam.orthographicSize = Mathf.Lerp(4,10, dist * Time.deltaTime);


    }
}
