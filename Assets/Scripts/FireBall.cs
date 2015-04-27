using UnityEngine;
using System.Collections;

public class FireBall : GameBall {

	// Update is called once per frame
	void Update () {
		//run the update of the base object
		base.Update ();
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "ScoreBoard") {
			//On collision with scoreboard, reward double points
			game.AddPoints(col.gameObject.GetComponent<ScoreBoard>().getPoints()*2);
			deleteBall();
		}
	}
}
