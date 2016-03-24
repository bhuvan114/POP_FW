using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class GunTrigger : MonoBehaviour
{

    private BehaviorAgent behaviorAgent;
    private Player3PController playerController;
    private Node root = null;
    private DrawGun drawGun;
    private GameObject gun;

    // Use this for initialization
    void Start()
    {
        gun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Draw");
            root = new Sequence(drawGun.execute(), drawGun.UpdateState());
            behaviorAgent = new BehaviorAgent(root);
            BehaviorManager.Instance.Register(behaviorAgent);
            behaviorAgent.StartBehavior();
            playerController.isPlayerBusy = true;
            gun.SetActive(true);
            gun.GetComponent<GunController>().SetIsHolding(true);
        }

        if (root != null)
        {
            if (!root.IsRunning)
            {
                playerController.isPlayerBusy = false;
                playerController.gameObject.GetComponent<CharacterMecanim>().ResetAnimation();
                root = null;

                NarrativeState.recomputePlan = true;
            }
        }
    }

}
