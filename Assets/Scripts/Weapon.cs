using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	[SerializeField] private Bullet bulletPrefab;
	private static float timeBtwShots = 0f;
	float currentTimeBtwShots = timeBtwShots;

	private Vector3 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public void Shoot() {
		if (currentTimeBtwShots > 0) return;
		currentTimeBtwShots = timeBtwShots;

		Bullet bullet = Instantiate(bulletPrefab); // Create a new bullet prefab
		bullet.transform.position = transform.position; // Set the position of the bullet
		// Set the direction of the bullet to be facing the mousePosition
		Vector3 dir = GetMousePosition() - transform.position; // get the direction of the mouse
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 90; // get the angle of the direction
		bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // set the rotation of the bullet about the z axis
	}

	void FixedUpdate() {
		currentTimeBtwShots -= Time.deltaTime;
	}
}
