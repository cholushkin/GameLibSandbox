// Demonstrates how to convert a UnityEngine.Mesh object to an editable ProBuilderMesh.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace ProBuilder.Examples
{
	[RequireComponent(typeof(MeshFilter))]
	public class MakePrimitiveEditable : MonoBehaviour
	{
		void Awake()
		{
			var importer = new MeshImporter(gameObject);
			importer.Import();
			var mesh = gameObject.GetComponent<ProBuilderMesh>();
			Debug.Log($"{gameObject.name} has {mesh.faces.Count} faces");
			Debug.Log($"{gameObject.name} has {mesh.normals.Count} normals");
			
			mesh.ToMesh();
			mesh.Refresh();
		}

		void Start()
		{
			// Import from a GameObject. In this case we're loading and assigning to the same GameObject, but you may
			// load and apply to different Objects as well.

			// Create a new MeshImporter
			var importer = new MeshImporter(gameObject);
			importer.Import();

			// Since we're loading and setting from the same object, it is necessary to create a new mesh to avoid
			// overwriting the mesh that is being read from.
			var filter = GetComponent<MeshFilter>();
			filter.sharedMesh = new Mesh();

			//Retrieve the create PB Mesh
			var mesh = gameObject.GetComponent<ProBuilderMesh>();

			// Do something with the pb_Object. Here we're extruding every face on the object by .25.
			//mesh.Extrude(mesh.faces, ExtrudeMethod.IndividualFaces, .25f);


			Face[] faces = new Face[1];
			foreach (var face in mesh.faces)
			{
				print( Vector3.Dot(Math.Normal(mesh, face), Vector3.up));
				var faceNormal = Math.Normal(mesh, face);
				faceNormal = transform.TransformVector(faceNormal);
				var dot = Vector3.Dot(faceNormal, Vector3.up);
				if (Mathf.Approximately(dot, 1f))
				{
					faces[0] = face;
					mesh.Extrude(faces, ExtrudeMethod.IndividualFaces, Random.value);
				}
			}

			

			// Apply the imported geometry to the pb_Object
			mesh.ToMesh();

			// Rebuild UVs, Collisions, Tangents, etc.
			mesh.Refresh();
		}
	}
}
