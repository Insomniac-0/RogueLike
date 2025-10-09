using UnityEngine;
using UnityEngine.UI;

public class NullableObjects : MonoBehaviour
{
    [HideInInspector] public Camera cam { private set; get; }
    [HideInInspector] public Player player { private set; get; }
    [HideInInspector] public Spawner spawner { private set; get; }
    [HideInInspector] public UpgradeUI upgrade_ui { private set; get; }
    [HideInInspector] public PauseUI pause_ui { private set; get; }
    [HideInInspector] public GameOverUI game_over_ui { private set; get; }
    [HideInInspector] public Slider volume_slider { private set; get; }
    [HideInInspector] public Toggle vsync_toggle { private set; get; }



    public static Camera GetCamera => InitResources.GetNullableObjects.cam;
    public static Player GetPlayer => InitResources.GetNullableObjects.player;
    public static Spawner GetSpawner => InitResources.GetNullableObjects.spawner;
    public static UpgradeUI GetUpgradeUI => InitResources.GetNullableObjects.upgrade_ui;
    public static PauseUI GetPauseUI => InitResources.GetNullableObjects.pause_ui;
    public static GameOverUI GetGameOverUI => InitResources.GetNullableObjects.game_over_ui;
    public static Slider GetVolumeSlider => InitResources.GetNullableObjects.volume_slider;
    public static Toggle GetVsyncToggle => InitResources.GetNullableObjects.vsync_toggle;


    public void AssignPlayer(Player p) => player = p;
    public void AssignCamera(Camera c) => cam = c;
    public void AssignSpawner(Spawner s) => spawner = s;
    public void AssignUpgradeUI(UpgradeUI ui) => upgrade_ui = ui;
    public void AssignPauseUI(PauseUI ui) => pause_ui = ui;
    public void AssignGameOverUI(GameOverUI ui) => game_over_ui = ui;
    public void AssignVolumeSlider(Slider s) => volume_slider = s;
    public void AssignVsyncToggle(Toggle t) => vsync_toggle = t;

}
