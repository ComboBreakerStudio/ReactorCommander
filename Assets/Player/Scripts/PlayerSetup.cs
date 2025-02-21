﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	public GameObject[] gameobjectsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	Camera sceneCamera;
	[SyncVar]
	public string idName;

	void Start()
	{
		
		// Disable components that should be active on the player that we control
		if (!isLocalPlayer) 
		{
			AssignRemoteLayer ();
			DisableComponents ();
		} 
		else if(isLocalPlayer)
		{
			//Set local player to Game Manager
			GameManager.GM.localPlayer = this.gameObject;

			// We are the local player: Disable the scene camera
			sceneCamera = Camera.main;
			if (sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive (false);
			}
		}

		RegisterPlayer ();
	}

	void RegisterPlayer()
	{
		string _ID = "Player" + GetComponent<NetworkIdentity> ().netId;
		transform.name = _ID;
		idName = _ID;
		CmdChangeName (idName);
	}

	void ChangeName(string ID){
		transform.name = ID;
	}

	void AssignRemoteLayer()
	{
		Transform[] go = GetComponentsInChildren<Transform> ();
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
		for(int i = 0; i < go.Length; i++){
			if(go[i].gameObject.layer != LayerMask.NameToLayer("Minimap")){
				go [i].gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
			}

		}
	}

	void DisableComponents()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable [i].enabled = false;
		}

		for (int i = 0; i < gameobjectsToDisable.Length; i++)
		{
			gameobjectsToDisable [i].SetActive (false);
		}
	}

	// When we are destroyed
	void OnDisable()
	{
		// We enable the scene camera
		if (sceneCamera != null) 
		{
			sceneCamera.gameObject.SetActive (true);
		}
	}

	[Command]
	public void CmdChangeName(string name){
		idName = name;
	}

	void Update(){
		if(transform.name != idName){
			transform.name = idName;
		}
//		if(transform.name == name){
//			this.enabled = false;
//		}
	}
		/*
	[SyncVar] // Only sync Float int string etc normal variable - not GameObject
	public int health;//Testing

	[SerializeField]
	private GameObject Bullet;

	// Use this for initialization
	void Start () 
	{
		if(!isLocalPlayer)
		{
			
			return;
		}
		//		playerTransform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!isLocalPlayer){
			return;
		}
	}

	[Command]// Only works in Server
	void CmdDebugLog(string log){
		Debug.Log (log);
	}

	//Left Weapon
	void LeftWeapon_Attack(){ 
		//Do a local Raycast to a position

		RpcSpawnBullet (Bullet);
	}

	//Right Weapon
	void RightWeapon_Attack (){
		//Do a local Raycast to a position || enable a collider || do something || Rpc to spawn Bullet
	}

	[Client] // means Calling functions to other client
	void RpcSpawnBullet(GameObject _bulletObject){//Use Rpc Only to spawn Particles/bullets (not for collision detection)
		Debug.Log ("Shoot");
	
		//Below are references

		GameObject go = Instantiate (_bulletObject);
		go.transform.position = transform.position;
		go.transform.rotation = transform.rotation;

		NetworkServer.Spawn (go);//Only can spawn an instantiated game object //Spawn go to other client as a network object
	*/
}
