using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	#region Variables
	private List<Tile> tiles = new List<Tile>();
	private int width, height;
	// private bool isStatic = false, isDestroyed = false;
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
	public void Init(int width, int height, List<Tile> tiles) {
		// Get the size of the pattern
		this.width = width;
		this.height = height;
		this.tiles = tiles;
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
	public void MoveBlock(int xDiff, int yDiff) {
		Tile[,] board = GameManager.INST.GetBoard();
		// Check if I can move the block
		foreach (Tile tile in tiles) {
			int x = tile.GetX(), y = tile.GetY();
			// If there is no open space below and the tile below is not this block's tile
			if (!OpenTile(x + xDiff, y + yDiff) && !MyTile(x + xDiff, y + yDiff)) return;
		}

		// Move the block down one tile
		// Remove the tiles from the board array
		foreach (Tile tile in tiles) board[tile.GetX(), tile.GetY()] = null;
		// Add the tiles to the board array with their new positions
		foreach (Tile tile in tiles) {
			tile.transform.position += new Vector3(xDiff, yDiff, 0);
			board[tile.GetX(), tile.GetY()] = tile;
		}
	}
	// public List<Tile>[] GetTileNeighbors() {
	// 	List<Tile>[] neighbors = new List<Tile>[tiles.Count];
	// 	for (int t = 0; t < tiles.Count; t++) {
	// 		Tile tile = tiles[t];
	// 		// Check the surrounding tiles
	// 		int x = tile.GetX(), y = tile.GetY();
	// 		for (int i = x - 1; i <= x + 1; i++) {
	// 			for (int j = y - 1; j <= y + 1; j++) {
	// 				if (!InBounds(i, j)) continue;
	// 				Tile neighborTile = GameManager.INST.GetBoard()[i, j];
	// 				if (neighborTile == null) continue;
	// 				neighbors[t].Add(neighborTile);
	// 			}
	// 		}
	// 	}
	// 	return neighbors;
	// }
	public void AddTile(Tile tile) => tiles.Add(tile);
	public void BreakBlock() {
		foreach (Tile tile in tiles) {
			tile.BreakTile();
		}
		GameManager.INST.RemoveBlock(this);
	}
	// public void BreakTile(Tile deletedTile) {
	// 	if (deletedTile == null ||
	// 		!tiles.Contains(deletedTile)
	// 	) return;
	// 	tiles.Remove(deletedTile);
	// 	// deletedTile.GetComponent<SpriteRenderer>().color = Color.white;
	// 	// Destroy(deletedTile.gameObject);
	// 	List<Block> newBlocks = new List<Block>();
	// 	foreach (Tile tile in tiles) {
	// 		Block block = Instantiate(GameManager.INST.GetBlockPrefab());
	// 		List<Tile> newTiles = new List<Tile>();
	// 		newTiles = GetTileGroup(newTiles, tile);
	// 		block.Init(width, height, newTiles);
	// 	}
	// }
	// public List<Tile> GetTileGroup(List<Tile> group, Tile tile) {
	// 	tiles.Remove(tile);
	// 	group.Add(tile);
	// 	int x = tile.GetX(), y = tile.GetY();
	// 	for (int i = -1; i <= 1; i++) {
	// 		for (int j = -1; j <= 1; j++) {
	// 			if (x == 0 && y == 0 ||
	// 				Mathf.Abs(i) + Mathf.Abs(j) == 2 ||
	// 				!MyTile(x + i, y + j)
	// 			) continue;
	// 			Tile neighborTile = GameManager.INST.GetBoard()[x + i, y + j];
	// 			GetTileGroup(group, neighborTile);
	// 		}
	// 	}
	// 	return group;
	// }
	#endregion

	#region Boolean functions
	// Check if there is an open tile at coordinates x, y
	private bool OpenTile(int x, int y) => GameManager.INST.InBounds(x, y) && GameManager.INST.GetBoard()[x, y] == null;
	// Check if my tile is at coordinates x, y
	private bool MyTile(int x, int y) => GameManager.INST.InBounds(x, y) && tiles.Contains(GameManager.INST.GetBoard()[x, y]);
	// // Check if the tile is in the bounds of the board
	// private bool InBounds(int x, int y) => x >= 0 && x < GameManager.INST.GetBoardWidth() && y >= 0 && y < GameManager.INST.GetBoardHeight();
	#endregion
}
