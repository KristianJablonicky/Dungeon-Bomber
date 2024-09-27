using UnityEngine;
public enum MetronomeBeatStates
{
    beforeUserInput,
    userInputStart,
    userInputEnd
}

public enum Movement
{
    Right,
    Down,
    Left,
    Up
}

public enum CharacterType
{
    Player,
    NPC
}

public static class Directions
{
    public static Vector3[] vectors = new Vector3[4]
    {
        new Vector3(1, 0, 0),
        new Vector3(0, -1, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0)
    };
}