using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[Serializable]
public class GameData {

	public static GameData gameData;
	public int high_score;
	public int current_score;
}
