using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWater : MonoBehaviour {


	public int CurveResolution = 10;
	public Vector3[] CurveCoordinates;
	public Vector3[] Tangents;
	public bool ClosedLoop = false;
	public Vector3[] TangentExtraPoints;
	private int extraPointCount = 0;
	public Vector3[] vertices = new Vector3[4];
	public float Muliplier = 0.5f;


	public Vector3[] GetTangentPoints(List<Vector3> Points)
	{
		int CurveResolution = 10;
		Vector3[] CurveCoordinates;
		Vector3[] Tangents;
		bool ClosedLoop = false;
		Vector3[] TangentExtraPoints;
		int extraPointCount = 0;
		Vector3 p0;
		Vector3 p1;
		Vector3 m0;
		Vector3 m1;
		int pointsToMake;

		if (ClosedLoop == true)
		{
			pointsToMake = (CurveResolution) * (Points.Count);
		}
		else
		{
			pointsToMake = (CurveResolution) * (Points.Count - 1);
		}

		CurveCoordinates = new Vector3[pointsToMake];
		Tangents = new Vector3[pointsToMake];

		TangentExtraPoints = new Vector3[pointsToMake * 2];

		int closedAdjustment = ClosedLoop ? 0 : 1;

		// First for loop goes through each individual control point and connects it to the next, so 0-1, 1-2, 2-3 and so on
		for (int i = 0; i < Points.Count - closedAdjustment; i++)
		{
			//if (Points[i] == null || Points[i + 1] == null || (i > 0 && Points[i - 1] == null) || (i < Points.Count - 2 && Points[i + 2] == null))
			//{
			//    return;
			//}

			p0 = Points[i];
			p1 = (ClosedLoop == true && i == Points.Count - 1) ? Points[0] : Points[i + 1];

			// Tangent calculation for each control point
			// Tangent M[k] = (P[k+1] - P[k-1]) / 2
			// With [] indicating subscript

			// m0
			if (i == 0)
			{
				m0 = ClosedLoop ? 0.5f * (p1 - Points[Points.Count - 1]) : p1 - p0;
			}
			else
			{
				m0 = 0.5f * (p1 - Points[i - 1]);
			}

			// m1
			if (ClosedLoop)
			{
				if (i == Points.Count - 1)
				{
					m1 = 0.5f * (Points[(i + 2) % Points.Count] - p0);
				}
				else if (i == 0)
				{
					m1 = 0.5f * (Points[i + 2] - p0);
				}
				else
				{
					m1 = 0.5f * (Points[(i + 2) % Points.Count] - p0);
				}
			}
			else
			{
				if (i < Points.Count - 2)
				{
					m1 = 0.5f * (Points[(i + 2) % Points.Count] - p0);
				}
				else
				{
					m1 = p1 - p0;
				}
			}

			Vector3 position;
			float t;
			float pointStep = 1.0f / CurveResolution;

			if ((i == Points.Count - 2 && ClosedLoop == false) || (i == Points.Count - 1 && ClosedLoop))
			{
				pointStep = 1.0f / (CurveResolution - 1);
				// last point of last segment should reach p1
			}
			// Second for loop actually creates the spline for this particular segment
			for (int j = 0; j < CurveResolution; j++)
			{
				t = j * pointStep;
				Vector3 tangent;
				position = CatmullSpline.Interpolate(p0, p1, m0, m1, t, out tangent);

				TangentExtraPoints [extraPointCount] = (position + (Vector3.Cross(tangent, Vector3.up).normalized)/0.1f);
				extraPointCount++;
				TangentExtraPoints [extraPointCount] = (position - (Vector3.Cross(tangent, Vector3.up).normalized)/0.1f);
				extraPointCount++;
				CurveCoordinates[i * CurveResolution + j] = position;
				Tangents[i * CurveResolution + j] = tangent;
			}
		}

		return TangentExtraPoints;
	}

	public Mesh UpdateMesh (Vector3[] Vertices) {
		var mesh = new Mesh();
		vertices = new Vector3[4];
		//Order of verticies is bottomleft,bottomright,topleft,topright
		vertices[0] = Vertices[0];
		vertices[1] = Vertices[1];
		vertices[2] = Vertices[2];
		vertices[3] = Vertices[3];

		mesh.vertices = vertices;

		int[]tri = new int[6];

		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;

		mesh.triangles = tri;

		Vector3[] normals = new Vector3[4];

		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;

		mesh.normals = normals;

		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1,0);
		uv[2] = new Vector2(0,1);
		uv[3] = new Vector2(1,1);

		mesh.uv = uv;

		return mesh;
	}

}