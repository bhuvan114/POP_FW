using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class StoreTrigger : MonoBehaviour {

	public GameObject StoreUIPanel;
	public GameObject store;

	private bool isPlayerDetected = false;
	private bool displayInteractionPannel = false;

	private EnterStore enterStore;
	private BuyWeapon buyWeapon;
	private BehaviorAgent behaviorAgent;
	private Node root = null;
	private Player3PController playerController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isPlayerDetected && Input.GetKeyDown (KeyCode.F)) {

			displayInteractionPannel = (displayInteractionPannel == true) ? false : true;

			if (!displayInteractionPannel) {

				StoreUIPanel.SetActive(false);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				Time.timeScale = 1f;
			}
		}

		if (displayInteractionPannel == true) {

			StoreUIPanel.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Time.timeScale = 0f;
		}

		if (root != null) {
			if (!root.IsRunning) {
				playerController.isPlayerBusy = false;
				playerController.gameObject.GetComponent<CharacterMecanim> ().ResetAnimation ();
				root = null;

				NarrativeState.recomputePlan = true;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		
		if (other.tag == "Player") {
			isPlayerDetected = true;
			StoreUIPanel.GetComponent<StoreInteractMenu>().setStore(this.gameObject);
			playerController = other.GetComponent<Player3PController>();
			enterStore = new EnterStore(store.GetComponent<SmartStore>(), other.GetComponent<SmartCharacter>());
			buyWeapon = new BuyWeapon(store.GetComponent<SmartStore>(), other.GetComponent<SmartCharacter>());
			//openDoor = new OpenDoor(door.GetComponent<SmartDoor>(), other.GetComponent<SmartCharacter>());
			//closeDoor = new CloseDoor(door.GetComponent<SmartDoor>(), other.GetComponent<SmartCharacter>());
		}
	}
	
	void OnTriggerExit(Collider other) {
		
		if (other.tag == "Player") {
			isPlayerDetected = false;
			//openDoor = null;
		}
	}

	// GUI FUNCTION
	void OnGUI ()
	{
		// Access InReach variable from raycasting script.
		//GameObject Player = GameObject.Find("Player");
		//Detection detection = Player.GetComponent<Detection>();
		
		if (isPlayerDetected)
		{
			GUI.color = Color.white;
			GUI.Box(new Rect(20, 20, 300, 25), "Press 'F' to Buy Weapon");
		}
	}

	public void BuyWeapon(){

		displayInteractionPannel = false;
		StoreUIPanel.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1f;

		root = new Sequence(enterStore.execute(), enterStore.UpdateState(), buyWeapon.execute(), buyWeapon.UpdateState());
		behaviorAgent = new BehaviorAgent (root);
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
		playerController.isPlayerBusy = true;


	}
}
