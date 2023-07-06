using GameLib.Random;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class RandomExtrudesOnTop : MonoBehaviour
{
    public float[] RandomHeights;
    private IPseudoRandomNumberGenerator _rnd = RandomHelper.CreateRandomNumberGenerator();

    void Awake()
    {
        RandomExtrudes();
    }

    public void RandomExtrudes()
    {
        var importer = new MeshImporter(gameObject);
        importer.Import();
        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = new Mesh();

        var mesh = gameObject.GetComponent<ProBuilderMesh>();
        foreach (var face in mesh.faces)
        {
            PrintFaceInfo(face);
            var faceNormal = transform.TransformVector(Math.Normal(mesh, face));
            var dot = Vector3.Dot(faceNormal.normalized, Vector3.up);
            if (Mathf.Approximately(dot, 1f))
                mesh.ExtrudeFaces(ExtrudeMethod.IndividualFaces, _rnd.FromArray(RandomHeights), face);
        }
    }

    private void PrintFaceInfo(Face face)
    {
    }
}
