using UnityEngine;
using System.Collections;

//using POPL.Planner;

public class GameController : MonoBehaviour {

	public GameObject PauseMenu;
	public GameObject JournalPane;

	private bool isPaused = true;
	private CursorLockMode lockMode;
	private bool cursorVisibility;
	private float timeScale;

	// Use this for initialization
	void Start () {

		lockMode = CursorLockMode.Confined;
		cursorVisibility = false;
		timeScale = 1f;
		isPaused = true;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
		//Time.timeScale = 0f;

		//PauseMenu.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {



		//if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		//{

		//}
		//if (!NarrativeState.terminated) {
			if (Input.GetKeyDown ("escape") && !JournalPane.activeSelf) {	
				if (!isPaused) {
					lockMode = Cursor.lockState;
					cursorVisibility = Cursor.visible;
					timeScale = Time.timeScale;
					isPaused = true;

					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = true;
					Time.timeScale = 0f;
					PauseMenu.SetActive (true);
				} else {
					Cursor.lockState = lockMode;
					Cursor.visible = cursorVisibility;
					Time.timeScale = timeScale;
					isPaused = false;
					PauseMenu.SetActive (false);
				}
			}

			if (Input.GetKeyDown (KeyCode.J) && !isPaused) {

				if (JournalPane.activeSelf) {

					Cursor.lockState = lockMode;
					Cursor.visible = cursorVisibility;
					Time.timeScale = timeScale;
					JournalPane.SetActive (false);
				} else {
					lockMode = Cursor.lockState;
					cursorVisibility = Cursor.visible;
					timeScale = Time.timeScale;
				
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = true;
					Time.timeScale = 0f;
					JournalPane.SetActive (true);
					NarrativeStateManager.UpdateJournal ();
				}
			}
		if (isPaused)
			Time.timeScale = 0f;
		//}
	}
}
