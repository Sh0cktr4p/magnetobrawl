using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamReplaceShader : MonoBehaviour {

	public Shader replacementShader;
	

	void OnEnable() {
		if (replacementShader != null) {
			GetComponent<Camera>().SetReplacementShader(replacementShader, "RenderType");
		}
	}

	void OnDisable() {
		GetComponent<Camera>().ResetReplacementShader();
	}
	
}
