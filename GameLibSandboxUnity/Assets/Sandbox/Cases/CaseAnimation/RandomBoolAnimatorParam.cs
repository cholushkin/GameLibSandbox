using UnityEngine;

public class RandomBoolAnimatorParam : MonoBehaviour
{
    public Animator Animator;
    public string Parameter;
    
    void Awake()
    {
        Animator.SetBool(Parameter, Random.value < 0.5f);
    }
}
