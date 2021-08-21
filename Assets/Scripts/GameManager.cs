using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make the arena
public class GameManager : MonoBehaviour {
	public static GameManager INST;
	private Tile[,] board; // a 2D array of tiles
	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform arenaHolder;

	// private int width = 5, height = 8;

	// public int GetArenaWidth() => width;
	// public int GetArenaHeight() => height;

	// private void MakeArena() {
	// 	// Make the tiles
	// 	for (int x = -width; x < width; x++) {
	// 		for (int y = -height; y < height; y++) {
	// 			if (x == -width || x == width - 1 || y == -height) {
	// 				GameObject tile = Instantiate(tilePrefab);
	// 				tile.transform.SetParent(arenaHolder);
	// 				tile.transform.localPosition = new Vector3(x, y, 0);
	// 			}
	// 		}
	// 	}
	// }

    void Awake() {
		INST = this;
		// MakeArena();
    }
}
