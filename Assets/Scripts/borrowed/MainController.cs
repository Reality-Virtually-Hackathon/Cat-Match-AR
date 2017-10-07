//-----------------------------------------------------------------------
// <copyright file="MainController.cs" company="Google, Wayfair">
//
// Copyright 2017 Google Inc. and Wayfair LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class MainController : MonoBehaviour {

	// A prefab for tracking and visualizing detected planes.
	public GameObject plane;

	// The first-person camera being used to render the passthrough camera.
	public Camera firstPersonCamera;

	// A model to place when a raycast from a user touch hits a plane.
	public GameObject catobj;

	// Update is called once per frame
	void Update () {

		// Skip the update if ARCore is not tracking
		if (Frame.TrackingState != FrameTrackingState.Tracking)
		{
			return;
		}

		// Get all new planes
		List<TrackedPlane> newPlanes = new List<TrackedPlane>();
		Frame.GetNewPlanes(ref newPlanes);

		// Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
		for (int i = 0; i < newPlanes.Count; i++)
		{
			// Instantiate a plane visualization prefab and set it to track the new plane.
			GameObject planeObject = Instantiate(plane, Vector3.zero, Quaternion.identity,
				transform);
			planeObject.GetComponent<PlaneVisualizer>().SetPlane(newPlanes[i]);
		}

		// See if user made an touch event
		Touch touch;
		if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
		{
			return;
		}

		TrackableHit hitResult;
		// Create a raycast filter
		TrackableHitFlag raycastFilter = TrackableHitFlag.PlaneWithinBounds | TrackableHitFlag.PlaneWithinPolygon;

		// Create a Raycast with touch position, filters and hitResult object
		if (Session.Raycast(firstPersonCamera.ScreenPointToRay(touch.position), raycastFilter, out hitResult))
		{
			// Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
			// world evolves.
			var anchor = Session.CreateAnchor(hitResult.Point, Quaternion.identity);

			// Intanstiate a Deadpool object as a child of the anchor;
			// It's transform will now benefit from the anchor's tracking.
			var newcat = Instantiate(catobj, hitResult.Point, Quaternion.identity,
				anchor.transform);

			// Make the model look at the camera
			newcat.transform.LookAt(firstPersonCamera.transform);
			newcat.transform.rotation = Quaternion.Euler(0.0f,
				newcat.transform.rotation.eulerAngles.y, newcat.transform.rotation.z);
		}
	}
}
