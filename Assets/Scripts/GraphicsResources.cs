using UnityEngine;
using UnityEngine.PlayerLoop;

public class GraphicsResources : MonoBehaviour
{
    [SerializeField] private Material blinker_mat;
    [SerializeField] private AnimationCurve blinker_fluid_animation;
    [SerializeField] private GameObject death_particles;

    public static Material GetBlinkerFluid => InitResources.GetGraphicsResources.blinker_mat;
    public static AnimationCurve GetBlinkerFluidAnimation => InitResources.GetGraphicsResources.blinker_fluid_animation;
    public static GameObject GetDeathParticles => InitResources.GetGraphicsResources.death_particles;

}
