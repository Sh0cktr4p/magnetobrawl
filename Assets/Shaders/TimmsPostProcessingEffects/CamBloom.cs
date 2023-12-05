using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBloom : MonoBehaviour {

	[Range(0,100)]
	public float blurSize;
	public float intensity;
	public RenderTexture blurRT;
	private Material blurMat;
	private Material combineMat;

	[Range(0,1)]
	public float brightness = 1;

	public float blSizeLow = 20;
	public float blSizeHigh = 40;
	public float intensityLow = 1;
	public float intensityHIgh = 1.5f;


	RenderTexture temp;
	void Awake() {
		blurMat = new Material(Shader.Find("Hidden/FXBloom"));
		combineMat = new Material(Shader.Find("Hidden/FXCombine"));
		temp = new RenderTexture(blurRT.width, blurRT.height, 0);

	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (blurSize == 0) {
			Graphics.Blit(source, destination);
			return;
		}

		brightness = Kick_Catcher.currentAmplitude;
		blurSize = Mathf.Lerp(blSizeLow, blSizeHigh, brightness);
		intensity = Mathf.Lerp(intensityLow, intensityHIgh, brightness);

		blurMat.SetFloat("_blurSize", blurSize);
		blurMat.SetFloat("_intensity", intensity);

		blurMat.SetFloat("_XDir", 0);
		Graphics.Blit(blurRT, temp, blurMat);
		
		blurMat.SetFloat("_XDir", 1);
		Graphics.Blit(temp, blurRT, blurMat);

		combineMat.SetTexture("_BlurTex", blurRT);
		Graphics.Blit(source, destination, combineMat);

		
	}
}
