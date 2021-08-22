using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	#region Variables
	private List<Tile> tiles = new List<Tile>();
	private int width, height;
	private bool isStatic = false, isDestroyed = false;
	#endregion

	#region Methods
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
					tiles.Add(tile);
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
	public void AddToBoard(Tile[,] board) {
		// Add the tiles to the board
		foreach (Tile tile in tiles) {
			int x = tile.GetX();
			int y = tile.GetY();
			board[x, y] = tile;
		}
	}
	public void MoveBlock(Tile[,] board) {
		// Check if I can move the block
		foreach (Tile tile in tiles) {
			int x = tile.GetX();
			int y = tile.GetY();
			
			// If there is no open space below and the tile below is not this block's tile
			if (!OpenTileBelow(board, x, y) && !MyTileBelow(board, x, y)) isStatic = true;
		}

		// Move the block down one tile
		if (!isStatic) {
			// Remove the tiles from the board array
			foreach (Tile tile in tiles) board[tile.GetX(), tile.GetY()] = null;
			
			// Add the tiles to the board array with their new positions
			foreach (Tile tile in tiles) {
				tile.transform.position += Vector3.down;
				board[tile.GetX(), tile.GetY()] = tile;
			}
		}
	}
	#endregion

	#region Boolean functions
	// Check if there is an open tile below the current tile
	private bool OpenTileBelow(Tile[,] board, int x, int y) => y > 0 && board[x, y - 1] == null;
	// Check if my tile is below
	private bool MyTileBelow(Tile[,] board, int x, int y) => y > 0 && tiles.Contains(board[x, y - 1]);
	#endregion
}
