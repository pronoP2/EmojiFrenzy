using UnityEngine;
using System.Collections;

public class BubbleScript : MonoBehaviour {

	// Use this for initialization
	public bool isPlusBubble;
	public bool isMinusBubble;
	public bool isShieldBubble;
	void Start () {
		Destroy (gameObject, 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
