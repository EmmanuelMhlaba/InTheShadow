using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager Instance { get; private set; }
	public Text titleText = null;
	public Text timeText = null;
	public GameObject startScreen = null;
	public GameObject mainMenu = null;
	public GameObject normalMenu = null;
	public GameObject testerMenu = null;
	public GameObject pauseMenu = null;
	public GameObject retryMenu = null;
	public GameObject winMenu = null;

	void Awake () {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public void SetTitleText (string text) {
		if (titleText != null) {
			titleText.text = text;
		} else {
			Debug.LogWarning ("Text object is not attached.");
		}
	}

	public void SetTimeText (string text) {
		if (timeText != null) {
			timeText.text = text;
		} else {
			Debug.LogWarning ("Text object is not attached.");
		}
	}

	private void TriggerMenu (GameObject menu, string trigger) {
		if (menu != null) {
			Animator animator = menu.GetComponent<Animator> ();
			if (animator != null) {
				animator.SetTrigger (trigger);
			} else {
				Debug.LogWarning ("Could not get object animator.");
			}
		} else {
			Debug.LogWarning ("Game object is not attached.");
		}
	}

	public void HidePauseMenu () {
		TriggerMenu (pauseMenu, "Hide");
	}

	public void ShowPauseMenu () {
		TriggerMenu (pauseMenu, "Show");
	}

	public void ShowMainMenu () {
		TriggerMenu (mainMenu, "Show");
	}

	public void HideMainMenu () {
		TriggerMenu (mainMenu, "Hide");
	}

	public void ShowStartScreen () {
		TriggerMenu (startScreen, "Show");
	}

	public void HideStartScreen () {
		TriggerMenu (startScreen, "Hide");
	}

	public void ShowNormalMenu () {
		TriggerMenu (normalMenu, "Show");
	}

	public void HideNormalMenu () {
		TriggerMenu (normalMenu, "Hide");
	}

	public void ShowTesterMenu () {
		TriggerMenu (testerMenu, "Show");
	}

	public void HideTesterMenu () {
		TriggerMenu (testerMenu, "Hide");
	}

	public void ShowRetryMenu () {
		TriggerMenu (retryMenu, "Show");
	}

	public void HideRetryMenu () {
		TriggerMenu (retryMenu, "Hide");
	}

	public void ShowWinMenu () {
		TriggerMenu (winMenu, "Show");
	}

	public void HideWinMenu () {
		TriggerMenu (winMenu, "Hide");
	}
}
