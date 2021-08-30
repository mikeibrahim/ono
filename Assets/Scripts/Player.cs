using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	#region Variables
	private Weapon weapon;
	private static Vector3	wasd = Vector2.zero,
							velocity = Vector2.zero;
	private static int maxHeath = 1;
	private static float	speed = 15,
							maxGravity = -9.81f,
							maxFriction = 0.5f;
	private int currentHealth = maxHeath;
	private float 	currentFriction = maxFriction,
					currentGravity = maxGravity;
	#endregion

	#region Public Methods
	public void Init() {
		weapon = GetComponentInChildren<Weapon>();
	}
	// Handles lives
	public void Damage() {
		currentHealth--;
		if (currentHealth <= 0) {
			Die();
		}
	}
	// Handles death
	public void Die() {
		gameObject.SetActive(false);
	}
	#endregion
	#region Private Methods
	// Get the user's input
	private void GetWasdInput() {
		if (Input.GetKey(KeyCode.W)) wasd.y = 1;
		else if (Input.GetKey(KeyCode.S)) wasd.y = -1;
		else wasd.y = 0;
		if (Input.GetKey(KeyCode.A)) wasd.x = -1;
		else if (Input.GetKey(KeyCode.D)) wasd.x = 1;
		else wasd.x = 0;
	}
	// Update the player's movement direction based on the WASD input
	private void SetVelocity() {
		if (wasd.x != 0) velocity.x = wasd.x * speed;
		if (wasd.y != 0) velocity.y = wasd.y * speed;
	}
	private void Gravity() => velocity.y += currentGravity * 2f * Time.deltaTime;
	// Makes the player bounce off of the walls
	private void Bounce(RaycastHit2D hit) {
		velocity *= currentFriction;
		// reflect the velocity in the collision normal
		velocity = Vector3.Reflect(velocity, hit.normal);
	}
	// Update the player's position
	private void Movement() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			GetWasdInput(); // Get WASD input
			SetVelocity(); // Change velocity
		}
		Gravity();
		if (IsGrounded()) {
			velocity.y = Mathf.Max(0, velocity.y); // make it so gravity doesnt effect when on the grounded
			int heavyMult = currentFriction != maxFriction ? 10 : 1;
			velocity.x -= velocity.x * heavyMult * currentFriction * Time.deltaTime; // friction
		}
		UpdatePosition(); // Move the player
	}
	// Handles collisions and position
	private void UpdatePosition() {
		if (velocity.magnitude <= 0.1) return; // if the velocity is 0, don't move
		// circlecast in the direction the player is moving
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.5f, velocity, velocity.magnitude * Time.deltaTime);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.gameObject != this.gameObject) {
				Bounce(hit);
				break;
			}
		}
		// if the player is not going to hit anything, move the player
		if (hits.Length == 1) {
			transform.position += velocity * Time.deltaTime;
		}
	}
	// Handles shooting
	private void ShootWeapon() {
		if (Input.GetMouseButtonDown(0)) {
			weapon.Shoot();
		}
	}
	// Player heavy when shifting
	private void PlayerHeavy() {
		if (Input.GetKey(KeyCode.LeftShift)) {
			currentFriction = maxFriction * 0.5f; // double friction force
			currentGravity = maxGravity * 2.0f; // double gravity
		} else {
			currentFriction = maxFriction;
			currentGravity = maxGravity;
		}
	}
	#endregion

	#region Booleans
	private bool IsGrounded() => Physics2D.RaycastAll(transform.position, Vector2.down, 0.501f).Length > 1;
	#endregion

	#region Callbacks
	private void Update() {
		Movement(); // Move the player
		ShootWeapon(); // Shoot the weapon
		PlayerHeavy(); // Player heavy when shifting
	}
	
	#endregion
}