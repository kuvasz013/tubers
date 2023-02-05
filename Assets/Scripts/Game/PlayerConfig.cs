using UnityEngine.InputSystem;

public class PlayerConfig {
    public int playerID;
    public string controlScheme;
    public InputDevice inputDevice;
    public TuberType tuberType;
    public PlayerInput playerInput;
    public bool isDead;
}

public enum TuberType
{
    Potato,
    Carrot,
    Beet,
    Scallion,
    NONE
}