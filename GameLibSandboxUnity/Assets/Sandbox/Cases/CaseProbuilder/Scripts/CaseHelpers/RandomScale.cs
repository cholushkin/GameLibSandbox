using UnityEngine;

public class RandomScale: MonoBehaviour
{
    public Vector3 Speed;
    public Vector3 Travel;
    public float Seed;

    void Awake()
    {
        if (Seed < 0f)
            Seed = Random.Range(0f, 10000f);
    }

    void Update()
    {
        float seededTime = Time.time + Seed;
        float timex = seededTime * Speed.x;
        float timey = seededTime * Speed.y;
        float timez = seededTime * Speed.z;

        Vector3 scale = new Vector3(
            Speed.x == 0f ? 1f : (Mathf.PerlinNoise(timex, timex)) * Travel.x,
            Speed.y == 0f ? 1f : (Mathf.PerlinNoise(timey + 1f, timey + 1f)) * Travel.y,
            Speed.z == 0f ? 1f : (Mathf.PerlinNoise(timez + 2f, timez + 2f)) * Travel.z
        );

        transform.localScale = scale;
    }
}
