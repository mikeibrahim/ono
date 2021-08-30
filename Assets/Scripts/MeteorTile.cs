using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Starts off slowly, then accelerates to a max speed.
change color and size based on time to live

*/

public class MeteorTile : Tile {
	#region Variables
	private Transform target;
	private SpriteRenderer spriteRenderer;
	private static float transparency = 0.5f;
	private static Color startColor = new Color(1f, 1f, 1f, transparency / 3.0f);
	private Color finalColor;
	// Set by the game
	private float	maxSpeed,
					maxSize,
					maxLifetime;
	// Referencing the max variables
	private float	currentSpeed = 0,
					currentSize = 0,
					currentLifetime = 0;
	#endregion

	#region Public Methods
    public void Init(int x, int y, Color c, float maxSpeed, float maxSize, float maxLifetime) {
        transform.localPosition = new Vector3(x, y, 0); // Set the position
		finalColor = new Color(c.r, c.g, c.b, transparency); // Set the color
		
		// Init the variables
		this.maxSpeed = maxSpeed; this.maxSize = maxSize; this.maxLifetime = maxLifetime;

		UpdateColor();
    }
	public void SetTarget(Transform target) => this.target = target;
	#endregion

	#region Private Methods
	private void UpdateColor() => spriteRenderer.color = Color.Lerp(startColor, finalColor, currentLifetime / maxLifetime);
	private void UpdateSize() => transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * maxSize, currentSize / maxSize);
	private void AddSize() => currentSize = currentLifetime * maxSize / maxLifetime;
	private void AddSpeed() => currentSpeed = currentLifetime * maxSpeed / maxLifetime;
	private void AddLifetime() {
		currentLifetime += Time.deltaTime;
		if (currentLifetime >= maxLifetime) {
			Explode();
		}
	}
	private void MoveToTarget() {
		if (target == null) return;
		Vector3 direction = target.position - transform.position;
		float distance = direction.magnitude;
		if (distance < 0.1f) return;
		direction.Normalize();
		transform.position += direction * currentSpeed * Time.deltaTime; // Update meteor
	}
	private void Explode() {
		// rectcastall to find all objects in the explosion radius
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxSize / 2.0f);
		foreach (Collider2D collider in colliders) {
			collider.gameObject.GetComponent<Player>()?.Damage();

			// Blcok collisions
		}
		Destroy(gameObject);
	}
	#endregion

	#region Unity Callbacks
	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	private void FixedUpdate() {
		MoveToTarget();
		UpdateSize();
		UpdateColor();
		AddLifetime();
		AddSize();
		AddSpeed();
	}
	#endregion
}