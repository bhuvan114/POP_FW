using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class DoorTrigger : MonoBehaviour {

	public DoorScript door;
	public Transform OpenPoint1;
	public Transform OpenPoint2;
    public bool hasKey = false;

	private OpenDoor openDoor;
	private CloseDoor closeDoor;
    private LockDoor lockDoor;
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
					root = new Sequence(openDoor.execute(), openDoor.UpdateState());
					behaviorAgent = new BehaviorAgent (root);

				} else {
					Debug.Log("Close");
					root = new Sequence(closeDoor.execute(), closeDoor.UpdateState());
					behaviorAgent = new BehaviorAgent (root);
				}
			
				BehaviorManager.Instance.Register (behaviorAgent);
				behaviorAgent.StartBehavior ();
				playerController.isPlayerBusy = true;

			}
			//	StartCoroutine(door.Open ());
		}

        if (door.isPlayerDetected && Input.GetKeyDown(KeyCode.F) && playerController.hasKey)
        {
            //Debug.LogError("Key Recorded");
            if (door.Running == false)
            {
                if (door.State == 0)
                {
                    Debug.Log("Lock");
                    root = new Sequence(lockDoor.execute(), lockDoor.UpdateState());
                    behaviorAgent = new BehaviorAgent(root);
                }
            }


            BehaviorManager.Instance.Register(behaviorAgent);
            behaviorAgent.StartBehavior();
            playerController.isPlayerBusy = true;
        }

                    if (root != null) {
			if(!root.IsRunning) {
				playerController.isPlayerBusy = false;
				playerController.gameObject.GetComponent<CharacterMecanim>().ResetAnimation();
				root = null;
				//doorState = true;
				door.State ^= 1;
				NarrativeState.recomputePlan = true;
			}
		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {
			door.isPlayerDetected = true;
			playerController = other.GetComponent<Player3PController>();
			openDoor = new OpenDoor(door.GetComponent<SmartDoor>(), other.GetComponent<SmartCharacter>());
			closeDoor = new CloseDoor(door.GetComponent<SmartDoor>(), other.GetComponent<SmartCharacter>());
            lockDoor = new LockDoor(door.GetComponent<SmartDoor>(), other.GetComponent<SmartCharacter>());
			if(Vector3.Distance(other.transform.position, OpenPoint1.transform.position) < Vector3.Distance(other.transform.position, OpenPoint2.transform.position)) {
				door.OpenPoint = OpenPoint1.transform;
			} else {
				door.OpenPoint = OpenPoint2.transform;
			}
		}
	}



	void OnTriggerExit(Collider other) {

		if (other.tag == "Player") {
			door.isPlayerDetected = false;
			openDoor = null;
		}
	}
}
