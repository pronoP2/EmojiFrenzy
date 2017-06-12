using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {

		SaveLoadScript.Save_no_of_game_plays (0);
		Invoke ("loadMainMenu", 3f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void loadMainMenu()
	{
		Application.LoadLevel ("MainMenu");
	}
}
