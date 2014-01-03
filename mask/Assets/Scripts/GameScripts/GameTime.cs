using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {
	//sun[] array to hold all sun like game objects
	public Transform[] gameLighting;
	//How many physical world minutes are in virtual day
	public float dayCycleMinutes = 24;
	//Constants for working with ingame time in virtual day seconds
	public float dayNumber = 1;
	private const float SECOND = 1;
	private const float MINUTE = 60 * SECOND;
	private const float HOUR = 60 * MINUTE;
	private const float DAY = 24 * HOUR;
	
	private int _currentTimeMinutes;
	private int _currentTimeHours;
	private float _timeElapsed;
	private float _rotationPerFrame;
	
	void Start() {
		_timeElapsed = 6;
		_rotationPerFrame = 360 / (dayCycleMinutes * MINUTE);
		
	}
	
	void Update () {
		//Rotate directional light source
		gameLighting[0].Rotate(new Vector3(_rotationPerFrame, 0, 0) * Time.deltaTime);
		//Record real time elapsed
		_timeElapsed += (Time.deltaTime / MINUTE) / (dayCycleMinutes/24);
		//Ensure time doesn't go over 24 hours, add a day count if it does
		if (_timeElapsed >= 24) {
			_timeElapsed -= 24;
			dayNumber ++;
		}
		_currentTimeHours = (int)_timeElapsed;
		_currentTimeMinutes = (int)((_timeElapsed - (int)_timeElapsed) * MINUTE);
		Debug.Log (_currentTimeHours + " " + _currentTimeMinutes);
		
		
	}
}