using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator.AStar
{
    public class Node
    {
        public Vector2Int Position { get; set; }
        public Node Previous { get; set; }
        public int Weight { get; set; }

        public Node(Vector2Int position)
        {
            Position = position;
            Weight = int.MaxValue;
        }
    }
}