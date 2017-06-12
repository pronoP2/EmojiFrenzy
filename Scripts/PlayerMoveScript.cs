using UnityEngine;
using System.Collections;
using StartApp;

public class PlayerMoveScript : MonoBehaviour {

	public Vector2 speed = new Vector2(50, 50);
	private Vector2 movement;

	Vector2 startPoint = new Vector2 (0,0);

	private float offset_x = 0;
	private float offset_y = 0;
	private float dist;
	private float leftBorder;
	private float rightBorder;
	private float topBorder;
	private float bottomBorder;
	
	public Sprite sprite_dull;
	public Sprite sprite_smile;
	public Sprite sprite_cry;
	public Sprite sprite_shield;

	public GUIStyle label_hint1;
	public GUIStyle label_hint2;
	public GUIStyle label_continue;
	public Texture hint2_image;
	public Texture hint3_image;
	public Texture hint4_image;
		
	public GUIStyle label_pause;
	public GUIStyle bg_scoreBox;
	public GUIStyle style_home_button;
	public GUIStyle style_play_button;

	GameObject enemy1;
	GameObject enemy2;
	GameObject enemy3;

	PlayerScript player;

	float touch_offset;

	float hint1_x;
	float hint1_y;
	float hint1_w;
	float hint1_h;

	float hint2_x;
	float hint2_y;
	float hint2_w;
	float hint2_h;

	float hint2_image_x;
	float hint2_image_y;
	float hint2_image_w;
	float hint2_image_h;

	float hint3_x;
	float hint3_y;
	float hint3_w;
	float hint3_h;

	float hint3_image_x;
	float hint3_image_y;
	float hint3_image_w;
	float hint3_image_h;

	float hint4_x;
	float hint4_y;
	float hint4_w;
	float hint4_h;
	
	float hint4_image_x;
	float hint4_image_y;
	float hint4_image_w;
	float hint4_image_h;

	float continue_x;
	float continue_y;
	float continue_w;
	float continue_h;

	string hint1;
	string hint2;
	string hint3;
	string hint4;
	string str_continue;

	int direction_x;
	int direction_y;


	// Use this for initialization
	void Start () {
		Debug.Log ("raj Screen Width: "+Screen.width);
		Debug.Log ("raj Screen Height: "+Screen.height);
		Debug.Log ("raj Screen DPI: "+Screen.dpi);
		Debug.Log ("raj Screen Size: "+Helper.getDeviceSize()+" inches");

		label_pause.fontSize = Mathf.FloorToInt((label_pause.fontSize * Screen.dpi)/360);

		enemy1 = GameObject.Find ("enemy1");
		enemy2 = GameObject.Find ("enemy2");
		enemy3 = GameObject.Find ("enemy3");

		hint1 = "Touch and move the finger any where on the screen to move the smiley.";
		hint2 = "Taking the pill will make smiley Slim.";
		hint3 = "Eating burger will make smiley Fat.";
		hint4 = "Taking shield will make smiley Immortal.";
		str_continue = "Tap to Continue";

		label_hint1.fontSize = Mathf.FloorToInt((label_hint1.fontSize * Screen.dpi)/320);
		label_hint2.fontSize = Mathf.FloorToInt((label_hint2.fontSize * Screen.dpi)/320);
		label_continue.fontSize = Mathf.FloorToInt((label_continue.fontSize * Screen.dpi)/320);

		offset_x = transform.GetComponent<Renderer>().bounds.size.x / 2;
		offset_y = transform.GetComponent<Renderer>().bounds.size.y / 2;

		dist = (transform.position - Camera.main.transform.position).z;
		
		leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).x + offset_x;
		
		rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
			).x - offset_x;
		
		topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).y + offset_y;
		
		bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
			).y - offset_y;

		transform.position = new Vector3 (rightBorder / 2, bottomBorder / 2, 0);

		
		sprite_smile = transform.GetComponent <SpriteRenderer>().sprite;
		transform.GetComponent <SpriteRenderer>().sprite = sprite_dull;

		player = transform.GetComponent<PlayerScript> ();

		//touch_offset = getTouchOffset ();

		adjustHints ();
		if(!Helper.isRetry)
			freezeGame ();

	}
	
	// Update is called once per frame
	void Update () {

		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		// 4 - Movement per direction
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);

		if (GetComponent<Rigidbody2D>().gravityScale <= 0) 
		{
			if(player.isShieldPresent)
				transform.GetComponent <SpriteRenderer> ().sprite = sprite_shield;
			else
			{
				if (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && !Helper.gameOver)
					transform.GetComponent <SpriteRenderer> ().sprite = sprite_smile;
				else
					transform.GetComponent <SpriteRenderer> ().sprite = sprite_dull;
			}
		}
		else
			transform.GetComponent <SpriteRenderer>().sprite = sprite_cry;
		
		
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
			transform.position.z
			);
		if (GetComponent<Rigidbody2D>().gravityScale > 0 && transform.position.y <= topBorder) 
		{
			Invoke("loadMainMenu", 0.5f);
			//Invoke("loadGameOverScene", 0.5f);
		}

		if (Helper.isGameFreezed && !Helper.isGamePaused) 
		{
			if(Input.touchCount > 0 || Input.GetMouseButtonDown(0))
				unFreezeGame();
		}

		if (Input.touchCount > 0) {

						if (Input.GetTouch (0).phase == TouchPhase.Began) {
								startPoint.x = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position).x;
								startPoint.y = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position).y;
						}

						if ((Input.GetTouch (0).phase == TouchPhase.Moved && GetComponent<Rigidbody2D>().gravityScale <= 0) && 
								!Helper.gameOver && !Helper.isGameFreezed) {
								float dx = 0;
								float dy = 0;
								Vector3 wp = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
								dx = wp.x - startPoint.x;
								dy = wp.y - startPoint.y;
								startPoint.x = wp.x;
								startPoint.y = wp.y;
								Vector2 pos = new Vector2 (transform.position.x + dx, transform.position.y + dy);
								transform.position = pos;
						}
				}


		//getBounds ();
	
	}

	void moveSmiley()
	{
		Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		Vector2 touchPos = new Vector2(wp.x, wp.y);

		var hit = Physics2D.OverlapPoint(touchPos);
		if (hit) 
		{
			transform.position = touchPos;
		}
	}
	public void loadMainMenu()
	{
		GameObject enemy1 = GameObject.Find ("enemy1");
		GameObject enemy2 = GameObject.Find ("enemy2");
		GameObject enemy3 = GameObject.Find ("enemy3");
		Destroy(enemy1);
		Destroy(enemy2);
		if(enemy3 != null)
			Destroy(enemy3);
		//Destroy(gameObject);
		//Destroy (bloodTransform.gameObject);
		Application.LoadLevel("MainMenu");

	}
	public void loadGameOverScene()
	{
		Destroy(enemy1);
		Destroy(enemy2);
		//Destroy(gameObject);
		Application.LoadLevel("GameOver");
	}
	void FixedUpdate()
	{
		// 5 - Move the game object
		GetComponent<Rigidbody2D>().velocity = movement;

		if (Application.platform == RuntimePlatform.Android) 
		{
			if (Input.GetKey (KeyCode.Escape)) 
			{
				pauseGame();
				/*if(!Helper.gameOver)
				{
					Helper.isGamePaused = true;
					GetComponent<PlayerScript>().stopMusic();
					freezeGame();
					//displayAdsBanner();
				}*/
			}
		}
	}
	public void pauseGame()
	{
		if(!Helper.gameOver)
		{
			Helper.isGamePaused = true;
			GetComponent<PlayerScript>().stopMusic();
			freezeGame();
			//displayAdsBanner();
		}
	}
	void OnApplicationPause()
	{
		Debug.Log ("raj Game Paused........");
		pauseGame ();
	}
	public void getBounds()
	{
		offset_x = transform.GetComponent<Renderer>().bounds.size.x / 2;
		offset_y = transform.GetComponent<Renderer>().bounds.size.y / 2;
		
		dist = (transform.position - Camera.main.transform.position).z;
		
		leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).x + offset_x;
		
		rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
			).x - offset_x;
		
		topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).y + offset_y;
		
		bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
			).y - offset_y;
	}

	void OnGUI()
	{
		if (Helper.isGameFreezed && !Helper.isGamePaused)
		{
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "");	
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "");

			GUI.Label (new Rect (hint1_x, hint1_y, hint1_w, hint1_h), hint1, label_hint1);

			GUI.DrawTexture (new Rect (hint2_image_x, hint2_image_y, hint2_image_w, hint2_image_h), hint2_image);
			GUI.Label (new Rect (hint2_x, hint2_y, hint2_w, hint2_h), hint2, label_hint2);

			GUI.DrawTexture (new Rect (hint3_image_x, hint3_image_y, hint3_image_w, hint3_image_h), hint3_image);
			GUI.Label (new Rect (hint3_x, hint3_y, hint3_w, hint3_h), hint3, label_hint2);

			GUI.DrawTexture (new Rect (hint4_image_x, hint4_image_y, hint4_image_w, hint4_image_h), hint4_image);
			GUI.Label (new Rect (hint4_x, hint4_y, hint4_w, hint4_h), hint4, label_hint2);

			GUI.Label (new Rect (continue_x, continue_y, continue_w, continue_h), str_continue, label_continue);
		}
		else if (Helper.isGameFreezed && Helper.isGamePaused)
		{
			float box_width = (Screen.width - 50)*0.75f;
			float box_height = (Screen.height/2)*0.75f;
			float button_width = (box_width/3)*0.75f;
			float button_height = button_width;
			
			float label_score_x = 0;
			float label_score_y = 40;
			float label_score_w = box_width;
			float label_score_h = label_pause.fontSize;	

			float button_home_y = label_score_y + (label_score_h * 2);
			float button_play_y = button_home_y;
			
			float button_home_x = (box_width/2 - button_width)/2;//(box_width - button_width)/2;
			float button_play_x = box_width/2 + (box_width/2 - button_width)/2;

			box_height = (2*label_score_y) + (2*label_score_h) + (button_height);

			//layout start
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");	
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

			GUI.BeginGroup(new Rect((Screen.width - box_width)/2 ,(Screen.height - box_height)/2, box_width, box_height));
			//GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));	
			//the menu background box
			
			
			GUI.Box(new Rect(0, 0, box_width, box_height), "",bg_scoreBox);	
			
			//GUI.Label(new Rect(0,10,box_width, 40),"Game Over",label_gameover);
			
			GUI.Label(new Rect(label_score_x,label_score_y,label_score_w, label_score_h),"Pause",label_pause);
			
			//GUI.DrawTexture (new Rect (button_width, box_width/2, box_width/6, box_width/6), homeTexture);
			if (GUI.Button (new Rect (button_home_x, button_home_y, button_width, button_height), "", style_home_button))
			{
				//removeAdsBanner ();
				Helper.isGamePaused = false;
				MainMenu.current_score = 0;
				loadMainMenu();
			}
			
			//GUI.DrawTexture (new Rect (box_width - button_width*2, box_width/2, box_width/6, box_width/6), retryTexture);
			if (GUI.Button (new Rect (button_play_x, button_play_y, button_width, button_height), "", style_play_button)) 
			{
				//removeAdsBanner ();
				PlayerScript sound = GetComponent<PlayerScript>();
				if(sound.sound_option == 1)
					sound.playMusic();
				unFreezeGame();
			}				
			//layout end
			GUI.EndGroup();
		}


	}

	void adjustHints()
	{
		hint1_x = 10;
		hint1_y = Screen.height/2;
		hint1_w = Screen.width - (2*hint1_x);
		hint1_h = Mathf.CeilToInt ((label_hint1.fontSize * hint1.Length) / Screen.width)* label_hint1.fontSize;

		hint2_image_x = 20;
		hint2_image_y = hint1_y + hint1_h;
		hint2_image_w = Mathf.FloorToInt((70 * Screen.dpi)/320);
		hint2_image_h = Mathf.FloorToInt((70 * Screen.dpi)/320);

		hint2_x = hint2_image_x + hint2_image_w + 10;
		hint2_y = hint1_y + hint1_h;
		hint2_w = Screen.width - hint2_image_w - hint2_image_x - 10;
		hint2_h = (Mathf.CeilToInt((label_hint2.fontSize * hint2.Length)/Screen.width) + 1)* label_hint2.fontSize;

		hint3_image_x = 20;
		hint3_image_y = hint2_y + hint2_h;
		hint3_image_w = Mathf.FloorToInt((70 * Screen.dpi)/320);
		hint3_image_h = Mathf.FloorToInt((70 * Screen.dpi)/320);

		hint3_x = hint3_image_x + hint3_image_w + 10;
		hint3_y = hint2_y + hint2_h;
		hint3_w = Screen.width - hint3_image_w - hint3_image_x - 10;
		hint3_h = (Mathf.CeilToInt((label_hint2.fontSize * hint3.Length)/Screen.width) + 1) * label_hint2.fontSize;

		hint4_image_x = 20;
		hint4_image_y = hint3_y + hint3_h;
		hint4_image_w = Mathf.FloorToInt((70 * Screen.dpi)/320);
		hint4_image_h = Mathf.FloorToInt((70 * Screen.dpi)/320);
		
		hint4_x = hint4_image_x + hint4_image_w + 10;
		hint4_y = hint3_y + hint3_h;
		hint4_w = Screen.width - hint4_image_w - hint4_image_x - 10;
		hint4_h = (Mathf.CeilToInt((label_hint2.fontSize * hint4.Length)/Screen.width) + 1) * label_hint2.fontSize;

		continue_x = 10;
		continue_y = hint4_y + 2 * hint4_h;
		continue_w = Screen.width - (2*continue_x);
		continue_h = Mathf.CeilToInt((label_continue.fontSize * str_continue.Length)/Screen.width) * label_continue.fontSize;

		hint1_y = (Screen.height - (hint1_h + hint2_h + hint3_h + (2*hint4_h) + continue_h)) / 2;

		hint2_image_y = hint1_y + hint1_h + (label_hint2.fontSize/2);
		hint2_y = hint1_y + hint1_h;

		hint3_image_y = hint2_y + hint2_h + (label_hint2.fontSize/2);
		hint3_y = hint2_y + hint2_h;		

		hint4_image_y = hint3_y + hint3_h + (label_hint2.fontSize/2);
		hint4_y = hint3_y + hint3_h;

		continue_y = hint4_y + 2 * hint4_h;

	}

	void freezeGame()
	{
		Debug.Log ("Game Freezed...");
		Helper.isGameFreezed = true;
		//enemy1.gameObject.SetActive (false);
		//enemy2.gameObject.SetActive (false);
		enemy1.GetComponent<EnemyScript> ().enabled = false;
		enemy2.GetComponent<EnemyScript> ().enabled = false;
		enemy1.GetComponent<EnemyMoveScript> ().enabled = false;
		enemy2.GetComponent<EnemyMoveScript> ().enabled = false;
		if (enemy3 != null) {
						enemy3.GetComponent<EnemyMoveScript> ().enabled = false;
						enemy3.GetComponent<EnemyScript> ().enabled = false;
				}
		enemy1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		enemy2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		if (enemy3 != null) {
						enemy3.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				}
		transform.GetComponent<PlayerScript> ().enabled = false;

	}
	void unFreezeGame()
	{
		Debug.Log ("Game unFreezed...");
		Helper.isGameFreezed = false;
		enemy1.GetComponent<EnemyScript> ().enabled = true;
		enemy2.GetComponent<EnemyScript> ().enabled = true;
		enemy1.GetComponent<EnemyMoveScript> ().enabled = true;
		enemy2.GetComponent<EnemyMoveScript> ().enabled = true;
		if (enemy3 != null) {
						enemy3.GetComponent<EnemyMoveScript> ().enabled = true;
						enemy3.GetComponent<EnemyScript> ().enabled = true;
				}
		transform.GetComponent<PlayerScript> ().enabled = true;
		if(!Helper.isGamePaused)
			Helper.startTime = Mathf.CeilToInt(Time.time);
		else
			Helper.startTime = Mathf.CeilToInt(Time.time) - enemy1.GetComponent<EnemyScript> ().roundedRestSeconds;
		Helper.isGamePaused = false;
	}
	void displayAdsBanner()
	{
		#if UNITY_ANDROID			
		
		//StartAppWrapper.addBanner (
		//	StartAppWrapper.BannerType.AUTOMATIC,
		//	StartAppWrapper.BannerPosition.BOTTOM);
		
		//StartAppWrapper.loadAd ();
		
		#endif
	}
	
	void removeAdsBanner()
	{
		#if UNITY_ANDROID
		//StartAppWrapper.removeBanner (StartAppWrapper.BannerPosition.BOTTOM);
		
		#endif
	}
}
