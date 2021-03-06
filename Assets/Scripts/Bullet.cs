using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	private static float speed = 50f;
	
    private void FixedUpdate() {
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.forward, speed * Time.deltaTime);
		foreach (RaycastHit2D hit in hits) {
			// If its a tile
			if (hit.collider.gameObject.GetComponent<Tile>()) {
				int moveX = transform.eulerAngles.z > 180 ? 1 : -1; // move the block in the direction of the bullet's x dir
				hit.collider.gameObject.GetComponentInParent<Block>()?.MoveBlock(moveX, 0);
				// hit.collider.gameObject.GetComponent<Tile>()?.BreakFromBlock();

				// Destroy the bullet
				Destroy(gameObject);
			}
		}
		transform.position += transform.up * speed * Time.deltaTime;
    }
}
