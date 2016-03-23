using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
		}
		if (Input.GetKeyDown("escape"))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = true;
		}
	}
}
