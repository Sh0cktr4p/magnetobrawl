using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFixer : MonoBehaviour {

	public Texture rampTex;

	void Start () {
		MaterialPropertyBlock pb = new MaterialPropertyBlock();
		pb.SetTexture("_MainTex", rampTex);
		GetComponent<TrailRenderer>().SetPropertyBlock(pb);
	}
	
	void Update () {
		
	}
}
