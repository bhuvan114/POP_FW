using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	//private Vector3 gunOffest;
	private bool isHolding;
	private Rigidbody rb;

	public GameObject holder;
	// Use this for initialization
	void Start () {
		isHolding = false;
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isHolding) {
			transform.position = holder.transform.position;
			transform.rotation = holder.transform.rotation;
		}
	}

	public void SetIsHolding(bool holding) {

		isHolding = holding;
	}

	public void SetGravity(bool status) {

		rb.useGravity = status;
	}
}
