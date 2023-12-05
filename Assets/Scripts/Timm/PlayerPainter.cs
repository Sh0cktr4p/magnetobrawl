using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPainter : MonoBehaviour {

	public Color[] colors;
	public GameObject cursor;

	void Start() {
		int id = GetComponent<PlayerMovement>().PlayerID;
		GetComponent<SpriteRenderer>().color = colors[id];
		GetComponent<TrailRenderer>().startColor = colors[id];
		GetComponent<TrailRenderer>().endColor = colors[id];
		cursor.GetComponent<SpriteRenderer>().color = colors[id];
	}
}
