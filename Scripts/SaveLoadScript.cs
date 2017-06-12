using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadScript {
	
	//public static List<string> savedGames = new List<string>();
	
	//it's static so we can call it from anywhere
	public static bool isAuthenticated = false;
	static string highScoreKey = "HighScore";
	static string soundKey = "SoundOption";
	static string gamePlayKey = "no_of_game_plays";
	public static void Save(int data) {
		
		PlayerPrefs.SetInt(highScoreKey, data);
		PlayerPrefs.Save();
	}  
	
	public static int Load() 
	{
		return PlayerPrefs.GetInt(highScoreKey,0); 
	}
	public static void Save_sound(int data) {
		PlayerPrefs.SetInt(soundKey, data);
		PlayerPrefs.Save();
	}  
	
	public static int Load_sound() 
	{
		return PlayerPrefs.GetInt(soundKey,1);
	}

	public static void Save_no_of_game_plays(int no_of_game_plays) {
		PlayerPrefs.SetInt(gamePlayKey, no_of_game_plays);
		PlayerPrefs.Save();
	}  
	
	public static int Load_no_of_game_plays() 
	{
		return PlayerPrefs.GetInt(gamePlayKey,0);
	}
}
