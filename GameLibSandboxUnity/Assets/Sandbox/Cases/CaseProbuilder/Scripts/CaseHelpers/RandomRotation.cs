using UnityEngine;

public class RandomRotation : MonoBehaviour
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

        Vector3 rotation = new Vector3(
            (Mathf.PerlinNoise(timex, timex) - 0.5f) * Travel.x,
            (Mathf.PerlinNoise(timey + 1f, timey + 1f) - 0.5f) * Travel.y,
            (Mathf.PerlinNoise(timez + 2f, timez + 2f) - 0.5f) * Travel.z
        );

        transform.rotation = Quaternion.Euler(rotation);
    }
}
