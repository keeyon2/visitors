using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
	
	private	GUIText clientMessage;
	private	GUIText serverMessage;
	private GameScreen _mainScreen;
	private SearchingView _searchingView;
	private PlayingView _playingView;
	private PauseMenu _pauseMenu;
	private GameEndMenu _gameEndMenu;
	private int currentLevel = -1;
	private int _weaponIndex = 0;
	private bool _searching;
	
	public AudioClip[] soundEffects = new AudioClip[2];
	//public AudioSource shootSound;
	//public AudioSource changeWeaponSound2;
	//public AudioClip changeWeaponSound;
	
	// Use this for initialization
	void Start () 
	{
		_searching = true;
		
		_mainScreen = (GameScreen)gameObject.AddComponent("GameScreen");
		_searchingView = (SearchingView)gameObject.AddComponent("SearchingView");
		_searchingView.Init();
		_searchingView.Size = new Vector2(_mainScreen.Size.x, _mainScreen.Size.y);
		_searchingView.SetPosition(new Vector2(0, 0), GameView.GameViewAnchor.BottomLeftAnchor);
		_searchingView.PauseButton.ButtonPressed += new EventHandler(PausePressed);
		_searchingView.PrintButton.ButtonPressed += new EventHandler(PrintButtonPressed);
		_searchingView.PlayWithoutButton.ButtonPressed += new EventHandler(PlayWithoutButtonPressed);
		_mainScreen.AddView(_searchingView);
		
		_playingView = (PlayingView)gameObject.AddComponent("PlayingView");
		_playingView.Init();
		_playingView.Size = new Vector2(_mainScreen.Size.x, _mainScreen.Size.y);
		_playingView.SetPosition(new Vector2(0, 0), GameView.GameViewAnchor.BottomLeftAnchor);
		_playingView.PauseButton.ButtonPressed += new EventHandler(PausePressed);
		_playingView.SwitchWeaponButton.ButtonPressed += new EventHandler(SwitchWeaponButtonPressed);
		_playingView.FireButton.ButtonPressed += new EventHandler(FireButtonPressed);
		_mainScreen.AddView(_playingView);
		
		_pauseMenu = (PauseMenu)gameObject.AddComponent("PauseMenu");
		_pauseMenu.Init();
		_pauseMenu.Size = _mainScreen.Size;
		_pauseMenu.Position = new Vector2(_mainScreen.Size.x / 2.0f, _mainScreen.Size.y / 2.0f);
		_pauseMenu.ResumeButton.ButtonPressed += new EventHandler(ResumePressed);
		_pauseMenu.MainMenuButton.ButtonPressed += new EventHandler(MenuPressed);
		_pauseMenu.ResetButton.ButtonPressed += new EventHandler(ResetPressed);
		_mainScreen.AddView(_pauseMenu);
		
		_gameEndMenu = (GameEndMenu)gameObject.AddComponent("GameEndMenu");
		_gameEndMenu.Init();
		_gameEndMenu.Size = _mainScreen.Size;
		_gameEndMenu.Position = new Vector2(_mainScreen.Size.x / 2.0f, _mainScreen.Size.y / 2.0f);
		_gameEndMenu.MainMenuButton.ButtonPressed += new EventHandler(MenuPressed);
		_gameEndMenu.ResetButton.ButtonPressed += new EventHandler(ResetPressed);
		_mainScreen.AddView(_gameEndMenu);
		
		
		//AudioClip[] soundEffects = GetComponents(AudioSource);
		
	}
	
	public void Update()
	{
		
		// If we are searching but the searching view is not showing...
		if(_searching && _searchingView.State != GameView.GameViewState.Showing)
		{
			// If the playing view is still showing, get rid of it
			if(_playingView.State == GameView.GameViewState.Showing)
			{
				_playingView.Hide(true);
			}
			// If the playing view is hidden, but the searching view is not showing, start showing it
			if(_playingView.State == GameView.GameViewState.Hidden && _searchingView.State != GameView.GameViewState.AnimatingIn)
			{
				_searchingView.Show(true);
			}
		}
		
		// If we are playing but the playing view is not showing...
		if(!_searching && _playingView.State != GameView.GameViewState.Showing)
		{
			// If the searching view is still showing, get rid of it
			if(_searchingView.State == GameView.GameViewState.Showing)
			{
				_searchingView.Hide(true);
			}
			// If the searching view is hidden, but the playing view is not showing, start showing it
			if(_searchingView.State == GameView.GameViewState.Hidden && _searchingView.State != GameView.GameViewState.AnimatingIn)
			{
				_playingView.Show(true);
			}
		}
		
<<<<<<< HEAD
=======
		// End game stuff
		_playingView.WeaponBar.Energy -= Time.deltaTime;
		if(_playingView.WeaponBar.Energy == 0 && !(_gameEndMenu.State == GameView.GameViewState.Showing) && !(_gameEndMenu.State == GameView.GameViewState.AnimatingIn))
		{
			ShowEndGameMenu();	
		}
>>>>>>> 94e841d0a742bec0af7989b27aa4f35a582be888
	}
			
	public void ResumePressed(object sender)	
	{
		DismissPauseMenu();
	}
	
	public void MenuPressed(object sender)
	{
		HUDGameViewMenuButtonPressed();
	}
	
	public void ResetPressed(object sender)
	{
		ResetLevel();
	}
	
	public void SwitchWeaponButtonPressed(object sender)
	{
		audio.clip = soundEffects[1];
		audio.Play();
		
		if(_weaponIndex == 0) _weaponIndex = 1;
		else _weaponIndex = 0;
		_playingView.Switcher.CurrentWeapon = _weaponIndex;
	}
	
	public void ShowPauseMenu()
	{
		_pauseMenu.Show(true);
		_playingView.HasFocus = false;
		_searchingView.HasFocus = false;
	}
	
	
	
	public void PausePressed(object sender)
	{
		ShowPauseMenu();
	}
	
	public void DismissPauseMenu()
	{
		_pauseMenu.Hide(true);
		_playingView.HasFocus = true;
		_searchingView.HasFocus = true;
	}
	
	public void PrintButtonPressed(object sender)
	{
		UtilityPlugin.PrintARCard();
	}
	
	public void PlayWithoutButtonPressed(object sender)
	{
		if (GameState.GetCurrentLevel().Contains("Egypt"))
			Application.LoadLevel("SP-Egypt-NonAR");
		else if (GameState.GetCurrentLevel().Contains("Castle"))
			Application.LoadLevel("SP-Castle-NonAR");
		_searching = false;
	}
	
	public void SwitchToPlayingView()
	{
		_searching = false;
	}
	public void SwitchToSearchingView()
	{
		_searching = true;
	}

	// IHUDGameViewController methods
	public void HUDGameViewWeaponsSwitched(int newWeapon)
	{
		_weaponIndex = newWeapon;

	}
	
	public void FireButtonPressed(object sender)
	{

//		_gameView.Energy = _gameView.Energy - 1.0f;
//		GameObject cam = GameObject.Find("ARCamera");
//		Debug.Log(cam.transform.position);
//		GameObject thePrefab = (GameObject)Resources.Load("StrongBall");
//		GameObject instance = (GameObject)Network.Instantiate(thePrefab, cam.transform.position, cam.transform.rotation, 0);
//		Vector3 fwd = cam.transform.forward * 50000;
//		instance.rigidbody.AddForce(fwd);
		

		audio.clip = soundEffects[0];
		audio.pitch = Random.Range(0.9F, 1.1F);
		audio.Play();
		//soundEffects[0].pitch = Random.Range(0.9F, 1.1F);
		//soundEffects[0].Play();
		
		//audio.clip = shootSound;
		//shootSound.pitch = Random.Range(0.9F, 1.1F);
		//shootSound.Play();
	
		
		GameObject cam = GameObject.Find("ARCamera");
		Vector3 fwd = cam.transform.forward * 500;
		ShootWithoutNetworkInstantiate(cam.transform.position, cam.transform.rotation, fwd);
		_playingView.WeaponBar.Energy = _playingView.WeaponBar.Energy - 1.0f;

		
		//networkView.RPC("ShootWithoutNetworkInstantiate",RPCMode.All);
		
		/*
		GameObject thePrefab = (GameObject)Resources.Load("StrongBall");
		GameObject instance = (GameObject)Network.Instantiate(thePrefab, cam.transform.position, cam.transform.rotation, 0);
		Vector3 fwd = cam.transform.forward * 50000;
		instance.rigidbody.AddForce(fwd);
		*/
	}
	
	
	public void ShootWithoutNetworkInstantiate(Vector3 position, Quaternion rotation, Vector3 fwd)
	{
		bool primaryWeapon = (_weaponIndex == 0);
		string weaponID = "";
		if (primaryWeapon)
			weaponID = GameState.LoadPrimaryWeapon();
		else
			weaponID = GameState.LoadSecondaryWeapon();
		Debug.Log("Shooting: " + weaponID + ". Primary: " + primaryWeapon);
		Transform spawn = (Transform) Resources.Load(weaponID, typeof(Transform));
		
		Transform newWeapon = (Transform)Instantiate(spawn, position, rotation);
		newWeapon.rigidbody.AddForce(fwd);
	}
	
	public void HUDGameViewMenuButtonPressed()
	{
		Application.LoadLevel("Main Menu");
	}
	
	public void ResetLevel()
	{
		if (currentLevel != Application.loadedLevel)
		{
			currentLevel = Application.loadedLevel;
		}
		Application.LoadLevel(currentLevel);
	}
	
	public void ShowEndGameMenu()
	{
		_searchingView.HasFocus = false;
		_playingView.HasFocus = false;
		_gameEndMenu.Show(true);
	}
}