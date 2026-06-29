using UnityEngine;

namespace CuteAliens.Data
{
    [CreateAssetMenu(
        fileName = "NewGameSetupSettings",
        menuName = "Cute Aliens/Game Setup Settings"
    )]
    public class GameSetupSettings : ScriptableObject
    {
        [Min(2)] public int playerCount = 2;
        [Min(1)] public int startingHandSize = 5;
        [Min(1)] public int roundCount = 3;
    }
}