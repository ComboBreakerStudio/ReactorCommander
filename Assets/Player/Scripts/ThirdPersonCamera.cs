﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ThirdPersonCamera : NetworkBehaviour {

	[SerializeField]
	private GameObject torsoObject;
	[SerializeField]
	private GameObject cam;
	[SerializeField]
	private bool lockCursor;
	[SerializeField]
	private float mouseSensitivity = 5;
	[SerializeField]
	private Vector2 yawMinMax = new Vector2 (-45, 45);
	[SerializeField]
	private Vector2 pitchMinMax = new Vector2 (-30, 30);
	[SerializeField]
	private float rotationSmoothTime = .12f;

	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	//Body Control
	public GameObject leftWeaponPivot, rightWeaponPivot, targetPosition;
	//Melee
	public bool isMelee;


	float yaw;
	float pitch;

	void Start() {
		//Find Torso Object
		Transform[] _childObject = GetComponentsInChildren<Transform>();
		for(int i = 0; i < _childObject.Length; i++){
			if(_childObject[i].name == "TorsoPivot"){
				torsoObject = _childObject [i].gameObject;
			}
		}

		if(!isLocalPlayer){
			return;
		}

		if (lockCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

	}

	void LateUpdate () {
		if(!isLocalPlayer){
			return;
		}

		if (cam != null)
		{
			yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
			yaw = Mathf.Clamp (yaw, yawMinMax.x, yawMinMax.y);
			pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
			pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

			currentRotation = Vector3.SmoothDamp (currentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

			cam.transform.localEulerAngles = currentRotation;
//			if(!isMelee){

				torsoObject.transform.localEulerAngles = new Vector3 (0, cam.transform.localEulerAngles.y, 0 );
//
//				leftWeaponPivot.transform.localEulerAngles = new Vector3 (cam.transform.eulerAngles.x, cam.transform.localEulerAngles.y, cam.transform.eulerAngles.z );
//				rightWeaponPivot.transform.localEulerAngles = new Vector3 (cam.transform.eulerAngles.x, cam.transform.localEulerAngles.y, cam.transform.eulerAngles.z );
//			}


//			leftWeaponPivot.transform.localEulerAngles = currentRotation;
//			rightWeaponPivot.transform.localEulerAngles = currentRotation;
			leftWeaponPivot.transform.LookAt(targetPosition.transform);
			rightWeaponPivot.transform.LookAt(targetPosition.transform);
		}
	}

}
