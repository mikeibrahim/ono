using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockPattern {
	// make all the tetris blocks
	public static int[][,] pattern =  {
		new int[,] {
			{1},
			{1},
			{1},
			{1}
		},
		new int[,] {
			{1,1},
			{1,1}
		},
		new int[,] {
			{0,1,0},
			{1,1,1}
		},
		new int[,] {
			{0,1,1},
			{1,1,0}
		},
		new int[,] {
			{1,1,0},
			{0,1,1}
		},
		new int[,] {
			{1,0,0},
			{1,1,1}
		},
		new int[,] {
			{0,0,1},
			{1,1,1}
		},
	};

	public static int numPatterns = pattern.Length;
}

public class GameManager : MonoBehaviour {
	public static GameManager INST;
	private static int BOARD_WIDTH = 15, BOARD_HEIGHT = 10, BOARD_SIZE = BOARD_WIDTH * BOARD_HEIGHT;
	private Tile[,] board = new Tile[BOARD_WIDTH, BOARD_HEIGHT];
	private List<Block> blocks = new List<Block>();
	private float spawnInterval = 2f, stepInterval = 0.5f;
	float currentSpawnInterval, currentStepInterval;
	[SerializeField] private Block blockPrefab;
	[SerializeField] private Tile tilePrefab;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform arenaHolder;
	// private Tile[,] board = null; // a 2D array of tiles

	// Make the border around the board
	private void CreateBorder() {
		for (int i = -1; i <= BOARD_WIDTH; i++)
			for (int j = -1; j <= BOARD_HEIGHT; j++)
				if (i == -1 || i == BOARD_WIDTH || j == -1 || j == BOARD_HEIGHT) {
					Tile tile = Instantiate(tilePrefab, arenaHolder.transform);
					tile.transform.SetParent(arenaHolder.transform);
					tile.Init(i, j, Color.black);
					// tile.SetAsBorder(); // Set the tile to be a border
				}
	}

	// Make the player
	private void CreatePlayer() {
		GameObject player = Instantiate(playerPrefab, arenaHolder.transform);
		player.transform.position = new Vector3(BOARD_WIDTH / 2, BOARD_HEIGHT / 3, 0);
	}

	// Make a block
	private void CreateBlock() {
		// Get a random pattern
		int patternIndex = UnityEngine.Random.Range(0, BlockPattern.numPatterns);
		int[,] pattern = BlockPattern.pattern[patternIndex];

		pattern = CreateRandomRotationPattern(pattern); // Pick one of 4 new rotations

		Block block = Instantiate(blockPrefab, arenaHolder.transform);
		Color c = new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
		block.Init(pattern, tilePrefab, c);
		block.PositionAtTop(BOARD_WIDTH, BOARD_HEIGHT);
		// Add the block's tiles to the board
		block.AddToBoard(board);
		blocks.Add(block);
	}

	// Create a random rotation pattern
	private int[,] CreateRandomRotationPattern(int[,] pattern) {
		int width = pattern.GetLength(0);
		int height = pattern.GetLength(1);
		int rot = UnityEngine.Random.Range(0, 4);
		if (rot % 2 == 1) (width, height) = (height, width);
		int[,] newPattern = new int[width, height];
		for (int w = 0; w < width; w++) {
			for (int h = 0; h < height; h++) {
				// Remapping the pattern to various rotations
				if (rot == 0) {
					newPattern[w, h] = pattern[w, h];
				} else if (rot == 1) {
					newPattern[w, h] = pattern[h, width - 1 - w];
				} else if (rot == 2) {
					newPattern[w, h] = pattern[width - 1 - w, height - 1 - h];
				} else if (rot == 3) {
					newPattern[w, h] = pattern[height - 1 - h, w];
				}
			}
		}
		return newPattern;
	}

    private void Awake() {
		INST = this;
		currentSpawnInterval = spawnInterval;
    }

	private void Start(	) {
		// Make the border
		CreateBorder();
		// Make the player
		CreatePlayer();
	}

	private void FixedUpdate() {
		// Spawn a block every spawnInterval seconds
		if (currentSpawnInterval <= 0) {
			CreateBlock();
			currentSpawnInterval = spawnInterval;
		} else {
			currentSpawnInterval -= Time.deltaTime;
		}

		// Move the blocks
		if (currentStepInterval <= 0) {
			foreach (Block block in blocks) {
				block.MoveBlock(board);
			}
			currentStepInterval = stepInterval;
		} else {
			currentStepInterval -= Time.deltaTime;
		}

	}
}