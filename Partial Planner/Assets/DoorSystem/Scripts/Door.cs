////////////////////////////////////
//Last edited by: Alexander Ameye //
//on: Sunday, 22/11/2015          //
////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

public class Door : MonoBehaviour {
	
	// DOOR SETTINGS
	[Header("Door Settings")]
	[Tooltip("The start angle of the rotation (original position = 0 degrees)")]
	public float StartAngle = 0.0F;
	[Tooltip("The end angle of the rotation")]
	public float EndAngle = 90.0F;
	
	public enum SideOfHinge
	{
		Left,
		Right,
	}
	public SideOfHinge HingeSide;
	
	[Tooltip("Moving speed of the door/window")]
	public float Speed = 3F;
	[Tooltip("0 = ∞")]
	public int TimesMoveable = 0;

	//PRIVATE SETTINGS
	private int n = 0; //For 'TimesMoveable' loop.
	[HideInInspector] public bool Running = false;

	// Define a start and an end rotation.
	private Quaternion EndRot, StartRot;
	private int State;

	// Create a hinge.
	GameObject hinge;

	// START FUNCTION
	void Start ()
	{
		// Create a hinge.
		hinge = new GameObject();
		hinge.name = "hinge";

		// Calculate Cosine and Sine of initial angle (needed for math calculations).
		float CosDeg = Mathf.Cos ((transform.eulerAngles.y * Mathf.PI) / 180);
		float SinDeg = Mathf.Sin ((transform.eulerAngles.y * Mathf.PI) / 180);

		// HINGE POSITIONING
		// Read transform (position/rotation/scale) of door.
		float PosDoorX = transform.position.x;
		float PosDoorY = transform.position.y;
	    float PosDoorZ = transform.position.z;

		float RotDoorX = transform.localEulerAngles.x;
		float RotDoorY = transform.localEulerAngles.y;
		float RotDoorZ = transform.localEulerAngles.z;
	
		float ScaleDoorX = transform.localScale.x;
		float ScaleDoorY = transform.localScale.y;
		float ScaleDoorZ = transform.localScale.z;

		// Make copy of hinge's position/rotation (placeholder).
		Vector3 HingePosCopy = hinge.transform.position;
		Vector3 HingeRotCopy = hinge.transform.localEulerAngles;

		// Set side of hinge left.
		if (HingeSide == SideOfHinge.Left)
		{
			// Math. (!RADIANS)
			if (transform.localScale.x > transform.localScale.z)
			{
				HingePosCopy.x = (PosDoorX - (ScaleDoorX / 2 * CosDeg));
				HingePosCopy.z = (PosDoorZ + (ScaleDoorX / 2 * SinDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -StartAngle;
				HingeRotCopy.z = RotDoorZ;
			}

			else
			{
				HingePosCopy.x = (PosDoorX + (ScaleDoorZ / 2 * SinDeg));
				HingePosCopy.z = (PosDoorZ + (ScaleDoorZ / 2 * CosDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -StartAngle;
				HingeRotCopy.z = RotDoorZ;
     		}	
		}

		// Set side of hinge right.
		if (HingeSide == SideOfHinge.Right)
		{
			// Math. (!RADIANS)
			if(transform.localScale.x > transform.localScale.z)
			{
				HingePosCopy.x = (PosDoorX + (ScaleDoorX / 2 * CosDeg));
				HingePosCopy.z = (PosDoorZ - (ScaleDoorX / 2 * SinDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -StartAngle;
				HingeRotCopy.z = RotDoorZ;
			}

			else
			{
				HingePosCopy.x = (PosDoorX - (ScaleDoorZ / 2 * SinDeg));
				HingePosCopy.z = (PosDoorZ - (ScaleDoorZ / 2 * CosDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -StartAngle;
				HingeRotCopy.z = RotDoorZ;
			}
		}

		// Hinge positioning.
		hinge.transform.position = HingePosCopy;
		transform.parent = hinge.transform;
		hinge.transform.localEulerAngles = HingeRotCopy;
		
		// DEBUGGING
		//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//cube.transform.position = HingePosCopy;
		//cube.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		//cube.GetComponent<Collider> ().tag = "DebugCube";
		//cube.GetComponent<Renderer>().material.color = Color.black;
		
		// USER ERROR CODES
		if (Mathf.Abs(StartAngle) + Mathf.Abs(EndAngle) == 180 || Mathf.Abs(StartAngle) + Mathf.Abs(EndAngle) > 180)
		{
			UnityEditor.EditorUtility.DisplayDialog ("Error 001","Difference between StartAngle and EndAngle can't be >=180", "Ok", "");
			UnityEditor.EditorApplication.isPlaying = false;
		}

		// Angle defining.
		// Set 'StartRot' to be rotation when door is not yet moved.
		StartRot = Quaternion.Euler (0, -StartAngle, 0);
		// Set 'EndRot' to be the rotation when door is moved.
		EndRot = Quaternion.Euler(0, -EndAngle, 0);
	}

	// UPDATE FUNCTION
	void Update ()
	{

	}

	// OPEN FUNCTION
	public IEnumerator Open ()
    {
		if (n < TimesMoveable || TimesMoveable == 0)
		{
			if (hinge.transform.rotation == (State == 0 ? EndRot : StartRot))
			{
				// Change state from 1 to 0 and back (= change from Endrot to StartRot).
				State ^= 1;
			}

			// Set 'finalRotation' to 'Endrot' when moving and to 'StartRot' when moving back.
			Quaternion finalRotation = ((State == 0) ? EndRot : StartRot);

    	// Make the door rotate until it is fully opened/closed.
    	while (Mathf.Abs(Quaternion.Angle(finalRotation, hinge.transform.rotation)) > 0.01f)
    	{
			Running = true;
			hinge.transform.rotation = Quaternion.Lerp (hinge.transform.rotation, finalRotation, Time.deltaTime * Speed);

      		yield return new WaitForEndOfFrame();
    	}

			Running = false;

			if(TimesMoveable == 0)
			{
				n = 0;
			}

			else n++;

		}
	}

	// GUI FUNCTION
	void OnGUI ()
	{
		// Access InReach variable from raycasting script.
		GameObject Player = GameObject.Find("Player");
		Detection detection = Player.GetComponent<Detection>();

		if (detection.InReach == true)
		{
			GUI.color = Color.white;
			GUI.Box(new Rect(20, 20, 200, 25), "Press 'E' to open/close");
		}
	}
}
