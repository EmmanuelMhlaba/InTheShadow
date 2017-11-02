using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance { get; private set; }
	public float TimeLeft { get; private set; }
	public bool Playing { get; private set; }
	public bool PrimaryContainer { get; private set; }
	private float _rate = -1f;
	private string _puzzle;
	private bool[] _correctPuzzles = new bool[2];
	public bool tester = false;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			Playing = false;
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {
		UIManager.Instance.ShowStartScreen ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) == true) {
			if (Playing == true) {
				PauseGame ();
			} else {
				UnpauseGame ();
			}
		}
		if (Playing == true) {
			UIManager.Instance.SetTimeText (TimeLeft.ToString ());
			if (Input.GetKeyDown (KeyCode.LeftControl) == true &&
				PuzzleManager.Instance.CurrentPuzzle.multipleModels == true) {
				PrimaryContainer = !PrimaryContainer;
			}
			if (_correctPuzzles [0] && (_correctPuzzles [1] ||
				PuzzleManager.Instance.CurrentPuzzle.multipleModels == false)) {
				GameWon ();
			}
		}
	}

	private void Countdown () {
		if ((TimeLeft += _rate) == 0f) {
			CancelInvoke ("Countdown");
			GameOver ();
		}
	}

	public void StartGame (string name) {
		_puzzle = name;
		_correctPuzzles [0] = false;
		_correctPuzzles [1] = false;
		PuzzleManager.Instance.LoadPuzzle (name);
		PrimaryContainer = true;
		if (PuzzleManager.Instance.CurrentPuzzle != null) {
			TimeLeft = PuzzleManager.Instance.CurrentPuzzle.timeLimit;
			UIManager.Instance.SetTitleText (name);
			Playing = true;
			_rate = -1f;
			if (tester == false) {
				InvokeRepeating ("Countdown", 1f, 1f);
			}
		} else {
			Debug.LogWarning ("Puzzle: " + name + " could not be loaded.");
		}
	}

	public void PauseGame () {
		if (Playing == true) {
			_rate = 0f;
			Playing = false;
			UIManager.Instance.ShowPauseMenu ();
		}
	}

	public void UnpauseGame () {
		if (Playing == false && PuzzleManager.Instance.CurrentPuzzle != null) {
			_rate = -1f;
			Playing = true;
			UIManager.Instance.HidePauseMenu ();
		}
	}

	private void EndGame () {
		Playing = false;
		PuzzleManager.Instance.UnloadCurrentPuzzle ();
		UIManager.Instance.SetTimeText ("");
		UIManager.Instance.SetTitleText ("");
		CancelInvoke ("Countdown");
	}

	public void GameOver () {
		EndGame ();
		UIManager.Instance.ShowRetryMenu ();
	}

	public void StopGame () {
		EndGame ();
		UIManager.Instance.ShowMainMenu ();
		UIManager.Instance.HidePauseMenu ();
	}

	public void RestartGame () {
		StartGame (_puzzle);
		UIManager.Instance.HideRetryMenu ();
	}

	public void ReplayGame () {
		StartGame (_puzzle);
		UIManager.Instance.HideWinMenu ();
	}

	public void ExitGame () {
		PuzzleManager.Instance.SaveData ();
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit ();
		#endif
	}

	private void GameWon () {
		PuzzleManager.Instance.PuzzleComplete ();
		EndGame ();
		UIManager.Instance.ShowWinMenu ();
		PuzzleManager.Instance.SaveData ();
	}

	public void PuzzleCorrect (bool primaryContainer) {
		if (primaryContainer == true) {
			_correctPuzzles [0] = true;
		} else {
			_correctPuzzles [1] = true;
		}
	}
}
