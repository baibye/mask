using UnityEngine;
using System.Collections;

/* TO DO:
 * Implement in game lighting that turns on and off automatically
 * during night / day
 * Set up a gradually incrementing tint on skybox[1] so that it can
 * be used for Day Phases: Dawn, Day, Dusk
 * 
 * MAYBE use fog effects in Dawn, Dusk
 * */

public class DayNight : MonoBehaviour 
{
	// All of the game lighting controlled by game time
	public Transform[] gameLighting;
	// Array of skyboxes into SkyBoxes[]
	public Material[] SkyBoxes;
	// Real-time seconds in one game day
	public float dayCycleSeconds;
	// The curent time in the day cycle
	public float currentCycleTime;
	// The current 'phase' of the day: Dawn, Day, Dusk, Night
	public DayPhase currentPhase;
	// The hour of the day (calculated, do not change)
	public int worldTimeHour;
	public int worldTimeMinute;
	// Ambient color during Night
	public Color nightLight;
	// Ambient color during Day
	public Color dayLight;
	// The calculated time that it takes for night, dawn, day, dusk
	private float nightTime;
	private float dawnTime;
	private float dayTime;
	private float duskTime;
	private float changeTime;
	private float relativeTime;

	void Start()
	{
		Initialize();
	}


	void Initialize() 
	{
		currentCycleTime = 0.0f;
		dawnTime = 0.0f;
		dayTime = dawnTime + dayCycleSeconds * 0.15f;
		duskTime = dayTime + dayCycleSeconds * 0.10f;
		nightTime = duskTime + dayCycleSeconds * 0.15f;
		changeTime = dayTime - dawnTime;
		RenderSettings.skybox = SkyBoxes[0];
		RenderSettings.ambientLight = nightLight;
	}

	// Update is called once per frame
	void Update() 
	{
		// Check if cycle has changed
		if (currentCycleTime > nightTime && currentPhase == DayPhase.Dusk)
		{
			setNight();
		}
		else if (currentCycleTime > duskTime && currentPhase == DayPhase.Day)
		{
			setDusk();
		}
		else if (currentCycleTime > dayTime && currentPhase == DayPhase.Dawn)
		{
			setDay();
		}
		else if (currentCycleTime >= dayCycleSeconds && currentPhase == DayPhase.Night)
		{
			setDawn();
		}
		
		// Update time and amount of daylight
		UpdateWorldTime();
		UpdateDaylight();
		
		// Add how many seconds have elasped to currentCycleTime
		currentCycleTime += Time.deltaTime;
	}

	void Reset()
	{
		nightLight = new Color(32.0f / 255.0f, 28.0f / 255.0f, 46.0f / 255.0f);
		dayLight = new Color(255.0f / 255.0f, 255.0f / 255.0f, 240.0f / 255.0f);
		dayCycleSeconds = 720f;
		currentPhase = DayPhase.Dawn;
		currentCycleTime = 0.0f;
	}
	
	void setNight () 
	{
		RenderSettings.skybox = SkyBoxes[3];
		currentPhase = DayPhase.Night;
	}
	
	void setDawn () 
	{
		currentCycleTime = 0.0f;
		RenderSettings.skybox = SkyBoxes[0];
		currentPhase = DayPhase.Dawn;
		//SkyBoxes[1].SetColor( "_Tint", Color( red, green, blue ) );
	}
	
	void setDay () 
	{
		RenderSettings.skybox = SkyBoxes[1];
		currentPhase = DayPhase.Day;
	}
	
	void setDusk () 
	{
		RenderSettings.skybox = SkyBoxes[2]; 
		currentPhase = DayPhase.Dusk;
		//SkyBoxes[1].SetColor( "_Tint", Color( red, green, blue) );		
	}

	// Add Fog here when/if time comes
	private void UpdateDaylight()
	{
		// Increase / Decrease light intensity and gradually shift ambient colors
		if (currentPhase == DayPhase.Dawn)
		{
			float relativeTime = currentCycleTime - dawnTime;
			RenderSettings.ambientLight = Color.Lerp(nightLight, dayLight, (relativeTime / changeTime));
			light.intensity = Mathf.Sin(0.5f * Mathf.PI / (dayTime - dawnTime) * currentCycleTime)*0.3f;
		}
		else if (currentPhase == DayPhase.Dusk)
		{
			float relativeTime = currentCycleTime - duskTime;
			RenderSettings.ambientLight = Color.Lerp(dayLight, nightLight, (relativeTime / changeTime));
			light.intensity = Mathf.Cos(0.5f * Mathf.PI / (nightTime - duskTime) * (currentCycleTime - duskTime))*0.3f;
		}
		// Rotate directional light
		transform.Rotate(Vector3.right * ((Time.deltaTime / dayCycleSeconds) * 360.0f), Space.Self);
	}

	// Update and set time in a 24 hour clock. Reset Day Night phase 
	private void UpdateWorldTime() 
	{
		worldTimeHour = Mathf.FloorToInt((currentCycleTime / dayCycleSeconds)*24) + 7;
		worldTimeMinute = (int)((((currentCycleTime / dayCycleSeconds * 24) + 7) - worldTimeHour) * 60);
		if (worldTimeHour >= 24) 
		{
			worldTimeHour -= 24;
		}
	}

	public enum DayPhase 
	{ 
		Dawn, 
		Day, 
		Dusk,
		Night,
	}
}
		