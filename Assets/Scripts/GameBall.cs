using UnityEngine;
using System.Collections;

//Base class for GameBall objects
public class GameBall : MonoBehaviour {

	public GameController game;
	//speed gained per second by touching boost pad
	Vector3 boostSpeed = new Vector3(0,0,100);
	//max speed the ball can move at
	Vector3 maxSpeed = new Vector3(20,20,20);

	// Update is called once per frame
	protected void Update () {
		//if the speed in any direction exceeds the max speed, cap it at the max speed;
		Vector3 newSpeed = this.GetComponent<Rigidbody> ().velocity;
		if (newSpeed.x > maxSpeed.x) {
			newSpeed.x = maxSpeed.x;
		}
		if (newSpeed.y > maxSpeed.y) {
			newSpeed.y = maxSpeed.y;
		}
		if (newSpeed.z > maxSpeed.z) {
			newSpeed.z = maxSpeed.z;
		}
		this.GetComponent<Rigidbody> ().velocity = newSpeed;
	}

	//On Collision with a trigger
	protected void OnTriggerEnter(Collider col){
		//If the trigger was a scoreboard, add the scoreboards points and delete the ball
		if (col.gameObject.tag == "ScoreBoard") {
			game.AddPoints (col.gameObject.GetComponent<ScoreBoard> ().getPoints ());
			deleteBall ();
		}
	}
	protected void OnTriggerStay(Collider col){
		//If the trigger was a boostpad, boost the ball
		if (col.gameObject.tag == "BoostPad") {
			this.GetComponent<Rigidbody> ().AddForce (boostSpeed);
		}
	}

	//Delete the gameball in the scene
	public void deleteBall(){
		Destroy (this.gameObject);
	}

}
