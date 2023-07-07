using UnityEngine;

public class RandomColorChangeGroup : MonoBehaviour
{
    public Material[] Materials;
    private MeshRenderer[] _meshRenderers;
    
    void Start()
    {
        // Get all mesh renderers in children
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in _meshRenderers)
            meshRenderer.material = Materials[(int) (Materials.Length * Random.value)];
        
        InvokeRepeating("RandomChangeColor", 0.2f, 0.2f);
    }

    void RandomChangeColor()
    {
        foreach (var meshRenderer in _meshRenderers)
            meshRenderer.material = Materials[(int) (Materials.Length * Random.value)];
    }
}
