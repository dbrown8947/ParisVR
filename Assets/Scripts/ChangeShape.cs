using UnityEngine;
using System.Collections;

public class ChangeShape : MonoBehaviour {

	public int scale = 1;
	public Material material;
	private Mesh tileMesh;
	public Vector3[] vertices = new Vector3[4];

	// Use this for initialization
	//public void Start () {
	public void UpdateMesh (Vector3[] Vertices) {
		MeshFilter mf  = GetComponent<MeshFilter>();
		var mesh = new Mesh();
		mf.mesh = mesh;

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

		//mesh.normals = normals;

		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(vertices[0].x, vertices[0].y);
		uv[1] = new Vector2(vertices[1].x, vertices[1].y);
		uv[2] = new Vector2(vertices[2].x, vertices[2].y);
		uv[3] = new Vector2(vertices[3].x, vertices[3].y);



		mesh.uv = uv;

	}

}


