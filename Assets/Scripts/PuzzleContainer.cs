using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleContainer : MonoBehaviour {
	private float rotationSpeed = 5f;
	public float errorMargin = 0f;
	public bool primaryContainer;
	
	void Update () {
		if (GameManager.Instance.Playing == true){
			if (GameManager.Instance.PrimaryContainer == primaryContainer) {
				RotateAndMove ();
			}
			if (CheckRotation () == true && GameManager.Instance.tester == false) {
				GameManager.Instance.PuzzleCorrect (primaryContainer);
			}
		}
	}
	
	private void RotateAndMove() {
		if (Input.GetMouseButton (0)) {
			float x = Input.GetAxis ("Mouse X");
			float y = Input.GetAxis ("Mouse Y");
			if (Input.GetKey (KeyCode.LeftShift) == true) {
				Vector3 tmp = transform.position;
				tmp.x += x * Time.deltaTime;
				tmp.y += y * Time.deltaTime;
				if (Compare (tmp.x, 0, 0.5f) && Compare (tmp.y, 1, 0.5f)) {
					transform.position = tmp;
				}
			} else {
				x *= rotationSpeed;
				y *= rotationSpeed;
				if (PuzzleManager.Instance.CurrentPuzzle.verticalMovement == true &&
				   PuzzleManager.Instance.CurrentPuzzle.horizontalMovement == true) {
					transform.Rotate (x, y, 0);
				} else if (PuzzleManager.Instance.CurrentPuzzle.verticalMovement == true) {
					transform.Rotate (x, 0, 0);
				} else {
					transform.Rotate (0, y, 0);
				}
			}
		}
	}

	public void RandomizeRotation () {
		float x = Random.Range (0, 360);
		float y = Random.Range (0, 360);
		float z = Random.Range (0, 360);
		if (PuzzleManager.Instance.CurrentPuzzle.verticalMovement == true &&
			PuzzleManager.Instance.CurrentPuzzle.horizontalMovement == true) {
			transform.rotation = Quaternion.Euler( new Vector3 (x, y, z));
		} else if (PuzzleManager.Instance.CurrentPuzzle.verticalMovement == true) {
			transform.rotation = Quaternion.Euler( new Vector3 (x, 0, 0));
		} else {
			transform.rotation = Quaternion.Euler( new Vector3 (0, y, 0));
		}
	}

	private float To360 (float f) {
		if (f < 0) {
			return 180 + (180 - Mathf.Abs (f));
		}
		return f;
	}

	private bool Compare (float ang1, float ang2, float margin) {
		if (ang1 >= ang2 - margin && ang1 <= ang2 + margin) {
			return true;
		}
		return false;
	}

	private bool CheckRotation () {
		if (PuzzleManager.Instance.CurrentPuzzle != null) {
			float x = transform.rotation.eulerAngles.x;
			float y = transform.rotation.eulerAngles.y;
			float z = transform.rotation.eulerAngles.z;
			float CorrectX = To360 (PuzzleManager.Instance.CurrentPuzzle.correctContainerRotation.x);
			float CorrectY = To360 (PuzzleManager.Instance.CurrentPuzzle.correctContainerRotation.y);
			float CorrectZ = To360 (PuzzleManager.Instance.CurrentPuzzle.correctContainerRotation.z);
			if (Compare (x, CorrectX, errorMargin) && Compare (y, CorrectY, errorMargin) && Compare (z, CorrectZ, errorMargin)) {
				return true;
			}
		}
		return false;
	}
}
