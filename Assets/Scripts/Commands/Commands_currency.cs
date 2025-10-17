using UnityEngine;

namespace Commands
{
    public partial class CommandManager : MonoBehaviour
    {
        [Command("spawnBox", "Spawns given amounts of box")]
        public void SpawnBox(int amount)
        {
            SpawnObjectsScript.Instance.SpawnMany(amount);
        }
    }
}
