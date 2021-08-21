// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Block : MonoBehaviour {
// 	int width, height;

// 	internal int GetWidth() => width;

//     private void CreateTiles((int, int)[] pattern, GameObject tilePrefab, Color color) {
// 		// Foreach block in the pattern
// 		for (int i = 0; i < pattern.Length; i++) {
// 			// Get the block's position
// 			int x = pattern[i].Item1;
// 			int y = pattern[i].Item2;

// 			// Spawn the tile
// 			GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
			
// 			// Set the tile's color
// 			tile.GetComponent<SpriteRenderer>().color = color;
			
// 			// Set the tile's parent
// 			tile.transform.SetParent(this.transform);
// 		}
// 	}

// 	private void RandomizeRotation() {
// 		// Randomize the block's rotation (snap to 90 degrees)
// 		int rotation = Random.Range(0, 4);
// 		this.transform.localRotation = Quaternion.Euler(0, 0, rotation * 90);
// 	}

// 	// Formats the block tiles for easy positioning
// 	private void UpdateBlock() {
// 		// Get the bounds of the gameObject's tiles
// 		Transform[] tiles = this.transform.GetComponentsInChildren<Transform>();
// 		int lowerBoundX = int.MaxValue;
// 		int upperBoundX = int.MinValue;
// 		int lowerBoundY = int.MaxValue;
// 		int upperBoundY = int.MinValue;
// 		foreach (Transform tile in tiles) {
// 			// Get the tile's position
// 			Vector3 position = tile.position;
// 			int x = (int)position.x;
// 			int y = (int)position.y;
// 			if (x < lowerBoundX) lowerBoundX = x;
// 			else if (x > upperBoundX) upperBoundX = x;
	
// 			if (y < lowerBoundY) lowerBoundY = y;
// 			else if (y > upperBoundY) upperBoundY = y;
// 		}
// 		width = Mathf.Abs(upperBoundX) + Mathf.Abs(lowerBoundX) + 1;
// 		height = Mathf.Abs(upperBoundY) + Mathf.Abs(lowerBoundY) + 1;
// 		ShiftTiles(lowerBoundX, lowerBoundY);
// 	}

// 	// makes the bottom left tile be the origin of the blick
// 	private void ShiftTiles(int lowerBoundX, int lowerBoundY) {
// 		// Shift the tiles to the bottom left
// 		Transform[] tiles = this.transform.GetComponentsInChildren<Transform>();
// 		foreach (Transform tile in tiles) {
// 			tile.position = new Vector3(tile.position.x - lowerBoundX, tile.position.y - lowerBoundY, 0);
// 		}
// 	}

// 	public void Init((int, int)[] pattern, GameObject tilePrefab, Color color) {
// 		// Create the tiles
// 		CreateTiles(pattern, tilePrefab, color);
// 		// Randomize the block's rotation
// 		RandomizeRotation();
// 		// Update the block's bounds
// 		UpdateBlock();
// 		print(width + " " + height);
// 	}
// }
