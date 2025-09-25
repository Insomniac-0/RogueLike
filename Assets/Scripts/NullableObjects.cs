using UnityEngine;

public class NullableObjects : MonoBehaviour
{
    public Camera cam;
    public Player player;


    public void AssignPlayer(Player p) => player = p;
    public void AssignCamera(Camera c) => cam = c;
}
