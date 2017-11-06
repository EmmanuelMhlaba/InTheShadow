using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

public class PuzzleManager : MonoBehaviour {
	public static PuzzleManager Instance { get; private set; }
	public PuzzleDatabase puzzleDb;
	public PuzzleModel[] models;
	public PuzzleContainer puzzleContainer;
	public PuzzleContainer puzzleContainer2;
	private string _path;
	private GameObject[] _instantiatedModels = new GameObject[2];
	public PuzzleData CurrentPuzzle { get; private set; }
	public PuzzleTray puzzleTray;
	
	void Awake () {
		if (Instance == null) {
			Instance = this;
			_path = Application.dataPath + "/Puzzles.xml";
			LoadData ();
			CurrentPuzzle = null;

		} else {
			Destroy (gameObject);
		}
	}
	
	public void SaveData () {
		XmlSerializer serializer = new XmlSerializer (typeof(PuzzleDatabase));
		FileStream stream = new FileStream (_path, FileMode.Create);
		serializer.Serialize (stream, puzzleDb);
		stream.Close ();
	}
	
	public void LoadData () {
		if (File.Exists(_path) == true) {
			XmlSerializer serializer = new XmlSerializer (typeof(PuzzleDatabase));
			FileStream stream = new FileStream (_path, FileMode.Open);
			puzzleDb = (PuzzleDatabase)serializer.Deserialize (stream);
			stream.Close ();
		} else {
			Debug.LogWarning ("File: " + _path + " does not exist.");
		}
	}
	
	private PuzzleData FindData (string name) {
		foreach (PuzzleData pd in puzzleDb.puzzles) {
			if (pd.name == name) {
				return pd;
			}
		}
		return null;
	}
	
	private PuzzleModel FindModel (int index) {
		foreach (PuzzleModel pm in models) {
			if (pm.modelIndex == index) {
				return pm;
			}
		}
		return null;
	}
	
	public void LoadPuzzle (string name) {
		CurrentPuzzle = FindData (name);
		if (CurrentPuzzle != null) {
			PuzzleModel pm = FindModel (CurrentPuzzle.modelIndex);
			if (pm != null) {
				UnloadCurrentPuzzle ();
				_instantiatedModels[0] = Instantiate (pm.model);
				_instantiatedModels[0].transform.SetParent (puzzleContainer.transform);
				_instantiatedModels[0].transform.localRotation = Quaternion.Euler(CurrentPuzzle.correctRotation);
				_instantiatedModels[0].transform.localScale = CurrentPuzzle.correctScale;
				_instantiatedModels[0].transform.localPosition = new Vector3 (0f, 0f, 0f);
				puzzleContainer.RandomizeRotation ();
				if (CurrentPuzzle.multipleModels == true) {
					LoadSecondModel (pm.model2);
				}
			} else {
				Debug.LogWarning ("Model at index " + CurrentPuzzle.modelIndex + " could not be found.");
			}
		} else {
			Debug.LogWarning ("Puzzle: " + name + " could not be found.");
		}
	}

	private void LoadSecondModel (GameObject model) {
		_instantiatedModels[1] = Instantiate (model);
		_instantiatedModels[1].transform.SetParent (puzzleContainer2.transform);
		_instantiatedModels[1].transform.localRotation = Quaternion.Euler(CurrentPuzzle.correctRotation2);
		_instantiatedModels[1].transform.localScale = CurrentPuzzle.correctScale2;
		_instantiatedModels[1].transform.localPosition = new Vector3 (0f, 0f, 0f);
		puzzleContainer2.RandomizeRotation ();
	}

	public void UnloadCurrentPuzzle () {
		if (_instantiatedModels[0] != null) {
			Destroy (_instantiatedModels[0]);
			_instantiatedModels[0] = null;
			if (CurrentPuzzle.multipleModels == false)
				CurrentPuzzle = null;
		}
		if (_instantiatedModels [1] != null && CurrentPuzzle.multipleModels == true) {
			Destroy (_instantiatedModels [1]);
			_instantiatedModels [1] = null;
			CurrentPuzzle = null;
		}
	}

	public void PuzzleComplete () {
		CurrentPuzzle.complete = true;
		UnlockNext ();
		puzzleTray.UpdateTray ();
	}

	private void UnlockNext () {
		bool foundNext = false;
		foreach (PuzzleData pd in puzzleDb.puzzles) {
			if (foundNext == true) {
				pd.locked = false;
				foundNext = false;
			} else {
				if (pd.name == CurrentPuzzle.name) {
					foundNext = true;
				}
			}
		}
	}
}

[Serializable]
public class PuzzleModel {
	public  int modelIndex;
	public GameObject model;
	public GameObject model2;
}

[Serializable]
public class PuzzleData {
	public string name;
	public bool locked;
	public bool complete;
	public int timeLimit;
	public bool verticalMovement;
	public bool horizontalMovement;
	public int modelIndex;
	public bool multipleModels;
	public Vector3 correctRotation;
	public Vector3 correctScale;
	public Vector3 correctPosition;
	public Vector3 correctContainerRotation;
	public Vector3 correctContainerPosition;
	public Vector3 correctRotation2;
	public Vector3 correctScale2;
	public Vector3 correctPosition2;
	public Vector3 correctContainerRotation2;
	public Vector3 correctContainerPosition2;
}

[Serializable]
public class PuzzleDatabase {
	public List<PuzzleData> puzzles = new List<PuzzleData> ();
}
