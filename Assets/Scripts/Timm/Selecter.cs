using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Selecter : MonoBehaviour {

	public int colorID = 0;

	public int owner = -1;
	public RectTransform buttonTop;
	public Text t;

	string[] Cols = {"Yellow", "Orange", "Blue", "Purple"};
	static bool[] used;
	// Use this for initialization
	void Start () {
		used = new bool[]{ false,false, false, false};
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Joystick2Button7) || Input.GetKeyDown(KeyCode.Joystick3Button7) || Input.GetKeyDown(KeyCode.Joystick4Button7)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
		}

		if (owner == -1) {
			t.text = "<b>Press to Join "+ Cols[colorID]+ "</b>";
		} else {
			t.text = "<b>"+Cols[colorID]+" Joined by Player " +(owner+1)+ "</b>";
		}

		buttonTop.anchoredPosition = new Vector2(-5, 5);
		if (Input.GetKey(KeyCode.Joystick1Button0 + colorID)) {
			if (owner == -1 && !used[0]) {
				used[0] = true;
				owner = 0;
				PlayerPrefs.SetInt("PlayerID" + colorID, 0);
			}
			if (owner == 0) {
				buttonTop.anchoredPosition = new Vector2(0, 0);
			}
		}
		if (Input.GetKey(KeyCode.Joystick2Button0 + colorID)) {
			if (owner == -1 && !used[1]) {

				used[1] = true;
				owner = 1;
				PlayerPrefs.SetInt("PlayerID" + colorID, 1);
			}
			if (owner == 1) {
				buttonTop.anchoredPosition = new Vector2(0, 0);
			}
		}
		if (Input.GetKey(KeyCode.Joystick3Button0 + colorID)) {
			if (owner == -1 && !used[2]) {
				owner = 2;
				used[2] = true;
				PlayerPrefs.SetInt("PlayerID" + colorID, 2);
			}
			if (owner == 2) {
				buttonTop.anchoredPosition = new Vector2(0, 0);
			}
		}
		if (Input.GetKey(KeyCode.Joystick4Button0 + colorID)) {
			if (owner == -1 && !used[3]) {
				used[3] = true;
				owner = 3;
				PlayerPrefs.SetInt("PlayerID" + colorID, 3);
			}
			if (owner == 3) {
				buttonTop.anchoredPosition = new Vector2(0, 0);
			}
		}

	}
}
