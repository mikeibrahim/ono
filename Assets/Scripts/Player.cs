using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Rigidbody2D rb; // handles physics and collisions
	private Vector2 wasd = new Vector2(0, 0); // 
	private float speed = 10; // how fast the player moves
	private float gravity = -9.81f; // how fast the player falls
	private float friction = 0.6f; // how fast the player slows down
	private Vector2 velocity = new Vector2(0, 0);

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

	private void Bounce(Collision2D collision) {
		velocity *= friction;
		// reflect the velocity in the collision normal
		velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
	}

	// Update the player's position
	private void Move() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			GetWasd(); // Get WASD input
			ChangeVelocity(); // Change velocity
		}
		Gravity();
		rb.MovePosition(rb.position + velocity * Time.deltaTime);
	}

    private void Awake() {
        rb = GetComponent<Rigidbody2D>(); // Get the player's rigidbody
    }

    private void Update() {
		Move(); // Move the player
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		Bounce(collision); // Handle collisions
	}
}
