// GameState
// Created by Nathan Swenson

using UnityEngine;
using System.Collections;

// GameState is a static class in charge of saving the portion of the game's state that
// should be persistent between application launches, including the top score on each
// level and the player's current weapon selections. 
public class GameState : MonoBehaviour 
{
	static string _currentLevel;
	static bool _playingMultipler;
	
	public static void SaveScore(string level, int score)
	{
		PlayerPrefs.SetInt("score" + level, score);
		PlayerPrefs.Save();
	}
	
	public static void SetCurrentLevel(string level)
	{
		_currentLevel = level;
		Debug.Log("Saving level: " + _currentLevel);
	}
	
	public static string GetCurrentLevel()
	{
		return _currentLevel;
		Debug.Log("Entering level: " + _currentLevel);
	}
	
	public static int LoadScore(string level)
	{
		return PlayerPrefs.GetInt("score" + level);
	}
	
	public static void SavePrimaryWeapon(string weaponName)
	{
		
		PlayerPrefs.SetString("primaryWeapon", weaponName);
		PlayerPrefs.Save();
		Debug.Log("Saving primary weapon: " + weaponName);
	}
	
	public static string LoadPrimaryWeapon()
	{
		string weaponName = PlayerPrefs.GetString("primaryWeapon");
		Debug.Log("Loading primary weapon: " + weaponName);
		return weaponName;
	}
	
	public static void SaveSecondaryWeapon(string weaponName)
	{
		PlayerPrefs.SetString("secondaryWeapon", weaponName);
		PlayerPrefs.Save();
		Debug.Log("Saving secondary weapon: " + weaponName);
	}
	
	public static string LoadSecondaryWeapon()
	{
		string weaponName = PlayerPrefs.GetString("secondaryWeapon");
		Debug.Log("Loading primary weapon: " + weaponName);
		return weaponName;
	}

	public static bool GetPlayingMultiplayer()
	{
		return _playingMultipler;
	}
	
	public static void SetPlayingMultiplayer(bool multiplayerBoolean)
	{
		_playingMultipler = multiplayerBoolean;
	}
}
