using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public float boundary;
	public float scrollSpeed;
	public float zoomSpeed;

	private float screenWidth;
	private float screenHeight;

	// Use this for initialization
	void Start () {
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		// Screen scrolling using arrow keys
		if (Input.GetKey(KeyCode.UpArrow)) {   
			transform.position += new Vector3(0, 0, scrollSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.DownArrow)) {    
			transform.position += new Vector3(0, 0, -scrollSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {    
			transform.position += new Vector3(-scrollSpeed * Time.deltaTime, 0, 0);
		}

		if (Input.GetKey(KeyCode.RightArrow)) { 
			transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
		}


		// Screen scrolling using mouse cursor
		/*float mousePosX = Input.mousePosition.x;
		float mousePosY = Input.mousePosition.y;

		if (mousePosY > screenHeight - boundary) {    // UP
			transform.position += new Vector3(0, 0, scrollSpeed * Time.deltaTime);
		}

		if (mousePosY < 0 + boundary) {    // DOWN
			transform.position += new Vector3(0, 0, -scrollSpeed * Time.deltaTime);
		}

		if (mousePosX < 0 + boundary) {    // LEFT
			transform.position += new Vector3(-scrollSpeed * Time.deltaTime, 0, 0);
		}

		if (mousePosX > screenWidth - boundary) {    // RIGHT
			transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
		}*/


		// Zoom using mouse scrol wheel
		float scroll = Input.mouseScrollDelta.y;

		if (scroll > 0) {    // ZOOM OUT
			transform.position += new Vector3(0, zoomSpeed * Time.deltaTime, 0);
		}

		if (scroll < 0) {    // ZOOM IN
			if (transform.position.y > 25) {
				transform.position += new Vector3(0, -zoomSpeed * Time.deltaTime, 0);
			}
		}
	}
}
