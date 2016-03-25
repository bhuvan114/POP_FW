﻿using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class GunTrigger : MonoBehaviour
{

    private BehaviorAgent behaviorAgent;
    private Node root = null;
    private DrawGun drawGun;
    private ShootGun shootGun;

    public GameObject gun;
    public Player3PController playerController;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

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
