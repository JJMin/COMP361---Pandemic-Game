using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTool : MonoBehaviour {

    public float minDis = 79f;
    public float maxDIs=117f;
    public float sentivity = 20f;
    public float moveSpeed = 10;//相机移动速度
    
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
        float fov=gameObject.GetComponent<Camera>().fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sentivity;
        fov = Mathf.Clamp(fov, minDis, maxDIs);
        gameObject.GetComponent<Camera>().fieldOfView = fov;

        if(Input.GetMouseButton(1)){
            float h = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            float v = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            this.transform.Translate(h, v, 0,Space.Self);
            //this.transform.Translate(0, 0, v, Space.Self);    
        }
        if(Input.GetKeyDown(KeyCode.Q)){
          
            transform.localPosition = new Vector3(0, 0, -220);
            gameObject.GetComponent<Camera>().fieldOfView = 117;
        }
	}
}
