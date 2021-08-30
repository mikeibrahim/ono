using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTile : Tile {
	#region Variables
	private SpriteRenderer spriteRenderer;
	private static float transparency = 0.5f;
	private static Color startColor = new Color(1f, 1f, 1f, transparency);
	private Color finalColor;
	private static int numLevels = 3;
	int currentLevel = 0;
	float stepInterval, currentStepInterval;
	#endregion

	#region Public Methods
    public void Init(int x, int y, Color c, float stepInterval) {
        transform.localPosition = new Vector3(x, y, 0);
		this.stepInterval = stepInterval;
		currentStepInterval = stepInterval;
		finalColor = new Color(c.r, c.g, c.b, transparency);
		spriteRenderer.color = Color.Lerp(startColor, finalColor, 1.0f / numLevels);
    }
	#endregion

	#region Private Methods
	private void IncreaseLevel() {
		currentLevel++;
		if (currentLevel > numLevels) Explode();
		else spriteRenderer.color = Color.Lerp(startColor, finalColor, (float)currentLevel / numLevels);
	}
	private void Explode() {
		// rectcastall to find all objects in the explosion radius
		Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - new Vector3(0.45f, 0.45f), transform.position + new Vector3(0.45f, 0.45f));
		foreach (Collider2D collider in colliders) collider.gameObject.GetComponent<Player>()?.Damage();
		Destroy(gameObject);
	}
	#endregion

	#region Unity Callbacks
	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	private void FixedUpdate() {
		if (currentStepInterval <= 0) {
			IncreaseLevel();
			currentStepInterval = stepInterval;
		} else currentStepInterval -= Time.fixedDeltaTime;
	}
	#endregion
}