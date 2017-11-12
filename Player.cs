using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Controller2D))]

public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float moveSpeed = 6;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff; 
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	Vector2 directionalInput;

	bool wallSliding;
	int wallDirX;

	// Count for collectibles
	private int count;
	public Text scoreCount;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		print("Gravity: " + gravity + "  Jump velocity: " + maxJumpVelocity);
		count = 0;
		SetScoreText ();

	}

	// Update is called once per frame
	void Update () {
		if (controller.restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				controller.enabled = false;
				SceneManager.LoadScene ("Testing");
			}
		} else {
			CalculateVelocity ();
			HandleWallSliding ();

			// Move player via Move() method in Controller2D
			controller.Move (velocity * Time.deltaTime, directionalInput);

			// If statement so gravity doesn't "build up" and raycasts don't keep increasing
			if (controller.collisions.above || controller.collisions.below) {
				if (controller.collisions.slidingDownMaxSlope) {
					velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
				} else {
					velocity.y = 0;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Pick Up") {
			other.gameObject.SetActive (false);
			count++;
			SetScoreText ();
		}
	}

	void SetScoreText(){
		scoreCount.text = "Score: " + count.ToString ();
	}

	public void SetDirectionalInput(Vector2 input){
		directionalInput = input;
	}

	public void OnJumpInputDown(){
		// If player is sliding down wall, needs to be able to jump
		if (wallSliding) {
			// If player is jumping up same wall
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			} 
			// If player has no input on x axis
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			} 
			// If player is jumping away from wall
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			} 
		}

		// If player is on ground
		if (controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				// Not jumping against max slope
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) {
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} 
			else {
				
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp(){
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	void HandleWallSliding(){
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {

				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} 
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}
		}
	}


	void CalculateVelocity(){

		// Set velocity of x direction and gravity in y direction
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
}
