using UnityEngine;

namespace AgarioClone
{
	public static class QuadCreator
	{
		public static Mesh CreateQuad(float size)
		{
			Mesh mesh = new Mesh();

			float f = size * 0.5f;
			Vector3[] vertices = new Vector3[4]
			{
				new(-f, -f, 0),
				new(f, -f, 0),
				new(-f, f, 0),
				new(f, f, 0)
			};
			mesh.vertices = vertices;

			int[] tris = new int[6]
			{
				0, 2, 1,
				2, 3, 1
			};
			mesh.triangles = tris;

			Vector3[] normals = new Vector3[4]
			{
				Vector3.forward,
				Vector3.forward,
				Vector3.forward,
				Vector3.forward
			};
			mesh.normals = normals;

			Vector2[] uv = new Vector2[4]
			{
				new(0, 0),
				new(1, 0),
				new(0, 1),
				new(1, 1)
			};
			mesh.uv = uv;

			return mesh;
		}
	}
}