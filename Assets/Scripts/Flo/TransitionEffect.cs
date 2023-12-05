using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TransitionEffect : MonoBehaviour {

    public float fadeIntime = 1;
    public Material effectMat;

    private float timer = 0;

    private void OnStart() {
        
        //StartCoroutine(fadeIn());
    }
    public void Update() {
        if(timer < fadeIntime) {
            timer += Time.deltaTime;
            effectMat.SetFloat("_Cutoff", 1-(timer / fadeIntime));
        }
    }

    IEnumerator fadeIn() {
        float timer = 0;

        while (timer < fadeIntime) {
            
            yield return 0;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, effectMat);
    }
}
