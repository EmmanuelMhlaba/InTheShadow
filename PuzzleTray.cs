using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTray : MonoBehaviour {
	public PuzzleButton puzzleButton = null;
	public bool tester = false;

	public void UpdateTray () {
		if (puzzleButton != null) {
			foreach (Transform child in transform) {
				Destroy (child.gameObject);
			}
			foreach (PuzzleData pd in PuzzleManager.Instance.puzzleDb.puzzles) {
				PuzzleButton pb = Instantiate (puzzleButton);
				pb.SetData (pd, tester);
				pb.transform.SetParent (transform);
			}
		}
	}

	void Start () {
		UpdateTray ();
	}
}
