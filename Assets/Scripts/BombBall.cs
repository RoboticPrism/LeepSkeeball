using UnityEngine;
using System.Collections;

public class BombBall : GameBall {

	//Prototype explosion to spawn
	public GameObject explosion;

	//This object's spark particle system, off by default
	public GameObject sparks;
	
	public int life = 300;
	public bool life_tick = false;

	// Use this for initialization
	void Start () {
		//Turn sparks off initially
		sparks.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	void OnCollisionEnter(Collision col){
		//If colliding with a hand, turn the sparks on and enable to countdown
		if (col.gameObject.tag == "Hand") {
			life_tick = true;
			sparks.SetActive (true);
		}
	}

	//If countdown is enabled, begin removing life per tick, if life is out, destroy object and create an explosion
	void FixedUpdate() {
		if (life_tick) {
			life--;
			if (life < 0) {
				Instantiate (explosion, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
				game.AddPoints(0);
				Destroy (gameObject);
			}
		}
	}
}
