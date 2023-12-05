using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	private Transform background_1, background_2;
	private Transform starry_layer_1_1, starry_layer_2_1;
	private Transform starry_layer_1_2, starry_layer_2_2;

	private Vector2 fastSpeed = new Vector2(5, 7);
	private Vector2 slowSpeed = new Vector2(1, 2);

	private int last_parallax_speed_layer_1 = 0;
	private int last_parallax_speed_layer_2 = 0;

	private int parallax_speed_layer_1 = 1;
	private int parallax_speed_layer_2 = 2;

	private int teleport_trigger_point = -24;
	private int teleport_destination = 24;

	private float t_val = 0f;

	private bool stopped = false;

    void Start()
    {
		background_1 = this.gameObject.transform.GetChild(0).GetChild(0);
		starry_layer_1_1 = this.gameObject.transform.GetChild(0).GetChild(1);
		starry_layer_2_1 = this.gameObject.transform.GetChild(0).GetChild(2);

		background_2 = this.gameObject.transform.GetChild(1).GetChild(0);
		starry_layer_1_2 = this.gameObject.transform.GetChild(1).GetChild(1);
		starry_layer_2_2 = this.gameObject.transform.GetChild(1).GetChild(2);
    }

    void Update()
    {
		if (!stopped){
			ParallaxMovement(
				background_1,
				parallax_speed_layer_1,
				last_parallax_speed_layer_1
			);
			ParallaxMovement(
				background_2,
				parallax_speed_layer_1,
				last_parallax_speed_layer_1
			);

			ParallaxMovement(
				starry_layer_1_1,
				parallax_speed_layer_1,
				last_parallax_speed_layer_1
			);
			ParallaxMovement(
				starry_layer_2_1,
				parallax_speed_layer_2,
				last_parallax_speed_layer_2
			);

			ParallaxMovement(
				starry_layer_1_2,
				parallax_speed_layer_1,
				last_parallax_speed_layer_1
			);
			ParallaxMovement(
				starry_layer_2_2,
				parallax_speed_layer_2,
				last_parallax_speed_layer_2
			);
		}
    }

	void ParallaxMovement(Transform background, int target_speed, int last_speed){
		if (background.position.y <= teleport_trigger_point){
			background.position = new Vector2(0, teleport_destination);
		} else {
			float velocityValue = Mathf.Lerp(last_speed, target_speed, t_val);
			t_val += .2f * Time.deltaTime;

			background.position += Vector3.down * Time.deltaTime * velocityValue;
		}
	}

	public void setParallaxSpeed(Vector2 speed){
		t_val = 0f;

		last_parallax_speed_layer_1 = parallax_speed_layer_1;
		last_parallax_speed_layer_2 = parallax_speed_layer_2;

		parallax_speed_layer_1 = (int) speed.x;
		parallax_speed_layer_2 = (int) speed.y;
	}

	public void goFast(){
		stopped = false;
		setParallaxSpeed(fastSpeed);
	}

	public void goSlow(){
		stopped = false;
		setParallaxSpeed(slowSpeed);
	}

	public void stopMotion(){
		stopped = true;
	}
	public void restartMotion(){
		stopped = false;
	}
}
