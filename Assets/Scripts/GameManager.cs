using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	TODO: Make the board
	TODO: Add patterns of tiles
*/
public class GameManager : MonoBehaviour {
	public static GameManager INST;
	private static int BOARD_WIDTH = 5, BOARD_HEIGHT = 5, BOARD_SIZE = BOARD_WIDTH * BOARD_HEIGHT;
	private Tile[,] board = new Tile[BOARD_WIDTH, BOARD_HEIGHT];
	// private Tile[,] board = null; // a 2D array of tiles
	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform arenaHolder;

	// Make the border around the board
	private void MakeBorder() {
		for (int i = -1; i <= BOARD_WIDTH; i++) {
			for (int j = -1; j <= BOARD_HEIGHT; j++) {
				if (i == -1 || i == BOARD_WIDTH || j == -1 || j == BOARD_HEIGHT) {
					GameObject tile = Instantiate(tilePrefab, arenaHolder.transform);
					tile.transform.SetParent(arenaHolder.transform);
					tile.transform.localPosition = new Vector3(i, j, 0);

					// Set the tile to be a border
					tile.GetComponent<Tile>().SetAsBorder(Color.black);
				}
			}
		}
	}

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
		// Make the border
		MakeBorder();
    }
}
