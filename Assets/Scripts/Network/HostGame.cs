﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	[SerializeField]
	private uint roomSize; //max players

	private string roomName;

	private NetworkManager networkManager;

	void Start(){

		networkManager = NetworkManager.singleton;

		if(networkManager.matchMaker == null){
			networkManager.StartMatchMaker ();
		}
	}

	public void SetRoomName(string _name){
		roomName = _name;
	}

	public void CreateRoom(){
		if(roomName != "" && roomName != null){
			Debug.Log ("Create Room : " + roomName + " \nRoom Size : " + roomSize + " player(s)");
			//Create Room
			networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		}
	}
}
