using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	#region Public Methods
    public void Init(int x, int y, Color c) {
        transform.localPosition = new Vector3(x, y, 0);
		GetComponent<SpriteRenderer>().color = c;
    }
	public int GetX() => (int) transform.position.x;
	public int GetY() => (int) transform.position.y;
	#endregion
}