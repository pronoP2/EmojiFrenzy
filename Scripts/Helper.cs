using UnityEngine;
using System.Collections;

public class Helper{

	public static bool gameOver = false;
	public static float touchsense = 1.0f;
	public static float power_hold_time_disp;
	public static bool show_power_time = false;
	public static bool isGameFreezed = false;
	public static bool isGamePaused = false;
	public static int startTime = 0;
	public static bool isRetry = false;

	public static float getDeviceSize()
	{
		return (Mathf.Sqrt ((Screen.width * Screen.width)+(Screen.height * Screen.height)))/Screen.dpi;
	}
}
