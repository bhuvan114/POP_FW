using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class GunTrigger : MonoBehaviour
{

    private BehaviorAgent behaviorAgent;
    private Player3PController playerController;
    private Node root = null;
    private bool inReach;
    private DrawGun drawGun;
    // Use this for initialization
    void Start()
    {
        inReach = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            root = drawGun.execute();
            behaviorAgent = new BehaviorAgent(root);

            BehaviorManager.Instance.Register(behaviorAgent);
            behaviorAgent.StartBehavior();
            playerController.isPlayerBusy = true;
        }
    }
}
