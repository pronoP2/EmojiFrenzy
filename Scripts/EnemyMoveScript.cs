using UnityEngine;
using System.Collections;

public class EnemyMoveScript : MonoBehaviour {

	public Vector2 speed = new Vector2(3,3);
	public Vector2 direction = new Vector2 (1, 1);
	private Vector2 movement;

	// Use this for initialization
	void Awake()
	{
	}
	void Start () {
		Helper.gameOver = false;
		//gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		var dist = (transform.position - Camera.main.transform.position).z;
		
		var leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).x;
		
		var rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
			).x;
		
		var topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).y;
		
		var bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
			).y;
		//Debug.Log ("x = "+transform.position.x+" y = "+transform.position.y);

		if(direction.x == 1 && transform.position.x + transform.GetComponent<Renderer>().bounds.size.x/2 >= rightBorder)
		{
			direction.x = -1;
		}
		if(direction.x == -1 && transform.position.x - transform.GetComponent<Renderer>().bounds.size.x/2 <= leftBorder)
		{
			direction.x = 1;
		}
		if(direction.y == -1 && transform.position.y - transform.GetComponent<Renderer>().bounds.size.y/2 <= topBorder)
		{
			direction.y = 1;
		}
		if(direction.y == 1 && transform.position.y + transform.GetComponent<Renderer>().bounds.size.y/2 >= bottomBorder)
		{
			direction.y = -1;
		}
		if (!Helper.gameOver)
				movement = new Vector2 (speed.x * direction.x, speed.y * direction.y);
		else
				movement = new Vector2 (0, 0);
	
	}
	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = movement;
	}
}
