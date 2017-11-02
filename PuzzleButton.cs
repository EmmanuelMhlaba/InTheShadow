using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : MonoBehaviour {
	public Text puzzleText;
	public Text puzzleStatus;
	private PuzzleData _puzzleData = null;
	private string _locked = "Locked";
	private string _complete = "Complete";
	private string _unlocked = "Unlocked";
	private bool _tester = false;

	public void SetData (PuzzleData puzzleData, bool tester) {
		puzzleText.text = puzzleData.name;
		_tester = tester;
		_puzzleData = puzzleData;
		if (puzzleData.locked == true && tester == false) {
			puzzleStatus.text = _locked;
		} else {
			if (puzzleData.complete == true) {
				puzzleStatus.text = _complete;
			} else {
				puzzleStatus.text = _unlocked;
			}
		}
	}

	public void Play () {
		if ((_puzzleData != null && puzzleStatus.text != _locked)
		    || (_puzzleData != null && _tester == true)) {
			GameManager.Instance.StartGame (_puzzleData.name);
			if (_tester == false) {
				UIManager.Instance.HideNormalMenu ();
			} else {
				UIManager.Instance.HideTesterMenu ();
			}
			AudioManager.Instance.PlaySound ("NormalButton");
		} else {
			AudioManager.Instance.PlaySound ("BackButton");
		}
	}
}
