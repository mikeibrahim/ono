/*
Summary:
	[+] = Added; [*] = Not Added Yet; [-] = Probably Not Needed
	[+] Blocks fall from the top of the screen.
	[+] The player can move the blocks left and right.
	[+] The game charges up to destroys rows of blocks every so often (doesnt break blocks, only the player).
	[+] The game tracks the player with a growing meteorite.
	[*] Destroy blocks of 3x3 or more to gain points.
	[*] Player can obtain a shield/powerup midmatch which can save the player from being destroyed once.
	[*] Player gets a half second of invlunerability after getting hit.
	[*] Player bullets can be upgraded to pierce through multiple blocks
	[*] Player can purchase a lazer which can move through all blocks <-- is unlocked thrhough getting a certain score
	[-] The game charges up to destroys a 3x3 area of blocks at the player's location every so often.
	[-] The player can destroy a 3x3 area of blocks.
	[-] The player can destroy a row of blocks.

	How does the Player win?
		Its an endless game; you get more score as you destroy more blocks and stay alive longer.

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct blockType {
	public int[,] pattern;
	public Color color;

	public blockType(int[,] pattern, Color color) {
		this.pattern = pattern;
		this.color = color;
	}
}

public static class Blocks {
	// make all the tetris blocks
	public static blockType[] blocks = {
		new blockType(
			new int[,] {
				{1},
				{1},
				{1},
				{1}
			}, Color.cyan
		),
		new blockType(
			new int[,] {
				{1,1},
				{1,1}
			}, Color.yellow
		),
		new blockType(
			new int[,] {
				{0,1,0},
				{1,1,1}
			}, Color.magenta
		),
		new blockType(
			new int[,] {
				{0,1,1},
				{1,1,0}
			}, Color.green
		),
		new blockType(
			new int[,] {
				{1,1,0},
				{0,1,1}
			}, Color.red
		),
		new blockType(
			new int[,] {
				{1,0,0},
				{1,1,1}
			}, Color.blue
		),
		new blockType(
			new int[,] {
				{0,0,1},
				{1,1,1}
			}, new Color(1,0.647f,0f) // orange
		)
	};

	public static int numBlocks = blocks.Length;
}

public class GameManager : MonoBehaviour {
	#region Variables
	[SerializeField] private Block blockPrefab;
	[SerializeField] private Tile tilePrefab;
	[SerializeField] private LaserTile laserTilePrefab;
	[SerializeField] private MeteorTile meteorTilePrefab;
	[SerializeField] private Player playerPrefab;
	[SerializeField] private Weapon weaponPrefab;
	public static GameManager INST;
	private Player player;
	private Tile[,] board = new Tile[BOARD_WIDTH, BOARD_HEIGHT];
	private List<Block> blocks = new List<Block>();
	private static Color laserColor = Color.red,
						 meteorColor = new Color(1,0.647f,0f); // orange;
	private static int	BOARD_WIDTH = 25,
						BOARD_HEIGHT = 20,
						BOARD_SIZE = BOARD_WIDTH * BOARD_HEIGHT,
						minGroupSize = 2, // + 1 (but it is easier to be inclusive)
						extraLazers = 1;
	private static float spawnInterval = 5f,
						 stepInterval = 1f,
						 laserSpawnInterval = 4f,
						 meteorSpawnInterval = 5f,
						 meteorTileSpeed = 4f,
						 meteorTileSize = 4f,
						 meteorTileLifeTime = 3f;
	private float	currentSpawnInterval = spawnInterval,
					currentStepInterval = stepInterval,
					currentLaserSpawnInterval = laserSpawnInterval,
					currentMeteorSpawnInterval = meteorSpawnInterval;
	#endregion

	#region Public Methods
	public Tile[,] GetBoard() => board;
	public Block GetBlockPrefab() => blockPrefab;
	public int GetBoardWidth() => BOARD_WIDTH;
	public int GetBoardHeight() => BOARD_HEIGHT;
	// Check if the tile is in the bounds of the board
	public bool InBounds(int x, int y) => x >= 0 && x < BOARD_WIDTH && y >= 0 && y < BOARD_HEIGHT;
	public void RemoveTile(Tile t) => board[t.GetX(), t.GetY()] = null;
	public void RemoveBlock(Block b) => blocks.Remove(b);
	#endregion

	#region Private Methods
	// Make the border around the board
	private void CreateBorder() {
		for (int i = -1; i <= BOARD_WIDTH; i++)
			for (int j = -1; j <= BOARD_HEIGHT; j++)
				if (i == -1 || i == BOARD_WIDTH || j == -1 || j == BOARD_HEIGHT) {
					Tile tile = Instantiate(tilePrefab);
					tile.Init(i, j, Color.black);
				}
	}
	// Make the player
	private void CreatePlayer() {
		Player newPlayer = Instantiate(playerPrefab);
		newPlayer.transform.position = new Vector3(BOARD_WIDTH / 2, BOARD_HEIGHT / 3, 0);

		Weapon weapon = Instantiate(weaponPrefab);
		weapon.transform.SetParent(newPlayer.transform);
		weapon.transform.localPosition = new Vector3(0, 0, 0);
		newPlayer.Init();
		player = newPlayer;
	}
	// Make a block
	private void CreateBlock() {
		// Get a random block
		int patternIndex = UnityEngine.Random.Range(0, Blocks.numBlocks);
		blockType blockInfo = Blocks.blocks[patternIndex];
		int[,] pattern = blockInfo.pattern;
		pattern = CreateRandomRotationPattern(pattern); // Pick one of 4 new rotations
		Color color = blockInfo.color;

		// Create the block
		Block block = Instantiate(blockPrefab);
		block.Init(pattern, tilePrefab, color);
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
	// Move the camera to center of the board and resize it bruh i was asked to sit out of the quizlet because i was winning so much 
	private void ConfigureCamera() {
		Camera cam = Camera.main;
		cam.transform.position = new Vector3(BOARD_WIDTH / 2.0f, BOARD_HEIGHT / 2.0f - 0.5f, -10);
		// change camera size to fit the board
		cam.orthographicSize = BOARD_HEIGHT / 2.0f + 1;
	}
	// Make the laser tiles
	private void CreateLaserTiles() {
		bool isVertical = UnityEngine.Random.Range(0, 2) == 0;
		int target = isVertical ? UnityEngine.Random.Range(extraLazers, BOARD_WIDTH - extraLazers) : UnityEngine.Random.Range(extraLazers, BOARD_HEIGHT - extraLazers);
		int length = isVertical ? BOARD_HEIGHT : BOARD_WIDTH;
		for (int i = 0; i < length; i++) { // for each tile in the row/col of the laser
			for (int e = -extraLazers; e <= extraLazers; e++) { // for each of the laser's width tiles
				Vector2 laserPos = isVertical ? new Vector2(target + e, i) : new Vector2(i, target + e);
				LaserTile laserTile = Instantiate(laserTilePrefab);
				laserTile.Init((int)laserPos.x, (int)laserPos.y, laserColor, stepInterval);
			}
		}
	}
	// Make the meteor tiles
	private void CreateMeteorTile() {
		// int spawnX = UnityEngine.Random.Range(0, BOARD_WIDTH);
		// int spawnY = UnityEngine.Random.Range(0, BOARD_HEIGHT);
		int spawnX = (int)player.transform.position.x;
		int spawnY = (int)player.transform.position.y;
		MeteorTile meteorTile = Instantiate(meteorTilePrefab);
		meteorTile.Init(spawnX, spawnY, meteorColor, meteorTileSpeed, meteorTileSize, meteorTileLifeTime);
		meteorTile.SetTarget(player.transform);
	}
	// Check for block groups
	private void CheckForBlockGroups() {
		// List<Tile> tilesToRemove = new List<Tile>();
		for (int xOrigin = 0; xOrigin < BOARD_WIDTH; xOrigin++) {
			for (int yOrigin = 0; yOrigin < BOARD_HEIGHT; yOrigin++) {
				if (board[xOrigin, yOrigin] == null) continue;
				// We got our origin point of the group at (xOrigin, yOrigin)
				int groupWidth = 0, groupHeight = 0;
				// find the maximum possible width for the group
				while (InBounds(xOrigin + groupWidth + 1, yOrigin) && board[xOrigin + groupWidth + 1, yOrigin]) { // if in bounds and there is a tile there
					groupWidth++; // ingrease max width
				}
				while (InBounds(xOrigin, yOrigin + groupHeight + 1) && board[xOrigin, yOrigin + groupHeight + 1]) { // if in bounds and there is a tile above
					groupHeight++; // increase max height
				}
				if (groupWidth < minGroupSize || groupHeight < minGroupSize) continue;
				for (int h = 0; h <= groupHeight; h++) {
					for (int w = 0; w <= groupWidth; w++) {
						bool xUnderMinGroupSize = w < minGroupSize, yUnderMinGroupSize = h < minGroupSize;
						// If there is a missing tile in this current row
						if (InBounds(xOrigin + w, yOrigin + h) && !board[xOrigin + w, yOrigin + h]) {
							// If you cant cut off the group at this y
							if (yUnderMinGroupSize) {
								groupWidth = w - 1; // clamp the width to the current - 1
							} else {
								groupHeight = h - 1; // clamp the height to the current - 1
								break;
							}
						}
					}
				}
				// If the group is big enough
				if (groupWidth >= minGroupSize && groupHeight >= minGroupSize) {
					// delete the tiles
					for (int h = 0; h <= groupHeight; h++) {
						for (int w = 0; w <= groupWidth; w++) {
							if (!board[xOrigin + w, yOrigin + h]) continue;
							print("Removing tile at " + (xOrigin + w) + ", " + (yOrigin + h));
							board[xOrigin + w, yOrigin + h].BreakBlock();
							// board[xOrigin + w, yOrigin + h].BreakFromBlock();
							board[xOrigin + w, yOrigin + h] = null;
						}
					}
					// TODO: Add the score
				}
			}
		}
	}
	#endregion
	
	#region Callbacks
    private void Awake() {
		INST = this;
		// currentSpawnInterval = spawnInterval;
    }
	private void Start(	) {
		// Make the border
		CreateBorder();
		// Make the player
		CreatePlayer();
		// Center and size the camera
		ConfigureCamera();
	}

	// Handles the intervals and time-based events
	private void FixedUpdate() {
		// Spawn a block every spawnInterval seconds
		if (currentSpawnInterval <= 0) {
			CreateBlock();
			currentSpawnInterval = spawnInterval;
		} else currentSpawnInterval -= Time.deltaTime;

		// Move the blocks
		if (currentStepInterval <= 0) {
			foreach (Block block in blocks) block.MoveBlock(0, -1);
			currentStepInterval = stepInterval;
			CheckForBlockGroups();
		} else currentStepInterval -= Time.deltaTime;

		// Spawn a laser every laserSpawnInterval seconds
		if (currentLaserSpawnInterval <= 0) {
			CreateLaserTiles();
			currentLaserSpawnInterval = laserSpawnInterval;
		} else currentLaserSpawnInterval -= Time.deltaTime;

		// Spawn a meteor every meteorSpawnInterval seconds
		if (currentMeteorSpawnInterval <= 0) {
			CreateMeteorTile();
			currentMeteorSpawnInterval = meteorSpawnInterval;
		} else currentMeteorSpawnInterval -= Time.deltaTime;
	}
	#endregion
}