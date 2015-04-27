using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {
	public int points;
	public GameObject particleSys;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Return the point value of this item and play its particle system if it has one
	public int getPoints() {
		if (particleSys != null) {
			particleSys.GetComponent<ParticleSystem>().Play ();
		}
		return points;
	}
}
