using UnityEngine;
using System.Collections;

public class Player3PController : MonoBehaviour {

	public float walkSpeed = 2.0f;
	public float runSpeed = 8.0f;
	public float animSpeed = 1.5f;

	public float turnSmoothing = 3.0f;
	public float aimTurnSmoothing = 15.0f;
	public float speedDampTime = 0.1f;

	public bool isPlayerBusy = false;
	public bool hasGun = false;
	public bool hasKey = false;

	private Animator anim;
	private Transform cameraTransform;
	private int speedFloat;
	private bool isMoving;
	private float speed;
	private Vector3 lastDirection;


	public bool IsAiming() {

		return (Input.GetKey (KeyCode.Mouse1));
	}

	void Awake()
	{
		anim = this.gameObject.GetComponent<Animator> ();
		cameraTransform = Camera.main.transform;
		
		//speedFloat = Animator.StringToHash("Speed");
		/*	jumpBool = Animator.StringToHash("Jump");
		hFloat = Animator.StringToHash("H");
		vFloat = Animator.StringToHash("V");
		aimBool = Animator.StringToHash("Aim");
		// fly
		flyBool = Animator.StringToHash ("Fly");
		groundedBool = Animator.StringToHash("Grounded");
		distToGround = GetComponent<Collider>().bounds.extents.y;
		*/
	}

	// Use this for initialization
	void Start () {

		anim = this.gameObject.GetComponent<Animator> ();
		//Debug.Log ("animator = " + anim.ToString());
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("Speed = " + speed + " animator speed = " + anim.GetFloat("Speed"));

		float h = Input.GetAxis ("Horizontal");			
		float v = Input.GetAxis ("Vertical");

		if (!isPlayerBusy) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				anim.SetTrigger ("B_Jump");
			}

			if (Input.GetKeyDown (KeyCode.F)) {
				//anim.SetLayerWeight(1,1);
				//anim.SetBool("HandAnimation", true);
				//anim.SetBool("H_ChestPumpSalute", true);
				//anim.SetTrigger("H_Grab");
			}

			//if (!anim.GetBool ("B_Jump")) {
				isMoving = Mathf.Abs (v) > 0.1;

				if (isMoving) {
					//Debug.Log("Player is moving");
					MovementManagement (h, v);
				} else {
					//Debug.Log("Player not moving");
					UpdateDirection ();
				}

			//}
		}
		//Debug.Log ("Speed = " + speed + " animator speed = " + anim.GetFloat("Speed"));
	}

	void FixedUpdate ()
	{
		if (Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (0);

		//MovementManagement (h, v); 				
	}

	void UpdateDirection() {

		Vector3 cameraAngle = cameraTransform.eulerAngles;
		Vector3 playerAngle = transform.eulerAngles;
		float angDiff = cameraAngle.y - playerAngle.y;
		if (angDiff < 0)
			angDiff = 360 + angDiff;
		if (angDiff > 180)
			angDiff = angDiff - 360;

		//float temp = cameraAngle.y - playerAngle.y;
		//Debug.LogError (angDiff + " - " + temp);
		anim.SetFloat ("Direction", angDiff);

	}

	void MovementManagement(float horizontal, float vertical)
	{
		float angularSpeed = horizontal;

		//if (Mathf.Abs(horizontal) > 2000) {

		Vector3 cameraAngle = cameraTransform.eulerAngles;
		Vector3 playerAngle = transform.eulerAngles;
		float angDiff = cameraAngle.y - playerAngle.y;
		if (angDiff < 0)
			angDiff = 360 + angDiff;
		if (angDiff > 180)
			angDiff = angDiff - 360;
		//Debug.LogError (angDiff);

		//	Quaternion forward = cameraTransform.rotation;
			/*forward = forward.normalized;
			Vector3 right = new Vector3 (forward.z, 0, -forward.x);
			Vector3 targetDirection;
			float finalTurnSmoothing;

			targetDirection = forward * vertical + right * horizontal;
			finalTurnSmoothing = turnSmoothing;

			Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
*/
		//}
		////
		if (Input.GetKey (KeyCode.LeftShift)) {

			speed = runSpeed;
		} else {

			speed = walkSpeed;
		}
		//Debug.Log ("Speed = " + speed + " animator speed = " + anim.GetFloat("Speed"));
		anim.SetFloat ("Speed", speed);	
		//Debug.Log ("Speed = " + speed + " animator speed = " + anim.GetFloat("Speed"));
		//anim.SetFloat ("AngularSpeed", horizontal*100);
		anim.SetFloat ("AngularSpeed", angDiff);
		//Rotating(horizontal, vertical);
		
		//if(isMoving)
		//{
			//	speed = walkSpeed;
			//Debug.LogError("isMoving" + v.ToString() + " - " + speed);
			//anim.SetFloat("Speed", speed);	
			//anim.SetFloat("Direction", h);
			//anim.speed = animSpeed;
			//anim.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);
		//}
		//else
		//{
		//	speed = 0f;
		//	anim.SetFloat("Speed", 0f);
		//}
		//GetComponent<Rigidbody>().AddForce(Vector3.forward*speed);
		//Debug.Log ("Speed = " + speed + " animator speed = " + anim.GetFloat("Speed"));
	}
	/*
	Vector3 Rotating(float horizontal, float vertical)
	{
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward = forward.normalized;
		
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		
		Vector3 targetDirection;
		
		float finalTurnSmoothing;
		
		if(IsAiming())
		{
			targetDirection = forward;
			finalTurnSmoothing = aimTurnSmoothing;
		}
		else
		{
			targetDirection = forward * vertical + right * horizontal;
			finalTurnSmoothing = turnSmoothing;
		//}
		
		if((isMoving && targetDirection != Vector3.zero))// || IsAiming())
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
			// fly
			//if (fly)
			//	targetRotation *= Quaternion.Euler (90, 0, 0);
			
			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
			lastDirection = targetDirection;
		}
		//idle - fly or grounded
		if(!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
		{
			Repositioning();
		}
		
		return targetDirection;
	}

	private void Repositioning()
	{
		Vector3 repositioning = lastDirection;
		if(repositioning != Vector3.zero)
		{
			repositioning.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation (repositioning, Vector3.up);
			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
		}
	}*/

}
