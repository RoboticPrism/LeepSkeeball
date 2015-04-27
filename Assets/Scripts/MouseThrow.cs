using UnityEngine;
using System.Collections;

public class MouseThrow : MonoBehaviour {

	public GameObject Ball;
	public float distance = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
		Vector3 loc = Camera.main.ScreenToWorldPoint(position);
		Ball.transform.position = loc;
		Ball.transform.LookAt(position);    
		Debug.Log(position);    
		Ball.GetComponent<Rigidbody>().AddForce(Ball.transform.forward * 1000);
	}
}
