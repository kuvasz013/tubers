using UnityEngine.InputSystem;

public struct PlayerConfig {
    public int playerID;
    public string controlScheme;
    public InputDevice inputDevice;
    public TuberType tuberType;
    public PlayerInput playerInput;
}

public enum TuberType
{
    Potato,
    Carrot,
    Beet,
    Scallion
}