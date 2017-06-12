using UnityEngine;
using System.Collections;
using StartApp;using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MainMenu : MonoBehaviour {

	protected string lastResponse = "";

	int best_score = 0;
	public static int current_score = 0;
	string score_value;
	string current_score_value;
	string score_label = "Your Best";
	string start_label = "";
	string current_score_label = "Your Score";
	string challenge_label = "";

	public bool removeButtonSound = false;

	public Texture logoTexture;

	public Texture background;
	public Texture background_heading;
	public Texture background_heading_chase;
	public Texture background_caption;
	public Texture background_player;
	public Texture background_enemy;
	public Texture background_player_dull;

	public GUIStyle style_start_button;
	public GUIStyle style_challenge_button;	
	public GUIStyle style_leaderboard_button;
	public GUIStyle style_label;
	public GUIStyle style_box;
	public GUIStyle inner_style_box;
	public GUIStyle style_popup_background;
	public GUIStyle style_popup_label;
	public GUIStyle style_popup_button;

	public float start_button_x;
	public float start_button_y;
	public float start_button_h;
	public float start_button_w;

	public float challenge_button_x;
	public float challenge_button_y;
	public float challenge_button_h;
	public float challenge_button_w;

	public float leaderboard_button_x;
	public float leaderboard_button_y;
	public float leaderboard_button_h;
	public float leaderboard_button_w;

	float score_box_x;
	float score_box_y;
	float score_box_h;
	float score_box_w;

	float x_offset = 10;
	float y_offset = 10;

	float background_heading_x;
	float background_heading_y;
	float background_heading_w;
	float background_heading_h;

	float background_heading_chase_x;
	float background_heading_chase_y;
	float background_heading_chase_w;
	float background_heading_chase_h;

	float background_caption_x;
	float background_caption_y;
	float background_caption_w;
	float background_caption_h;

	float background_player_x;
	float background_player_y;
	float background_player_w;
	float background_player_h;

	float background_enemy_x;
	float background_enemy_y;
	float background_enemy_w;
	float background_enemy_h;

	float bestScore_label_x;
	float bestScore_label_y;
	float bestScore_label_w;
	float bestScore_label_h;

	float currentScore_label_x;
	float currentScore_label_y;
	float currentScore_label_w;
	float currentScore_label_h;

	float bestScore_box_x;
	float bestScore_box_y;
	float bestScore_box_w;
	float bestScore_box_h;

	float currentScore_box_x;
	float currentScore_box_y;
	float currentScore_box_w;
	float currentScore_box_h;

	float popup_window_x;
	float popup_window_y;
	float popup_window_w;
	float popup_window_h;

	float popup_button_x;
	float popup_button_y;
	float popup_button_w;
	float popup_button_h;
	
	float popup_label_x;
	float popup_label_y;
	float popup_label_w;
	float popup_label_h;

	public AudioSource button_sound;
	public AudioClip button_press;
	// Use this for initialization
	void Start () {

		//Debug.Log ("raj Internet: " + Application.internetReachability);
		//Debug.Log ("raj Font Size: " + style_label.fontSize);
		//Debug.Log ("raj Screen.dp: " + (style_label.fontSize * Screen.dpi)/ 320 );


		style_label.fontSize = Mathf.FloorToInt((style_label.fontSize * Screen.dpi)/320);
		inner_style_box.fontSize = Mathf.FloorToInt((inner_style_box.fontSize * Screen.dpi)/320);
		style_popup_label.fontSize = Mathf.FloorToInt((style_popup_label.fontSize * Screen.dpi)/320);
		style_popup_button.fontSize = Mathf.FloorToInt((style_popup_button.fontSize * Screen.dpi)/320);

		Helper.gameOver = false;

		best_score = SaveLoadScript.Load ();
		score_value = getTimeInString (best_score);

		//background_player = (Texture)TextureScale.Bilinear ((Texture2D)background_player, Screen.width / 2, Screen.width / 2);


		current_score_value = getTimeInString (current_score);


		background_heading_x = x_offset;
		background_heading_y = y_offset; 
		background_heading_w = Screen.width - (x_offset * 2);
		background_heading_h = Screen.height / 5;//(background_heading.height * background_heading_w) / background_heading.width;//

		background_heading_chase_x = x_offset;
		background_heading_chase_y = background_heading_y + background_heading_h;
		background_heading_chase_w = Screen.width/2 + Screen.width/6;
		background_heading_chase_h = ((background_heading_chase.height * background_heading_chase_w)/background_heading_chase.width)+30;//Screen.width/8;

		background_player_x = x_offset;
		background_player_y = background_heading_chase_y + background_heading_chase_h + (2 * y_offset);//background_enemy_y + (background_enemy_h / 2);
		background_player_w = Screen.width / 2 - (x_offset * 2);
		background_player_h = Screen.width / 2 - (x_offset * 2);

		background_caption_x = Screen.width / 4;
		background_caption_y = background_heading_chase_y + background_heading_chase_h;
		background_caption_w = Screen.width/2;
		background_caption_h = Screen.width/8;

		background_enemy_w = Screen.width / 4;
		background_enemy_h = Screen.width / 4;
		background_enemy_x = Screen.width - background_enemy_w - x_offset;
		background_enemy_y = background_heading_chase_y + (background_heading_chase_h / 2);//background_heading_y + background_heading_h + y_offset;


		start_button_w = background_player_w * 0.75f;
		start_button_h = Screen.height / 12;
		start_button_x = background_player_x + (background_player_w - start_button_w) / 2; 
		start_button_y = background_player_y + background_player_h + (y_offset * 3);

		score_box_x = Screen.width / 2 + x_offset;
		score_box_y = background_enemy_y + background_enemy_h + (y_offset * 2);
		score_box_w = Screen.width / 2 - (x_offset * 2);
		//score_box_h = style_label.fontSize * 2 + inner_style_box.fontSize * 2 + y_offset*2;

		bestScore_label_x = score_box_x + x_offset;
		bestScore_label_y = score_box_y + y_offset;
		bestScore_label_w = style_label.fontSize * score_label.Length;
		bestScore_label_h = style_label.fontSize+y_offset;

		bestScore_box_w = score_box_w * 0.75f;
		bestScore_box_h = inner_style_box.fontSize + 15;
		bestScore_box_x = score_box_x + (score_box_w - bestScore_box_w)/2;
		bestScore_box_y = bestScore_label_y + bestScore_label_h + y_offset;

		currentScore_label_x = score_box_x + x_offset;
		currentScore_label_y = bestScore_box_y + bestScore_box_h + y_offset*2;
		currentScore_label_w = style_label.fontSize * current_score_label.Length;
		currentScore_label_h = style_label.fontSize+y_offset;

		currentScore_box_w = score_box_w * 0.75f;
		currentScore_box_h = inner_style_box.fontSize + 15;
		currentScore_box_x = score_box_x + (score_box_w - currentScore_box_w)/2;
		currentScore_box_y = currentScore_label_y + currentScore_label_h + y_offset;

		score_box_h = bestScore_label_h + bestScore_box_h + currentScore_label_h + currentScore_box_h + (y_offset * 7);

		challenge_button_x = score_box_x;
		challenge_button_y = score_box_y + score_box_h + (y_offset * 3);
		challenge_button_w = score_box_w;
		challenge_button_h = start_button_h;

		/*leaderboard_button_x = score_box_x;
		leaderboard_button_y = challenge_button_y + challenge_button_h + y_offset;
		leaderboard_button_w = score_box_w;
		leaderboard_button_h = start_button_h;*/

		leaderboard_button_x = x_offset;
		leaderboard_button_y = start_button_y + start_button_h + y_offset;
		leaderboard_button_w = background_player_w;
		leaderboard_button_h = start_button_h;


		popup_window_x = (Screen.width - Screen.width * 0.75f) / 2;
		popup_window_y = (Screen.height - Screen.height / 3) / 2;
		popup_window_w = Screen.width * 0.75f;
		popup_window_h = Screen.height / 4;

		popup_button_w = popup_window_w / 2;
		popup_button_h = style_popup_button.fontSize + y_offset;
		popup_button_x = (popup_window_w - popup_button_w) / 2;
		popup_button_y = popup_window_h - popup_button_h - 2*y_offset;

		popup_label_x = x_offset;// + (popup_window_w/2);
		popup_label_y = y_offset * 2;
		popup_label_w = popup_window_w - (x_offset * 2);
		popup_label_h = popup_window_h - popup_button_h;


		/*#if UNITY_ANDROID
		
						StartAppWrapper.loadAd ();

						StartAppWrapper.addBanner (
		                          StartAppWrapper.BannerType.AUTOMATIC,
		                          StartAppWrapper.BannerPosition.BOTTOM);
				
		#endif*/

		PlayGamesPlatform.Activate();


	}
	
	// Update is called once per frame
	void Update () {

		}
	void LoginCallback(FBResult result)
	{
		/*if (result.Error != null)
			lastResponse = "Error Response:\n" + result.Error;
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
		}
		else
		{
			lastResponse = "Login was successful!";
			FB.Feed(picture:"http://hangoverstudios.com/images/512.png",
			        linkName:"Can You Beat My Score: "+SaveLoadScript.Load(),
			        linkCaption:"Funny Chase Challenge",
			        linkDescription:"Funny Chase is very interesting, addictive and time pass chase games. You have to help the smiley escape from the other angry smilies chasing him. If angry smiley catch player smiley then game will be over. As you continue play chase will become faster and faster.\nWhile playing you will get any of three power ups, shield, pill or burger. The power up valid for 10 seconds. Smiley became fat on eating a burger. Smiley will became slim by taking a pill. Shield will help you to survive for next 10 seconds from enemy.\nKids as well as adults can enjoy playing this game its absolutely free.\nEnjoy the game, achieve your best score and challenge your friends. Go ahead and crush your best friends high score.",
			        link:"https://play.google.com/store/apps/details?id=com.HangoverStudios.Funnychase");
			        
			FB.AppRequest(message:"Escape from Death",
			           //   data:"My High Score:",
			              title:"Escape like me");

		}*/
	}
	private void OnInitComplete()
	{
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		FB.Login("email,publish_actions", LoginCallback);
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}
		
		void OnGUI() 
		{

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
		GUI.depth = 64;
		GUI.DrawTexture (new Rect (background_heading_x, background_heading_y, background_heading_w, background_heading_h), background_heading);
		GUI.DrawTexture (new Rect (background_heading_chase_x, background_heading_chase_y, background_heading_chase_w, background_heading_chase_h), background_heading_chase);
		GUI.DrawTexture (new Rect (background_caption_x, background_caption_y, background_caption_w, background_caption_h), background_caption);
		GUI.DrawTexture (new Rect (background_player_x, background_player_y , background_player_w, background_player_h), background_player);
		GUI.DrawTexture (new Rect (background_enemy_x, background_enemy_y, background_enemy_w, background_enemy_h), background_enemy);


		GUI.Box(new Rect(score_box_x, score_box_y, score_box_w, score_box_h),"",style_box);
		
		GUI.Label(new Rect(bestScore_label_x, bestScore_label_y, bestScore_label_w, bestScore_label_h),
		          new GUIContent(score_label.ToString()), style_label);

		inner_style_box.font.RequestCharactersInTexture (score_value);

		GUI.Box(new Rect(bestScore_box_x, bestScore_box_y, bestScore_box_w, bestScore_box_h),
		        score_value, inner_style_box);
		
		GUI.Label(new Rect(currentScore_label_x, currentScore_label_y, currentScore_label_w, currentScore_label_h),
		          new GUIContent(current_score_label.ToString()), style_label);
		GUI.Box(new Rect(currentScore_box_x, currentScore_box_y, currentScore_box_w, currentScore_box_h),
		        current_score_value, inner_style_box);


		if(GUI.Button (new Rect (start_button_x, start_button_y , start_button_w, start_button_h), start_label, style_start_button))
		{
			Helper.isRetry = false;
			if(!removeButtonSound)
				button_sound.PlayOneShot(button_press);
			launchGame();
			//Invoke("launchGame", 0.5f);

			//Destroy(gameObject);
			//Application.LoadLevel("GameScene");
		}
		/*if(GUI.Button (new Rect (challenge_button_x, challenge_button_y , challenge_button_w, challenge_button_h), challenge_label, style_challenge_button))
		{
			if(!removeButtonSound)
				button_sound.PlayOneShot(button_press);
			if (Application.platform == RuntimePlatform.Android) {
				if(isInternetAvailable())
					FB.Init(OnInitComplete, OnHideUnity);
				else
					showPopUp = true;
			}
		}
		if(GUI.Button (new Rect (leaderboard_button_x, leaderboard_button_y , leaderboard_button_w, leaderboard_button_h), "", style_leaderboard_button))
		{
			if(!removeButtonSound)
				button_sound.PlayOneShot(button_press);
			if (Application.platform == RuntimePlatform.Android) {
				if(isInternetAvailable())
					showLeaderboard();
				else
					showPopUp = true;
			}
		}*/
		if (showPopUp)
		{		
			GUI.skin.window = style_popup_background;
			GUI.Window(0, new Rect(popup_window_x, popup_window_y, popup_window_w, popup_window_h), 
			           ShowGUI, "");
			
		}
	}

	void launchGame()
	{
		Destroy(gameObject);
		Application.LoadLevel("GameScene");
	}

	bool showPopUp;

	void ShowGUI(int windowID)
	{
		GUI.Label(new Rect(popup_label_x, popup_label_y, popup_label_w, popup_label_h), 
		          "Please Enable Internet to Proceed", style_popup_label);
		
		// You may put a button to close the pop up too
		
		if (GUI.Button(new Rect(popup_button_x, popup_button_y, popup_button_w, popup_button_h), "", style_popup_button))
		{
			showPopUp = false;
			// you may put other code to run according to your game too
		}
	}
	void showLeaderboard()
	{
		// authenticate user:
		/*if (!SaveLoadScript.isAuthenticated) 
		{
			Social.localUser.Authenticate ((bool success) => {
					// handle success or failure
				if (success) 
				{
					SaveLoadScript.isAuthenticated = true;
					postScoreToServer ();
				}
				else
					SaveLoadScript.isAuthenticated = false;
			Debug.Log (success ? "Authentication successfully" : "Authentication Failed");
			});
		}
		else
			postScoreToServer ();
		*/
	}
	
	void postScoreToServer()
	{
		//UnityEngine.SocialPlatforms.ILeaderboard leaderboard = Social.CreateLeaderboard ();
		/*Social.ReportScore(SaveLoadScript.Load (),"CggIh9T_tV8QAhAB",success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
		((PlayGamesPlatform) Social.Active).ShowLeaderboardUI("CggIh9T_tV8QAhAB");*/
	}

	private string getTimeInString(int time)
	{
		//string displaySeconds = Mathf.Floor(time % 60).ToString("00");
		//string displayMinutes = Mathf.Floor(time / 60).ToString("00");
		//return string.Format("{00}:{00}", displayMinutes, displaySeconds);
		//return displayMinutes + ":" + displaySeconds;
		return "" + time;
	}

	public static bool isInternetAvailable()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;//Application.internetReachability.NotReachable;
	}
}
