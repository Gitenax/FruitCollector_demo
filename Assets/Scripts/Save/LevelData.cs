using System.Collections.Generic;
using TestProject.Characters;
using TestProject.Characters.Enemies;
using TestProject.Resources;

namespace TestProject.Save
{
    public sealed class LevelData
    {
        public List<Enemy> Enemies;
        public List<Item> Items;
        public Player Player;
    }
}