using UnityEngine;
using System.Collections;

public class GameOverScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void displayMainMenu()
	{
		Destroy (gameObject);
		Application.LoadLevel ("MainMenu");
	}
	public void displayGameScene()
	{
		Destroy (gameObject);
		Application.LoadLevel ("GameScene");
	}
}
