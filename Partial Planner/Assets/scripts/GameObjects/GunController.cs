using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	//private Vector3 gunOffest;
	private bool isHolding;
	private bool isDrawn;
	private GameObject player;

	public GameObject holder;
	// Use this for initialization
	void Start () {
		isHolding = false;
		isDrawn = false;
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (isHolding) {
			transform.position = holder.transform.position;
			if (isDrawn) 
				transform.eulerAngles = new Vector3(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z);
			else
				transform.rotation = holder.transform.rotation;
		}
	}

	public bool IsDrawn() {

		return isDrawn;
	}

	public void SetIsDrawn(bool drawn) {

		isDrawn = drawn;
	}

	public void SetIsHolding(bool holding) {

		isHolding = holding;
	}

    public void SetHolder(GameObject hold)
    {
		this.SetIsHolding (true);
        holder = hold;
    }
}
