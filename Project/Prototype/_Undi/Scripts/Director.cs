﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Director : MonoBehaviour
{

	#region Variables
	public GameManager managerGame;
	public ManagerCamera managerCamera;
	//public Player player;
	//public ManagerMap managerMap;
	public ManagerEntity managerEntity;
	//public WavesManager waveManager;
	public ManagerInput managerInput;
	public ManagerUI managerUI;
	//public ScoreManager scoreManager;
	public ManagerAudio managerAudio;


	public Structs.GameMode currentGameMode { private set; get; }
	public Structs.GameDifficulty currentGameDifficulty { private set; get; }
	public Structs.GameView currentGameView { private set; get; }
	public Structs.GameScene currentScene;

	private int maxLevelNumber = 2;
	public int currentLevel = 0;

	public bool isPaused;

	#endregion


	#region Singleton
	private static Director instance;

	public static Director Instance
	{
		get { return instance; }
	}

	static Director()
	{
		GameObject obj = GameObject.Find( "Director" );

		if( obj == null )
		{
			obj = new GameObject( "Director", typeof( Director ) );
		}

		instance = obj.GetComponent<Director>();
	}
	#endregion


	#region Monobehaviour
	private void Awake()
	{
		DontDestroyOnLoad( this.gameObject );
	}

	private void LateUpdate()
	{
		// This is only for cameras with SnapToCameraGrab mode
		//if( managerCamera.cameras[0].type == CameraHelper.Type.SnapToCameraGrabs )
		//{
		//	CheckForNewCameraGrab();
		//}
	}
	#endregion


	#region Scene management
	private void ChangeScene( Structs.GameScene to )
	{
		currentScene = to;

		//Debug.Log("Change scene to: " + currentScene);

		switch( currentScene )
		{
			case Structs.GameScene.Initialization:
				SwitchToMenu();

				break;

			case Structs.GameScene.Menu:
				managerUI.SetPanels();
				//managerInput.SetEvents(); // using rewired
				managerCamera.UpdateCameras();

				// This will be called from the update with input
				//GameBegin();

				// Reset game. Only set score to 0 for now
				managerGame.Initialize();

				break;

			case Structs.GameScene.LoadingGame:
				managerUI.SetPanels();
				SwitchToIngame();
				break;

			case Structs.GameScene.Ingame:
				// This loads the map, sets the player and the camera
				// Using the number of the level
				//LoadLevel();

				// Load the 2 players
				bool playerSummoningFailed = managerEntity.SummonPlayers();
				if( !playerSummoningFailed )
				{
					Debug.LogError( "Failed instantiating players " );
					GameEnd();
					return;
				}

				// And subscribe to endgame conditions
				if( managerEntity.playersScript[0] != null )
				{
					PabloTools.Bind( ref managerEntity.playersScript[0].OnDie, GameReset );
				}
				if( managerEntity.playersScript[1] != null )
				{
					PabloTools.Bind( ref managerEntity.playersScript[1].OnDie, GameReset );
				}

				// Set managers
				//managerInput.SetEvents(); // using rewired
				managerUI.SetPanels();
				managerCamera.UpdateCameras();

				// Play start sfx
				managerAudio.PlaySfx( ManagerAudio.Sfx.Start );
				break;

			case Structs.GameScene.GameReset:
				// Update score
				if( managerEntity.playersScript[0].state == EntityPlayer.PlayerState.Alive )
				{
					Director.Instance.managerGame.ScoreIncrease( 0 );
				}
				else if( managerEntity.playersScript[1].state == EntityPlayer.PlayerState.Alive )
				{
					Director.Instance.managerGame.ScoreIncrease( 1 );
				}

				// Unsubscribe from endgame conditions and remove players
				if( managerEntity.playersScript[0] != null )
				{
					PabloTools.Unbind( ref managerEntity.playersScript[0].OnDie );
				}
				if( managerEntity.playersScript[1] != null )
				{
					PabloTools.Unbind( ref managerEntity.playersScript[1].OnDie );
				}
				managerEntity.Reset();


				//managerMap.Reset();
				GameBegin();
				break;

			case Structs.GameScene.GameEnd:
				// Unsubscribe from endgame conditions and remove players
				if( managerEntity.playersScript[0] != null )
				{
					PabloTools.Unbind( ref managerEntity.playersScript[0].OnDie );
				}
				if( managerEntity.playersScript[1] != null )
				{
					PabloTools.Unbind( ref managerEntity.playersScript[1].OnDie );
				}
				managerEntity.Reset();

				//managerMap.Reset();
				//managerInput.SetEvents(); // using rewired
				managerUI.SetPanels();
				SwitchToMenu();
				break;

			case Structs.GameScene.Exit:
				Application.Quit();
				break;
		}

	}
	#endregion


	#region Game settings & management
	public void SetGameSettings( Structs.GameMode gameMode /*, Structs.GameDifficulty gameDifficulty, Structs.GameView viewMode*/ )
	{
		currentGameMode = gameMode;
		//currentGameDifficulty = gameDifficulty;
		//currentGameView = viewMode;
	}

	private void LoadLevel()
	{
		/*
			// Reset entities and map
			managerEntity.Reset();
			managerMap.Reset();

			// Load the current level
			managerMap.LoadMap( currentLevel );
			var newMap = managerMap.mapScript;
			//Debug.Log( "Loading level number: " + (currentLevel + 1).ToString() );

			// Load player init position from map
			// If there is a player init pos in the map data
			Vector2 playerInitPos;
			if( newMap.players.Count > 0 )
			{
				playerInitPos = managerMap.mapScript.players[0];
			}
			else
			{
				playerInitPos = Vector2.zero;
			}
			managerEntity.SummonPlayer( 0, playerInitPos );

			// And finally, set camera
			Vector2 cameraInitPos = Vector2.zero;

			switch( newMap.cameraType )
			{
				case CameraHelper.Type.FixedPoint:
					if( newMap.cameraGrabs.Count > 0 )
					{
						// If there is camera data, init camera to first cameragrab
						cameraInitPos = managerMap.mapScript.cameraGrabs[0];
					}
					managerCamera.cameras[0].SetFixedPoint( cameraInitPos );
					break;

				case CameraHelper.Type.FixedAxis:
					break;

				case CameraHelper.Type.SnapToCameraGrabs:
					//Debug.Log( "CameraGrabs on new map: " + newMap.cameraGrabs.Count );
					if( newMap.cameraGrabs.Count > 0 )
					{
						managerCamera.cameras[0].SetSnapToCameraGrab();
					}
					else
					{
						// If there are no camera grabs, just fall back to default follow player
						managerCamera.cameras[0].SetFollow( managerEntity.playersScript[0].transform );
					}
					break;

				default: // By default, just follow the first player
				case CameraHelper.Type.Follow:
					managerCamera.cameras[0].SetFollow( managerEntity.playersScript[0].transform );
					break;
			}
		*/
	}

	private void LoadNumberLevel( int levelNumber )
	{
		currentLevel = levelNumber;
	}

	private void LoadNextLevel()
	{
		currentLevel++;
		if( currentLevel >= maxLevelNumber )
		{
			currentLevel = maxLevelNumber - 1;
		}
	}

	private void LoadPreviousLevel()
	{
		currentLevel--;
		if( currentLevel < maxLevelNumber )
		{
			currentLevel = 0;
		}
	}
	#endregion


	#region Game cycle
	// This is the first thing that begins the whole game
	public void EverythingBeginsHere()
	{
		ChangeScene( Structs.GameScene.Initialization );
	}

	// This is automatic
	private void SwitchToMenu()
	{
		ChangeScene( Structs.GameScene.Menu );
	}

	public void GameBegin()
	{
		ChangeScene( Structs.GameScene.LoadingGame );
	}

	// This is automatic
	private void SwitchToIngame()
	{
		ChangeScene( Structs.GameScene.Ingame );
	}

	private void GameReset()
	{
		ChangeScene( Structs.GameScene.GameReset );
	}

	public void GameEnd()
	{
		ChangeScene( Structs.GameScene.GameEnd );
	}

	public void Exit()
	{
		Debug.Log( "Exit game!" );
		ChangeScene( Structs.GameScene.Exit );
	}
	#endregion



	#region DEBUG
	public void DebugLoadLevel( int numb )
	{
		LoadNumberLevel( numb );
		GameReset();
	}

	public void DebugLevelNext()
	{
		LoadNextLevel();
		GameReset();
	}

	public void DebugLevelPrevious()
	{
		LoadPreviousLevel();
		GameReset();
	}

	private void CheckForNewCameraGrab()
	{
		/*
		// I'm calling this from Director.Update

		Vector2 player = managerEntity.playersScript[0].transform.position;
		List<Vector2> cameraGrabs = managerMap.mapScript.cameraGrabs;
		float min = 999;
		Vector2 correctCameraGrab = Vector2.zero;

		// Constantly checking for distance between player and ALL camera grabs?
		// Extremely inefficient
		for( int i = 0; i < cameraGrabs.Count; i++ )
		{
			float checking = Vector2.Distance( player, cameraGrabs[i] );
			if( checking < min )
			{
				min = checking;
				correctCameraGrab = cameraGrabs[i];
			}
		}

		// When the result of the check is different, then call CameraHelper.OnNewCameraGrab(x,y)
		managerCamera.cameras[0].OnNewCameraGrab( correctCameraGrab.x, correctCameraGrab.y );
		*/
	}
	#endregion
}
