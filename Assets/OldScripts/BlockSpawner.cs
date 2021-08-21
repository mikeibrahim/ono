// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public static class BlockPattern {
// 	// All the tetris blocks
// 	public static (int, int)[][] blockPatterns = new (int, int)[][] {
// 		/*
// 		[][][][]
// 		*/
// 		new (int, int)[] {
// 			(0, 0),
// 			(1, 0),
// 			(2, 0),
// 			(3, 0)
// 		},
// 		/*
// 		[][]
// 		[][]
// 		*/
// 		new (int, int)[] {
// 			(0, 0),
// 			(1, 0),
// 			(0, 1),
// 			(1, 1)
// 		},
// 		/*
// 		  []
// 		[][][]
// 		*/
// 		new (int, int)[] {
// 			(0, 0),
// 			(1, 1),
// 			(1, 0),
// 			(2, 0)
// 		},
// 		/*
// 		[][][]
// 		[]
// 		*/
// 		new (int, int)[] {
// 			(0, 0),
// 			(0, 1),
// 			(1, 1),
// 			(2, 1)
// 		},
// 		/*
// 		  [][]
// 		[][]
// 		*/
// 		new (int, int)[] {
// 			(0, 0),
// 			(1, 0),
// 			(1, 1),
// 			(2, 1)
// 		},
// 		/*
// 		[][]
// 		  [][]
// 		*/
// 		new (int, int)[] {
// 			(0, 1),
// 			(1, 0),
// 			(1, 1),
// 			(2, 0)
// 		}
// 	};
// 	public static int numPatterns = blockPatterns.Length;
// }

// public class BlockSpawner : MonoBehaviour {
// 	[SerializeField] private GameObject tilePrefab = null; // The prefab for the tiles
// 	[SerializeField] private GameObject blockPrefab = null; // The prefab for the blocks
// 	[SerializeField] private GameObject blockParent = null; // The parent for the blocks
// 	private float spawnInterval = 3f; // The interval between spawns
// 	float currentSpawnTime; // The current spawn time

// 	// Place the block above the screen
// 	private void CreateBlock() {
// 		// Get a random pattern
// 		int patternIndex = Random.Range(0, BlockPattern.numPatterns);
// 		(int, int)[] pattern = BlockPattern.blockPatterns[patternIndex];

// 		// Make the block
// 		Block block = Instantiate(blockPrefab).GetComponent<Block>();

// 		// Block Color TODO: Make predefined colors
// 		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

// 		// Set the block's tiles
// 		block.Init(pattern, tilePrefab, color);

// 		// Set the block's parent
// 		block.transform.SetParent(blockParent.transform);

// 		// Set the block's position
// 		block.transform.position = new Vector3(	Random.Range(	-GameManager.INST.GetArenaWidth()+1, 
// 																GameManager.INST.GetArenaWidth()-block.GetWidth()), 
// 												GameManager.INST.GetArenaHeight(), 0f);
// 	}

//     private void Awake() {
//         currentSpawnTime = spawnInterval; // Set the current spawn time to the interval
//     }

//     private void FixedUpdate() {
//         if (currentSpawnTime <= 0) { // If the spawn time is up
// 			CreateBlock(); // Spawn a block
// 			currentSpawnTime = spawnInterval; // Reset the spawn time
// 		} else {
// 			currentSpawnTime -= Time.deltaTime; // Decrement the spawn time
// 		}
// 	}
// }