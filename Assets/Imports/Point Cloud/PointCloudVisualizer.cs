// <copyright file="PointCloudVisualizer.cs" company="Google, Wayfair">
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using GoogleARCore;

// Visualize the point cloud. (Feature points)
public class PointCloudVisualizer : MonoBehaviour
{
	private Mesh mesh;
	private double lastTimeStamp;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
	}

	// Update is called once per frame
	void Update () {
		// Skip if ARCore is not tracking
		if (Frame.TrackingState != FrameTrackingState.Tracking) {
			return;
		}

		// Get the Point cloud from the Frame
		PointCloud pointCloud = Frame.PointCloud;

		// Make Vertices / Indices array of Point Cloud size
		int pCount = pointCloud.PointCount;
		Vector3 [] vertices  = new Vector3[pCount];
		int [] indices  = new int[pCount];

		// If the Point Cloud was not empty and the observed timestamp was new
		if (pCount > 0 && pointCloud.Timestamp > lastTimeStamp) {
			for (int i = 0; i < pCount; i++) {
				//Set the vertices as each Point Cloud point
				vertices [i] = pointCloud.GetPoint (i);

				// Update the mesh indices array
				indices [i] = i;
			}

			// Update the mesh with vertices and indices
			mesh.Clear ();
			mesh.vertices = vertices;
			mesh.SetIndices (indices, MeshTopology.Points, 0);
			lastTimeStamp = pointCloud.Timestamp;
		}
	}
}
