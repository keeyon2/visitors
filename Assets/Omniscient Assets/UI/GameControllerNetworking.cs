using UnityEngine;
using System.Collections;

public class GameControllerNetworking : MonoBehaviour 
{
	
	private	GUIText clientMessage;
	private	GUIText serverMessage;
	private GameScreen _mainScreen;
	private SearchingView _searchingView;
	private PlayingView _playingView;
	private PauseMenu _pauseMenu;
	private bool hasStarted = false;
	private int currentLevel = -1;
	private int _weaponIndex = 0;
	private bool _searching;
	
	// These two booleans determine whether or not the energy should be going down over time
	private bool _hasStarted;
	private bool _paused;
	
	public AudioClip[] soundEffects = new AudioClip[2];
	// Use this for initialization
	void Start () 
	{	
		_searching = true;
		_hasStarted = false;
		_paused = false;
		
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
		_playingView.WeaponSwitcher.WeaponSwitched += new EventHandler(WeaponsSwitched);
		_playingView.FireButton.ButtonPressed += new EventHandler(FireButtonPressed);
		_mainScreen.AddView(_playingView);
		
		_pauseMenu = (PauseMenu)gameObject.AddComponent("PauseMenu");
		_pauseMenu.Init();
		_pauseMenu.Size = _mainScreen.Size;
		_pauseMenu.Position = new Vector2(_mainScreen.Size.x / 2.0f, _mainScreen.Size.y / 2.0f);
		_pauseMenu.ResumeButton.ButtonPressed += new EventHandler(ResumePressed);
		_pauseMenu.MainMenuButton.ButtonPressed += new EventHandler(MenuPressed);
		_pauseMenu.ResetButton.ButtonPressed += new EventHandler(ResetPressed);
		print(_pauseMenu);
		_mainScreen.AddView(_pauseMenu);
		
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
		networkView.RPC("ResetLevel",RPCMode.All);
	}
	
	public void WeaponsSwitched(object sender)
	{
		audio.clip = soundEffects[1];
		audio.Play();
		
		_weaponIndex = _playingView.WeaponSwitcher.CurrentWeapon;
	}
	
	public void ShowPauseMenu()
	{
		_paused = true;
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
		_paused = false;
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
		_searching = true;
	}
	
	public void SwitchToPlayingView()
	{
		_hasStarted = true;
		_searching = false;
	}
	public void SwitchToSearchingView()
	{
		_searching = true;
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		if ((hasStarted == false) && (Network.peerType == NetworkPeerType.Server))
		{
			//If the game becomes full, this makes it so other devices
			//No Longer see this game session as an option
			if (Network.maxConnections == Network.connections.Length)
			{
				//Network.maxConnections = -1;
				networkView.RPC("GameIsFull",RPCMode.All);
			}
		}
		
		//If we ever have the game started, and for some reason the game isn't full,
		//we disconnect and go back to the main Menu
		if ((hasStarted == true) && (Network.peerType == NetworkPeerType.Server))
		{
			if (Network.maxConnections != Network.connections.Length)
			{
				networkView.RPC("MainMenu", RPCMode.Server);
			}
		}
		
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
		
		// Update game energy
		//if(_hasStarted && !_paused) 
		//	_playingView.WeaponBar.Energy -= Time.deltaTime;
		
		// End game stuff
		//if(_playingView.WeaponBar.Energy == 0 && !(_gameEndMenu.State == GameView.GameViewState.Showing) && !(_gameEndMenu.State == GameView.GameViewState.AnimatingIn))
		//{
		//	ShowEndGameMenu(false, 0);
		//}
	}
	
	//This function is called on Server when a Client Disconnects
    void OnPlayerDisconnected(NetworkPlayer player) 
	{
		//if full, we do stuff, if not full, stay and wait for more
		if (hasStarted == true)
		{
			Network.Disconnect();
			Application.LoadLevel("Main Menu");
		}
	}
	
	
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		//This is called on the Client when the Connection to Server is severed. 
		//Usually when the server DC's
		
		Application.LoadLevel("Main Menu");
	}
	
	[RPC]
	public void GameIsFull()
	{
		hasStarted = true;
	}
	
	public void HUDSearchingViewMenuButtonPressed()
	{
		if (hasStarted == true)
		{
			networkView.RPC("MainMenu", RPCMode.Server);
		}
		
		else
		{
			Network.Disconnect();
			Application.LoadLevel("Main Menu");
		}
	}
	
	// IHUDGameViewController methods
	public void HUDGameViewWeaponsSwitched(int newWeapon)
	{
		_weaponIndex = newWeapon;
		
	}
	
	public void FireButtonPressed(object sender)
	{	
		if (hasStarted)
		{
			// Fire vector
			float velocity = 5000;
			GameObject cam = GameObject.Find("ARCamera");
			Vector3 velocityVec = cam.transform.forward * velocity;
		
			networkView.RPC("Shoot",RPCMode.All, cam.transform.position, cam.transform.rotation, velocityVec, _weaponIndex);
		
			// Shooting sounds
			audio.clip = soundEffects[0];
			audio.pitch = Random.Range(0.9F, 1.1F);
			audio.Play();
		
			// UI Changes
			_playingView.WeaponBar.Energy -= 1.0f;
		}
	}
	
	[RPC]
	public void Shoot(Vector3 position, Quaternion rotation, Vector3 velocityVec, int currentWeapon)
	{
		// Choose weapon
		bool primaryWeapon = (currentWeapon == 0);
		Transform weapon;
		if (primaryWeapon)
			weapon = (Transform)Resources.Load(GameState.LoadPrimaryWeapon(), typeof(Transform));
		else
			weapon = (Transform)Resources.Load(GameState.LoadSecondaryWeapon(), typeof(Transform));
		Debug.Log("Shooting: " + weapon.name + ". Primary: " + primaryWeapon);
		
		// Instantiate weapon
		Transform weaponSpawn = (Transform)Instantiate(weapon, position, rotation);
		weaponSpawn.rigidbody.AddForce(velocityVec);
		GameObject imageTarget = GameObject.Find("ImageTarget");	
		weaponSpawn.transform.parent = imageTarget.transform;
	}
	
	public void HUDGameViewMenuButtonPressed()
	{
		if (hasStarted == true)
		{
			networkView.RPC("MainMenu", RPCMode.Server);
		}
		
		else
		{
			Network.Disconnect();
			Application.LoadLevel("Main Menu");
		}
		
	}
	
	[RPC]
	public void ResetLevel()
	{
		if (currentLevel != Application.loadedLevel)
		{
			currentLevel = Application.loadedLevel;
		}
		Application.LoadLevel(currentLevel);
		//Application.LoadLevel("sampleHUDnetworking");
	}
	
	[RPC]
	public void Disconnect()
	{
		Network.Disconnect();
		Application.LoadLevel("Main Menu");
	}
	
	[RPC]
	public void MainMenu()
	{
		networkView.RPC("Disconnect", RPCMode.Others);
		Network.Disconnect(200);
		Application.LoadLevel("Main Menu");
	}
}
