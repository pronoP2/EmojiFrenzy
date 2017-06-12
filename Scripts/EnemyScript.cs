using UnityEngine;
using System.Collections;
using StartApp;

public class EnemyScript : MonoBehaviour {

	//int startTime;
	public int roundedRestSeconds = 0;
	EnemyMoveScript enemyMove;
	public GUIStyle style_label;
	public Texture hour_glass;

	string stageTime;

	bool keepConstantSpeed = false;
	public float constantSpeedTime = 40f;
	public bool setEnemyActive = true;
	public float tabletSize = 5.9f;

	float var_constantSpeedTime;// = constantSpeedTime;

	// Use this for initialization
	void Start () {

		if (!setEnemyActive && isDeviceTablet()) 
		{
			setEnemyActive = true;
		}
		gameObject.SetActive (setEnemyActive);

		roundedRestSeconds = 0;

		if (getDeviceSize () >= 3.0f && getDeviceSize () < 4.0f)
			constantSpeedTime = 60f;
		if (getDeviceSize () >= 4.0f && getDeviceSize () < 5.0f)
			constantSpeedTime = 70f;
		else if (getDeviceSize () > 5.0f && getDeviceSize () <= 5.5f)
			constantSpeedTime = 80f;
		else if (getDeviceSize () > 5.5f && getDeviceSize () <= 6.0f)
			constantSpeedTime = 90f;
		else if (getDeviceSize () > 6.0f && getDeviceSize () <= 6.5f)
			constantSpeedTime = 100f;		
		else if (getDeviceSize () > 6.5f && getDeviceSize () <= 7.0f)
			constantSpeedTime = 110f;		
		else if (getDeviceSize () > 7.0f && getDeviceSize () <= 7.5f)
			constantSpeedTime = 120f;		
		else if (getDeviceSize () > 7.5f && getDeviceSize () <= 8.0f)
			constantSpeedTime = 130f;		
		else if (getDeviceSize () > 8.0f)
			constantSpeedTime = 150f;

		style_label.fontSize = Mathf.FloorToInt((style_label.fontSize * Screen.dpi)/320);

		var_constantSpeedTime = constantSpeedTime;
		enemyMove = GetComponent<EnemyMoveScript> ();
		Helper.startTime = Mathf.CeilToInt(Time.time);
		Debug.Log ("start time = "+Helper.startTime);
		#if UNITY_ANDROID
						//StartAppWrapper.removeBanner (StartAppWrapper.BannerPosition.BOTTOM);
	
		#endif
	}
	void Awake()
	{
		//roundedRestSeconds = 0;
	}

	float getDeviceSize()
	{
		return (Mathf.Sqrt ((Screen.width * Screen.width)+(Screen.height * Screen.height)))/Screen.dpi;
	}
	bool isDeviceTablet()
	{
		if (getDeviceSize() > 5.5f)
			return true;
		else
			return false;
	}
	
	// Update is called once per frame
	void Update () {

		if (!keepConstantSpeed) 
		{
			if (var_constantSpeedTime > 0)
				var_constantSpeedTime -= Time.deltaTime;
			else 
			{
				var_constantSpeedTime = constantSpeedTime;
				keepConstantSpeed = true;
			}
		}
		if(!Helper.gameOver && !Helper.isGamePaused)
			roundedRestSeconds = Mathf.CeilToInt(Time.time - Helper.startTime);

		/*if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				EnemyScript enemy = GetComponent<EnemyScript> ();
				if (enemy != null) {
					//Destroy (transform.gameObject);
					if (enemy.roundedRestSeconds > SaveLoadScript.Load ())
						SaveLoadScript.Save (enemy.roundedRestSeconds);
					MainMenu.current_score = enemy.roundedRestSeconds;
					Application.LoadLevel ("MainMenu");
					removeAdsBanner();
				}
				return;
			}
		}*/
		
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript> ();
		if (enemy != null) 
		{
			EnemyMoveScript move = enemy.GetComponent<EnemyMoveScript>();
			if(move != null)
				move.direction.y = -move.direction.y;
		}
	}
	void OnGUI() 
	{
		string displaySeconds = Mathf.Floor(roundedRestSeconds % 60).ToString("00");
		string displayMinutes = Mathf.Floor(roundedRestSeconds / 60).ToString("00");

		int m_displaySeconds = Mathf.FloorToInt(roundedRestSeconds % 60);
		int m_displayMinutes = roundedRestSeconds / 60; 

		if (!keepConstantSpeed) 
		{
			if (m_displaySeconds % 10 == 0) 
			{
				enemyMove.speed.x += 0.003f;
				enemyMove.speed.y += 0.003f;
			}
		}

		//Debug.Log ("keepConstantSpeed: "+keepConstantSpeed);

		stageTime = displayMinutes + ":" + displaySeconds;
		int offset = style_label.fontSize * stageTime.Length + 10;
		int power_time = Mathf.FloorToInt (Helper.power_hold_time_disp) + 1;
		style_label.font.RequestCharactersInTexture ("Score1");
		if (!Helper.gameOver)
		{
				GUI.Box (new Rect (10, 10, offset, 50), "Score: " + roundedRestSeconds, style_label);
				if (power_time > 0 && Helper.show_power_time)
						GUI.Box (new Rect (offset, 10, offset, 50), "PowerUp Time: " + power_time, style_label);
				//GUI.DrawTexture (new Rect (10 + offset, 10, 50, 50), hour_glass, ScaleMode.ScaleToFit);
		}

	}
	void displayAdsBanner()
	{
		#if UNITY_ANDROID
		
		/*StartAppWrapper.loadAd ();
		
		StartAppWrapper.addBanner (
			StartAppWrapper.BannerType.AUTOMATIC,
			StartAppWrapper.BannerPosition.TOP);
		*/
		#endif
	}
	
	void removeAdsBanner()
	{
		#if UNITY_ANDROID
		//StartAppWrapper.removeBanner (StartAppWrapper.BannerPosition.TOP);
		
		#endif
	}
}
