using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlybyText : MonoBehaviour {

	private GUIText text;
	private int timer=0;
	public int timerMax=125;

	// Use this for initialization
	void Start () {
		//get this object's GUI text 
		text = GetComponent<GUIText> ();
	}
	
	//Scale the text larget per tick, stop at 100, at max time destroy
	void FixedUpdate () {
		timer++;
		//scale text larger as time increases, after 100 stay at 100
		if (timer < 100) {
			text.fontSize = timer;
		} else {
			text.fontSize = 100;
		}
		//when time is up, destroy itself
		if (timer >= timerMax) {
			Destroy (gameObject);
		}
	}
}
