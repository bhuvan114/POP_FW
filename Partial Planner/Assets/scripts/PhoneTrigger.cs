using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class PhoneTrigger : MonoBehaviour {

    private BehaviorAgent behaviorAgent;
    private Node root = null;
    private UsePhone usePhone;
    //private PhoneController phoneController;

    public Player3PController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<Player3PController>();
        //phoneController = this.gameObject.GetComponent<PhoneController>();
    }

    // Update is called once per frame
    void Update()
    {

            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.LogWarning("Initiate use phone");

                usePhone = new UsePhone(this.GetComponent<SmartPhone>(), playerController.GetComponent<SmartCharacter>());
                root = new Sequence(usePhone.execute(), usePhone.UpdateState());
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
