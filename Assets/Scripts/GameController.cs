using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


	//INTERNAL VARIABLES

	//respawn delay vars
	int spawnTimer=0;
	int spawnTimerMax=100;

	//endgame delay vars
	int resetTimer = 0;
	int resetTimerMax=300;

	//Multiplayer vars
	bool p1turn=true;

	//Score
	int p1score=0;
	int p2score=0;

	//Ball Count
	int p1balls=5;
	int p2balls=5;

	//Debugging vars
	bool ready = true;

	//Current GameBall in scene (only one at a time)
	GameBall ball;

	//EXTERNAL VARIABLES
	
	//Ball Spawn locations
	public Transform p1spawn;
	public Transform p2spawn;

	//Sprite Objects to turn on and off
	public GameObject p1sprite;
	public GameObject p2sprite;

	//Text Objects to update 
	public GUIText player1ScoreText;
	public GUIText player2ScoreText;

	//Sprite for Ball Counter
	public Texture ballSprite;

	//PROTOTYPES

	//Turn Start prototypes to clone
	public GameObject p1turnstart;
	public GameObject p2turnstart;

	//victory text porotypes to clone
	public GameObject p1wins;
	public GameObject p2wins;
	public GameObject tie;

	//victory fireworks prototype
	public GameObject fireworks;

	//Ball prototypes to clone
	public GameBall BallPrefab;
	public GameBall FireBallPrefab;
	public GameBall BombBallPrefab;

	// Use this for initialization
	void Start () {
		//Create ball, set vars to p1's turn
		spawnBall ();
		p1balls--;
		p2sprite.SetActive (false);
		//display text
		Instantiate (p1turnstart);
	}
	
	// Update is called once per frame
	void Update () {

		//update score
		player1ScoreText.text = "P1 Score: " + p1score;
		player2ScoreText.text = "P2 Score: " + p2score;

		//mouse click to emulate throw
		if (Input.GetMouseButtonDown (0)) {
			//if the ball is at spawn, next click throws the ball
			if(ready){
				throwBall();
				ready=false;
			}
			//if the ball is in mid air (not at spawn) then next click returns it to spawn
			else{
				ball.deleteBall();
				spawnBall();
				p1turn=!p1turn;
				ready=true;
			}
		}
	}

	//updates at a fixed interva
	void FixedUpdate(){
		//if there are no objects in the scene labeled gameball
		if (GameObject.FindGameObjectsWithTag ("GameBall").Length==0) {
			//if both players are out of balls, begin the end
			if(p1balls<=0&&p2balls<=0){
				//turn off UI items
				player1ScoreText.gameObject.SetActive(false);
				player2ScoreText.gameObject.SetActive(false);
				p1sprite.SetActive(false);
				p2sprite.SetActive(false);
				//draw win screen
				if(resetTimer==30){
					drawWinner();
					Instantiate(fireworks, new Vector3(-10,6,36), Quaternion.Euler(new Vector3(0,0,0)));
					Instantiate(fireworks, new Vector3(10,6,36), Quaternion.Euler(new Vector3(0,0,0)));
					resetTimer++;
				}
				//reset level
				if(resetTimer>=resetTimerMax){
					Application.LoadLevel(Application.loadedLevelName);
				}
				//increment timer
				else{
					resetTimer++;
				}
			}
			//otherwise start the respawn timer, if the timer is up, spawn the ball
			else{
				//at end of timer, spawn new ball, swap turns
				if(spawnTimer>=spawnTimerMax){
					spawnBall();
					//remove a ball from whoever's turn it is
					if(p1turn){
						p1balls--;
					}
					else{
						p2balls--;
					}
					spawnTimer=0;
				}
				//if timer is half way though, spawn the turn text
				else if(spawnTimer==spawnTimerMax/2){
					if(p1turn){
						Instantiate(p1turnstart);
					}
					else{
						Instantiate(p2turnstart);
					}
					spawnTimer++;
				}
				//otherwise incriment timer
				else{
					spawnTimer++;
				}
			}
		}
	}

	//calculates who won, draws the winner or a tie
	void drawWinner(){
		//p1 wins
		if (p1score > p2score) {
			//Create p1 wins text
			Instantiate(p1wins);
		} 
		//p2 wins
		else if (p1score < p2score) {
			//Create p2 wins text
			Instantiate(p2wins);
		}
		//tie
		else {
			//Create tie text
			Instantiate(tie);
		}

	}

	//Add points to the current player and end their turn (called by ball on destruction)
	public void AddPoints(int points) {
		if (p1turn) {
			p1score += points;
			//turn sprites for each players turn on or off
			p1sprite.SetActive(false);
			p2sprite.SetActive(true);
		}
		else {
			p2score += points;
			//turn sprites for each players turn on or off
			p1sprite.SetActive(true);
			p2sprite.SetActive(false);
		}
		//swap turns
		p1turn=!p1turn;
		ready = true;
	}

	//OnGUI draw each players number of balls left
	void OnGUI(){
		drawBallCount (40, 100, p1balls);
		drawBallCount (670, 100, p2balls);
	}

	//Draws number of balls as sprites, stacking downwards from an xy coordinate within 800x600 screen space
	void drawBallCount(int x, int y, int balls){
		for (int i = 0; i < balls; i++) {
			GUI.DrawTexture (ResizeGUI(new Rect(x,y+65*i,60,60)), ballSprite, ScaleMode.ScaleToFit, true);
		}
	}

	//Helper method to resize GUI based upon screen resolution
	Rect ResizeGUI(Rect rect){
		//Set width to % out of 800, then multiply times true screen width
		float rectWidth = (rect.width / 800) * Screen.width;
		//Set height to % out of 600, then multiply times true screen height
		float rectHeight = (rect.height / 600) * Screen.height;
		//Set x to % out of 800, then multiply times true screen width
		float rectX = (rect.x / 800) * Screen.width;
		//Set y to % out of 600, then multiply times true screen height
		float rectY = (rect.y / 600) * Screen.height;
		
		return new Rect(rectX,rectY,rectWidth,rectHeight);
	}

	//Creates a new ball from given ball types randomly
	void spawnBall(){

		//randomizer for ball spawns
		int r = Random.Range(0,5);

		//choose spawn point based on who's turn it is
		Transform spawnPoint;

		if (p1turn) {
			spawnPoint = p1spawn;
		} 
		else {
			spawnPoint=p2spawn;
		}

		//Spawn bomb ball
		if(r==0){
			ball = (GameBall)Instantiate(BombBallPrefab, spawnPoint.position, Quaternion.Euler(new Vector3(0,0,0)));
		}
		//Spawn fire ball
		else if(r==1){
			ball = (GameBall)Instantiate(FireBallPrefab, spawnPoint.position, Quaternion.Euler(new Vector3(0,0,0)));
		}
		//spawn normal ball
		else{
			ball = (GameBall)Instantiate(BallPrefab, spawnPoint.position, Quaternion.Euler(new Vector3(0,0,0)));
		}
		//set the game controller of the ball we made (used to send score back)
		ball.game = this;
	}

	//For mouse controls, throws the ball with a set velocity
	void throwBall(){
		//Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10));
		//Vector3 loc = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		ball.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		ball.transform.LookAt (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10)));  
		ball.GetComponent<Rigidbody>().AddForce (transform.forward * 2000);
	}
	
}
