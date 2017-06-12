using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;
using StartApp;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayerScript : MonoBehaviour {

	public Transform plus_bubble_Prefab;
	public Transform minus_bubble_Prefab;
	public Transform shield_bubble_Prefab;
	public Transform blood_Prefab;

	public bool showPopUp = false;
	public GUIStyle style_Audio_button;

	public AudioSource BGSound;
	public AudioSource DeathSound;

	public Texture sound_on;
	public Texture sound_off;
	
	public Sprite sprite_cry;
	public Sprite sprite_shield;

	Texture sound_on_off;

	public Texture logoTexture;
	public Texture homeTexture;
	public Texture retryTexture;
	public Texture score_image;

	public GUIStyle label_gameover;
	public GUIStyle label_score;
	public GUIStyle label_scoreBest;
	public GUIStyle bg_scoreBox;
	public GUIStyle style_home_button;
	public GUIStyle style_retry_button;

	public int sound_option = 0;
	int best_score = 0;

	Vector3 original_scale;
	float upperLimit_scale_x;
	float lowerLimit_scale_x;
	float original_radius = 0;

	public float bubble_generation_time = 10.0f;
	float var_bubble_generation_time;
	public float bubble_Init_generation_time = 10.0f;
	public float player_power_hold_time = 10f;
	float var_player_power_hold_time;
	public float player_shield_hold_time = 10f;
	float var_player_shield_hold_time;
	public bool isShieldPresent = false;
	static bool start_bubble_generation = false;

	int plus_bubble = 0;
	int minus_bubble = 1;
	int shield_bubble = 2;
	int bubble_collided;

	float touchSensitivity = 5.0f;
	public GUIStyle sliderStyle;
	public GUIStyle thumbStyle;
	// Use this for initialization

	bool displaygameOverPopup;
	Transform bloodTransform;

	int noOfgamePlays;


	void Start () {

		displaygameOverPopup = false;

		original_radius = gameObject.GetComponent<CircleCollider2D> ().radius;
		original_scale = transform.localScale;
		upperLimit_scale_x = original_scale.x + 0.5f;
		lowerLimit_scale_x = original_scale.x - 0.5f;

		label_score.fontSize = Mathf.FloorToInt((label_score.fontSize * Screen.dpi)/360);
		label_scoreBest.fontSize = Mathf.FloorToInt((label_scoreBest.fontSize * Screen.dpi)/340);

		sound_option = SaveLoadScript.Load_sound ();
		if (sound_option == 0) 
		{
			sound_on_off = sound_off;
			BGSound.loop = false;
			if(isMusicPlaying())
				BGSound.Stop();
					
		}
		else 
		{
			sound_on_off = sound_on;
			BGSound.loop = true;
			if(!isMusicPlaying())
				BGSound.Play();
		}
		var_bubble_generation_time = bubble_generation_time;
		var_player_power_hold_time = player_power_hold_time;
		var_player_shield_hold_time = player_shield_hold_time;
		Helper.show_power_time = false;

		Invoke ("startGeneratingBubbles", bubble_Init_generation_time - bubble_generation_time);
		Debug.Log ("Data saved: "+SaveLoadScript.Load());

		noOfgamePlays = SaveLoadScript.Load_no_of_game_plays ();
		noOfgamePlays += 1;

		#if UNITY_ANDROID		
		//StartAppWrapper.loadAd ();
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log("radius: "+gameObject.GetComponent<CircleCollider2D>().renderer.collider2D.bounds.size.x);

		if (start_bubble_generation) 
		{
			if(var_bubble_generation_time > 0)
				var_bubble_generation_time -= Time.deltaTime;
			else
			{
				var_bubble_generation_time = bubble_generation_time;
				generateBubble(Random.Range(0,3));
			}
		}

		if (var_player_power_hold_time > 0) 
		{
			var_player_power_hold_time -= Time.deltaTime;
			Helper.power_hold_time_disp = var_player_power_hold_time;
		}
		else
		{
			transform.localScale = original_scale;
		}

		if (isShieldPresent)
		{
			if(var_player_shield_hold_time > 0)
			{
				var_player_shield_hold_time -= Time.deltaTime;
				Helper.power_hold_time_disp = var_player_shield_hold_time;
			}
			else
			{
				isShieldPresent = false;
			}
		}
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript> ();
		if(enemy != null)
		{
			if(GetComponent<Rigidbody2D>().gravityScale <= 0)
			{
				if(!Helper.gameOver && !Helper.isGamePaused)
					Handheld.Vibrate();
				//Destroy (transform.gameObject);
				if(!isShieldPresent)
				{
					if(enemy.roundedRestSeconds > SaveLoadScript.Load())
					{
						best_score = enemy.roundedRestSeconds;
						SaveLoadScript.Save(enemy.roundedRestSeconds);
					}
					else
						best_score = SaveLoadScript.Load();

					MainMenu.current_score = enemy.roundedRestSeconds;

					if(isMusicPlaying())
						stopMusic();
					if(sound_option == 1 && !Helper.gameOver)// && rigidbody2D.gravityScale <= 0)
						playDeathSound();

					//rigidbody2D.gravityScale = 20f;//raj
					//rigidbody2D.fixedAngle = false;//raj
					//stopGeneratingBubbles();
					//displayAdsBanner();
					//Helper.gameOver = true;
					bloodTransform = Instantiate (blood_Prefab) as Transform;
					bloodTransform.position = transform.position;
					gameOver();
				}
			}
			//transform.GetComponent <SpriteRenderer>().sprite = sprite_cry;

			//rigidbody2D.gravityScale = 20f;
			//rigidbody2D.fixedAngle = false;
		}
	}
	void OnTriggerEnter2D(Collider2D bubble)
	{
		if (GetComponent<Rigidbody2D>().gravityScale <= 0) 
		{
			BubbleScript bubbleScript = bubble.gameObject.GetComponent<BubbleScript> ();
			if (bubbleScript != null) 
			{
				if (bubbleScript.isMinusBubble) 
				{
					var_player_power_hold_time = player_power_hold_time;
					Helper.show_power_time = true;
					Helper.power_hold_time_disp = var_player_power_hold_time;
					bubble_collided = minus_bubble;
					increasePlayerSize ();
				}
				if (bubbleScript.isPlusBubble) 
				{
					var_player_power_hold_time = player_power_hold_time;
					Helper.show_power_time = true;
					Helper.power_hold_time_disp = var_player_power_hold_time;
					bubble_collided = plus_bubble;
					decreasePlayerSize ();
				}
				if (bubbleScript.isShieldBubble) 
				{
					var_player_shield_hold_time = player_shield_hold_time;
					Helper.show_power_time = true;
					Helper.power_hold_time_disp = var_player_shield_hold_time;
					bubble_collided = shield_bubble;
					isShieldPresent = true;
				}
			}
			Destroy (bubble.gameObject);
		}
		Debug.Log ("bubble name: " + bubble.ToString());
	}

	void gameOver()
	{
		stopGeneratingBubbles();
		Helper.gameOver = true;
		Invoke("displaygameOverScreen",1f);
		if (noOfgamePlays == 5) 
		{
			noOfgamePlays = 0;
			displayInterstitialAd ();
		}
		else
		{
			displayAdsBanner ();
		}
		SaveLoadScript.Save_no_of_game_plays (noOfgamePlays);
	}
	void increasePlayerSize()
	{
		if (!Helper.gameOver) {
						Vector3 scale = transform.localScale;
						scale.x += 0.25f;
						scale.y += 0.25f;
						if (scale.x <= upperLimit_scale_x)
								transform.localScale = scale;
			transform.GetComponent<PlayerMoveScript>().getBounds();
				}
	}
	void decreasePlayerSize()
	{
		if (!Helper.gameOver) {
						Vector3 scale = transform.localScale;
						scale.x -= 0.25f;
						scale.y -= 0.25f;
						if (scale.x >= lowerLimit_scale_x)
								transform.localScale = scale;
			transform.GetComponent<PlayerMoveScript>().getBounds();
				}
	}
	void OnGUI()
	{
		if (!Helper.gameOver) 
		{
				GUI.DrawTexture (new Rect (Screen.width - Screen.width / 10, 10, Screen.width / 10, Screen.width / 10), sound_on_off);
				if (GUI.Button (new Rect (Screen.width - Screen.width / 10, 10, Screen.width / 10, Screen.width / 10), "", style_Audio_button)) {
						if (isMusicPlaying ()) {
								sound_on_off = sound_off;
								sound_option = 0;
								stopMusic ();
								SaveLoadScript.Save_sound (sound_option);
						} else {
								sound_on_off = sound_on;
								sound_option = 1;
								playMusic ();
								SaveLoadScript.Save_sound (sound_option);
						}
				}
		}
		if(Helper.gameOver && displaygameOverPopup)
		{
			float box_width = Screen.width - 50;
			float box_height = Screen.height/2;
			float button_width = box_width/6;
			float button_height = button_width;
			
			float label_score_x = 0;
			float label_score_y = 40;
			float label_score_w = box_width;
			float label_score_h = label_score.fontSize;
			
			float label_Bscore_x = 0;
			float label_Bscore_y = label_score_y + label_score_h;
			float label_Bscore_w = box_width;
			float label_Bscore_h = label_scoreBest.fontSize;

			float image_x = 10;
			float image_y = label_Bscore_y + label_Bscore_h + 10;
			float image_w = box_width/3;
			float image_h;
			


			box_height = label_score_y + label_score_h + label_Bscore_h + (2 * button_height) + 80;

			image_h = box_height/2;
			image_w = image_h;
			image_x = ((box_width/2) - image_w)/2;

			float button_home_y = label_Bscore_y + label_Bscore_h + ((box_height - (label_Bscore_y + label_Bscore_h)) - button_height)/2;
			float button_retry_y = button_home_y;
			
			float button_home_x = box_width/2;//(box_width - button_width)/2;
			float button_retry_x = button_home_x + button_width + button_width/2;
			//layout start
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");	
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");	

			GUI.BeginGroup(new Rect((Screen.width - box_width)/2 ,(Screen.height - box_height)/2, box_width, box_height));
			//GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));	
			//the menu background box

			
			GUI.Box(new Rect(0, 0, box_width, box_height), "",bg_scoreBox);	
			
			//GUI.Label(new Rect(0,10,box_width, 40),"Game Over",label_gameover);
			
			GUI.Label(new Rect(label_score_x,label_score_y,label_score_w, label_score_h),"Your Score: "+MainMenu.current_score,label_score);
			GUI.Label(new Rect(label_Bscore_x,label_Bscore_y,label_Bscore_w, label_Bscore_h),"Best Score: "+best_score,label_scoreBest);

			GUI.DrawTexture (new Rect (image_x, image_y , image_w, image_h), score_image);

			//GUI.DrawTexture (new Rect (button_width, box_width/2, box_width/6, box_width/6), homeTexture);
			if (GUI.Button (new Rect (button_home_x, button_home_y, button_width, button_height), "", style_home_button))
			{
				loadMainMenu();
			}
			
			//GUI.DrawTexture (new Rect (box_width - button_width*2, box_width/2, box_width/6, box_width/6), retryTexture);
			if (GUI.Button (new Rect (button_retry_x, button_retry_y, button_width, button_height), "", style_retry_button)) 
			{
				Helper.isRetry = true;
				restartGame();
			}		
			
			//layout end
			GUI.EndGroup();
		}
		/*thumbStyle.fixedHeight = Screen.height / 40;
		touchSensitivity = GUI.VerticalSlider (new Rect (Screen.width - Screen.width/20, 
		                                                 15 + Screen.width/10, Screen.width/20, Screen.height/4),
		                                       touchSensitivity, 1.0f, 10.0f, sliderStyle, thumbStyle);
		Helper.touchsense = touchSensitivity;
		//Debug.Log ("touchSensitivity: "+touchSensitivity);
		*/
	}

	void displaygameOverScreen()
	{
		displaygameOverPopup = true;		
		Destroy (bloodTransform.gameObject);
	}

	void startGeneratingBubbles()
	{
		start_bubble_generation = true;
	}

	void stopGeneratingBubbles()
	{
		start_bubble_generation = false;
	}

	void generateBubble(int bubble)
	{
		if (!Helper.gameOver) {
						Transform bubbleTransform;
						if (bubble == plus_bubble)
								bubbleTransform = Instantiate (plus_bubble_Prefab) as Transform;
						if (bubble == minus_bubble)
								bubbleTransform = Instantiate (minus_bubble_Prefab) as Transform;
						if (bubble == shield_bubble)
								bubbleTransform = Instantiate (shield_bubble_Prefab) as Transform;
						else
								bubbleTransform = Instantiate (minus_bubble_Prefab) as Transform;

						Vector3 limitMin = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, -Camera.main.transform.position.z));
						Vector3 limitMax = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, -Camera.main.transform.position.z));
						Vector3 padding = bubbleTransform.GetComponent<Renderer>().bounds.extents;
						Vector3 spawnPos = new Vector3 (Random.Range (limitMin.x + padding.x, limitMax.x - padding.x), Random.Range (limitMin.y + padding.y, limitMax.y - padding.y));
						bubbleTransform.position = spawnPos;
				}
	}
	
	public void playMusic()
	{
		BGSound.loop = true;
		BGSound.Play ();
	}
	public void stopMusic()
	{
		BGSound.loop = false;
		BGSound.Stop ();
	}
	public bool isMusicPlaying()
	{
		return BGSound.isPlaying;
	}

	public void playDeathSound()
	{
		DeathSound.Play ();
	}

	public void loadMainMenu()
	{
		removeAdsBanner ();
		GameObject enemy1 = GameObject.Find ("enemy1");
		GameObject enemy2 = GameObject.Find ("enemy2");
		GameObject enemy3 = GameObject.Find ("enemy3");
		Destroy(enemy1);
		Destroy(enemy2);
		if(enemy3 != null)
			Destroy(enemy3);
		Destroy(gameObject);
		//Destroy (bloodTransform.gameObject);
		Application.LoadLevel("MainMenu");
	}

	void restartGame()
	{
		removeAdsBanner ();
		GameObject enemy1 = GameObject.Find ("enemy1");
		GameObject enemy2 = GameObject.Find ("enemy2");
		GameObject enemy3 = GameObject.Find ("enemy3");
		Destroy(enemy1);
		Destroy(enemy2);
		if(enemy3 != null)
			Destroy(enemy3);
		Destroy(gameObject);
		//Destroy (bloodTransform.gameObject);
		Application.LoadLevel("GameScene");
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
		//	StartAppWrapper.removeBanner (StartAppWrapper.BannerPosition.BOTTOM);

		#endif
	}
	void displayInterstitialAd()
	{
		//StartAppWrapper.showAd ();
		//StartAppWrapper.loadAd ();
	}
}
