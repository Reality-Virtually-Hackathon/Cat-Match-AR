//-----------------------------------------------------------------------
// <copyright file="PlaneVisualizer.cs" company="Google, Wayfair">
//
// Copyright 2017 Google Inc. and Wayfair Inc. All Rights Reserved.
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
using GoogleARCoreInternal;
using GoogleARCore;

// Visualizes a TrackedPlane in the Unity scene.
public class PlaneVisualizer : MonoBehaviour
{
    // The ARCore tracked plane to represent.
    private TrackedPlane plane;

	// The mesh for the plane
    private Mesh planeMesh;

	// Renderer for the plane mesh
    private MeshRenderer planeMeshRenderer;

    // The Unity Awake() method.
    private void Awake()
    {
		planeMesh = GetComponent<MeshFilter>().mesh;
		planeMeshRenderer = GetComponent<UnityEngine.MeshRenderer>();
    }
		
    // The Unity Update() method.
    private void Update()
    {
		if (plane == null)
        {
            return;
        }
		else if (plane.SubsumedBy != null)
        {
            Destroy(gameObject);
            return;
        }
		else if (!plane.IsValid || Frame.TrackingState != FrameTrackingState.Tracking)
        {
			planeMeshRenderer.enabled = false;
            return;
        }

		planeMeshRenderer.enabled = true;
		if (plane.IsUpdated)
        {
			UpdateMesh();
        }
    }

    // Update the Plane reference.
    public void SetPlane(TrackedPlane trackedPlane)
    {
		plane = trackedPlane;
		UpdateMesh();
    }

	private void UpdateMesh()
    {
		// Get the polygon vertices for a plane
		List<Vector3> meshVertices = new List<Vector3>();
		plane.GetBoundaryPolygon (ref meshVertices);
		int planeVertexCount = meshVertices.Count;

		// Get the center of the plane and include it as starting vertex
		Vector3 center = plane.Position;
		meshVertices.Insert (0, center);


		// Generate triangles for each polygon vertext
		int[] triangles = new int[planeVertexCount * 3];
		for (int i = 0; i < planeVertexCount-1; i++) 
		{
			triangles[i * 3] = i+2;
			triangles[i * 3 + 1] = 0;
			triangles[i * 3 + 2] = i + 1;
		}

		triangles[(planeVertexCount-1) * 3] = 1;
		triangles[(planeVertexCount-1) * 3 + 1] = 0;
		triangles[(planeVertexCount-1) * 3 + 2] = planeVertexCount;


		// Update the mesh with vertices and triangles
		planeMesh.Clear();
		planeMesh.SetVertices(meshVertices);
		planeMesh.triangles = triangles;
    }

}
