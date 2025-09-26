using UnityEngine;

public class NullableObjects : MonoBehaviour
{
    [HideInInspector] public Camera cam { private set; get; }
    [HideInInspector] public Player player { private set; get; }


    public void AssignPlayer(Player p) => player = p;
    public void AssignCamera(Camera c) => cam = c;
}
