using GameLib.Random;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class ExtrudesAndScales : MonoBehaviour
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
        int index = 0;
        foreach (var face in mesh.faces)
        {
            var faceNormal = transform.TransformVector(Math.Normal(mesh, face));
            var dot = Vector3.Dot(faceNormal.normalized, Vector3.up);
            if (Mathf.Approximately(dot, 1f))
            {
                //mesh.ExtrudeFaces(ExtrudeMethod.IndividualFaces, _rnd.FromArray(RandomHeights), face);
                var center = mesh.AveragePositionOfFace(face);
                //var center = mesh.AveragePositionOfFace(face);

                //mesh.MoveFaces(Vector3.up*Random.value*3, index);
                mesh.ScaleFaces(new Vector3(81.8f,3.5f, 19.4f), 0.5f, index);
                break;
            }

            index++;
        }
    }
}
