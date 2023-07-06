using GameLib.Random;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public abstract class RandomShape : MonoBehaviour
{
    public long Seed;
    public bool SpawnOnAwake;
    public Vector3 PivotPercent;
    protected IPseudoRandomNumberGenerator _rnd;
    private ProBuilderMesh _mesh;
    private Bounds _bounds;


    void Awake()
    {
        _rnd = RandomHelper.CreateRandomNumberGenerator(Seed);
        Seed = _rnd.GetState().AsNumber();
        if(SpawnOnAwake)
            SpawnShape();
    }

    public virtual void SpawnShape()
    {
        _mesh = Instantiate();

        // Set bounds and material
        {
            MeshRenderer mr = _mesh.gameObject.GetComponent<MeshRenderer>();
            mr.material = new Material(Shader.Find("Diffuse"));
            _bounds = mr.bounds;
        }
        _mesh.transform.SetParent(transform);
        _mesh.transform.position = transform.position;
        _mesh.SetPivot(transform.position + Vector3.Scale(_bounds.extents, PivotPercent));
        _mesh.transform.position = transform.position;
    }

    protected abstract ProBuilderMesh Instantiate();
}
