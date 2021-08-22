using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	private int width, height;

	public void Init(int[,] pattern, Tile tilePrefab, Color color) {
		// Get the size of the pattern
		width = pattern.GetLength(0);
		height = pattern.GetLength(1);
		// Create the tiles
		for (int row = 0; row < width; row++) {
			for (int col = 0; col < height; col++) {
				if (pattern[row, col] == 1) {
					Tile tile = Instantiate(tilePrefab);
					tile.Init(row, col, color);
					tile.transform.SetParent(transform, false);
				}
			}
		}
	}

	public void PositionAtTop(int board_width, int board_height) {
		// Position the block at the top of the board
		transform.position = new Vector3(
			UnityEngine.Random.Range(0, board_width - width), // Random x position
			board_height - height, // Position block at top of board
			0
		);
	}
}
