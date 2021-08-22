using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	// private Rigidbody2D rb; // handles physics and collisions
	private Vector2 wasd = new Vector2(0, 0); // 
	private float speed = 10; // how fast the player moves
	private float gravity = -9.81f; // how fast the player falls
	private float friction = 0.6f; // how fast the player slows down
	private Vector3 velocity = new Vector2(0, 0);
	private bool pressedSpace = false;

	// Get the player's direction of movement
	private void GetWasd() {
		if (Input.GetKey(KeyCode.W)) wasd.y = 1;
		else if (Input.GetKey(KeyCode.S)) wasd.y = -1;
		else wasd.y = 0;
		if (Input.GetKey(KeyCode.A)) wasd.x = -1;
		else if (Input.GetKey(KeyCode.D)) wasd.x = 1;
		else wasd.x = 0;
	}

	private void ChangeVelocity() {
		if (wasd.x != 0) velocity.x = wasd.x * speed;
		if (wasd.y != 0) velocity.y = wasd.y * speed;
	}

	private void Gravity() => velocity.y += gravity * 1.5f * Time.deltaTime;
	private bool IsGrounded() => Physics2D.RaycastAll(transform.position, Vector2.down, 0.51f).Length > 1;

	private void Bounce(RaycastHit2D hit) {
		velocity *= friction;
		// reflect the velocity in the collision normal
		velocity = Vector3.Reflect(velocity, hit.normal);
	}

	// Update the player's position
	private void Movement() {
		if (pressedSpace) {
			GetWasd(); // Get WASD input
			ChangeVelocity(); // Change velocity
			pressedSpace = false;
		}
		Gravity();
		if (IsGrounded()) velocity.y = Mathf.Max(0, velocity.y); // make it so gravity doesnt effect when on the ground
		UpdatePosition(); // Move the player
	}

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

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) pressedSpace = true;
	}

    private void FixedUpdate() {
		Movement(); // Move the player
    }
}
