using UnityEngine;
using System.Collections;

public class OneTimeParticle : MonoBehaviour {

	public ParticleSystem p;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//if the particle system is done, destroy this object
		if (!p.IsAlive ()) {
			Destroy(gameObject);
		}
	}
}
