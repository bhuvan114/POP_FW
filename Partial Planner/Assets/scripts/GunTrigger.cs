using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class GunTrigger : MonoBehaviour
{

    private BehaviorAgent behaviorAgent;
    private Node root = null;
    private DrawGun drawGun;
    private ShootGun shootGun;
	private GunController gunController;

    //public GameObject gun;
    public Player3PController playerController;

    // Use this for initialization
    void Start()
    {

		playerController = GameObject.Find("Player").GetComponent<Player3PController>();
		gunController = this.gameObject.GetComponent<GunController> ();
	}

    // Update is called once per frame
    void Update()
    {
		if (playerController.hasGun && !gunController.IsDrawn()) {
			if(Input.GetKeyDown(KeyCode.E)) {

				Debug.LogWarning("Initiate draw gun");
				gunController.SetIsDrawn (true);
				drawGun = new DrawGun(playerController.GetComponent<SmartCharacter>(), this.GetComponent<SmartGun>());
				root = new Sequence(drawGun.execute(), drawGun.UpdateState());
				behaviorAgent = new BehaviorAgent(root);
				BehaviorManager.Instance.Register(behaviorAgent);
				behaviorAgent.StartBehavior();
				playerController.isPlayerBusy = true;

			}
		}

		if(gunController.IsDrawn()) {
			if(Input.GetMouseButtonDown(0)) {

				//Debug.LogWarning ("Left mouse clicked!!");
				Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
				RaycastHit hit;
				int layerMask = 1 << 8;
				if (Physics.Raycast (cameraRay, out hit, 200, layerMask)) {

					//Debug.LogWarning (hit.collider.gameObject.tag);
					if (hit.collider.gameObject.CompareTag("SmartObject"))
					{
							Debug.LogWarning ("Hit!!");
					}
				}
			}
		}

		/*
        if (Input.GetMouseButtonDown(0) && !gun.activeSelf)
        {
            Debug.Log("Draw");
            gun.SetActive(true);

            drawGun = new DrawGun(playerController.GetComponent<SmartCharacter>(), gun.GetComponent<SmartGun>());
            root = new Sequence(drawGun.execute(), drawGun.UpdateState());
            behaviorAgent = new BehaviorAgent(root);
            BehaviorManager.Instance.Register(behaviorAgent);
            behaviorAgent.StartBehavior();
            playerController.isPlayerBusy = true;

        }
        else if (Input.GetMouseButtonDown(0) && gun.activeSelf)
        {
            Debug.Log("Shoot");

            shootGun = new ShootGun(playerController.GetComponent<SmartCharacter>(), gun.GetComponent<SmartGun>());
            root = new Sequence(shootGun.execute(), shootGun.UpdateState());
            behaviorAgent = new BehaviorAgent(root);
            BehaviorManager.Instance.Register(behaviorAgent);
            behaviorAgent.StartBehavior();
            playerController.isPlayerBusy = true;
        }
	*/
		

        if (root != null)
        {
            if (!root.IsRunning)
            {
                playerController.isPlayerBusy = false;
                //playerController.gameObject.GetComponent<CharacterMecanim>().ResetAnimation();
                //root = null;

                //NarrativeState.recomputePlan = true;
            }
        }
    }

}
