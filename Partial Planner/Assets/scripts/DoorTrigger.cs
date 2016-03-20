using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class DoorTrigger : MonoBehaviour {

	public DoorScript door;

	private OpenDoor openDoor;
	private CloseDoor closeDoor;
	private BehaviorAgent behaviorAgent;
	private Player3PController playerController;
	private Node root = null;
	private bool doorState = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		if (door.isPlayerDetected && Input.GetKeyDown (KeyCode.E)) {
			//Debug.LogError("Key Recorded");
			if(door.Running == false) {
				if (door.State == 0) {
					Debug.Log("Open");
					root = openDoor.execute();
					behaviorAgent = new BehaviorAgent (root);

				} else {
					Debug.Log("Close");
					root = closeDoor.execute();
					behaviorAgent = new BehaviorAgent (root);
				}
			
				BehaviorManager.Instance.Register (behaviorAgent);
				behaviorAgent.StartBehavior ();
				playerController.isPlayerBusy = true;

			}
			//	StartCoroutine(door.Open ());
		}

		if (root != null) {
			if(!root.IsRunning) {
				playerController.isPlayerBusy = false;
				playerController.gameObject.GetComponent<CharacterMecanim>().ResetAnimation();
				root = null;
				//doorState = true;
				door.State ^= 1;
			}
		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {
			door.isPlayerDetected = true;
			playerController = other.GetComponent<Player3PController>();
			openDoor = new OpenDoor(door.GetComponent<SmartDoor>(), other.GetComponent<SmartCharacter>());
			closeDoor = new CloseDoor(door.GetComponent<SmartDoor>(), other.GetComponent<SmartCharacter>());
		}
	}

	void OnTriggerExit(Collider other) {

		if (other.tag == "Player") {
			door.isPlayerDetected = false;
			openDoor = null;
		}
	}
}
