using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	private bool isMoving = true;

	public void SetAsBorder() {
		isMoving = false;
	}

    public void Init(int x, int y, Color c) {
        transform.localPosition = new Vector3(x, y, 0);
		GetComponent<SpriteRenderer>().color = c;
    }
}