using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	private bool isMoving = true;

	public void SetAsBorder(Color c) {
		GetComponent<SpriteRenderer>().color = c;
		isMoving = false;
	}

    void Awake() {
        
    }

    void Update() {
        
    }
}