﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeKnopf : MonoBehaviour {

    public GameObject img;

    private void Update () {

        

		if (Input.GetKeyDown(KeyCode.Escape))
		{
            
            img.SetActive(!img.active);
		}
	}
}
