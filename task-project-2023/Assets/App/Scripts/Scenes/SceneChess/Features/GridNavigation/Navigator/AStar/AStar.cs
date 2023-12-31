using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator.AStar
{
    public class AStar
    {
        public List<Vector2Int> FindPath(Vector2Int from, Vector2Int to, ChessGrid grid, List<Vector2Int> movePattern)
        {
            var startNode = new Node(from);
            var endNode = new Node(to);

            var reachable = new List<Node> { startNode };
            var explored = new List<Node>();

            while (reachable.Count != 0)
            {
                var node = ChooseNextNode(reachable, endNode);
                if (node.Position == endNode.Position)
                {
                    return BuildPath(node, startNode);
                }
                
                reachable.Remove(node);
                explored.Add(node);

                var adjacentNodes = GetAdjacentNodes(node, explored, movePattern, grid);
                for (var i = 0; i < adjacentNodes.Count; i++)
                {
                    var adjacentNode = adjacentNodes[i];
                    if (!reachable.Contains(adjacentNode))
                    {
                        reachable.Add(adjacentNode);
                    }

                    if (node.Weight + 1 < adjacentNode.Weight)
                    {
                        adjacentNode.Previous = node;
                        adjacentNode.Weight = node.Weight + 1;
                    }
                }
            }
            return null;
        }

        private List<Node> GetAdjacentNodes(Node node, IReadOnlyList<Node> explored, IReadOnlyList<Vector2Int> movePattern, ChessGrid grid)
        {
            var adjacentNodes = new List<Node>();
            var position = node.Position;

            for (var i = 0; i < movePattern.Count; i++)
            {
                var adjacentPosition = position + movePattern[i];
                if (IsExploredPosition(adjacentPosition, explored))
                {
                    continue;
                }
                
                if (CanMoveTo(adjacentPosition, grid))
                {
                    adjacentNodes.Add(new Node(adjacentPosition));
                }
            }
            return adjacentNodes;
        }

        private bool CanMoveTo(Vector2Int position, ChessGrid grid)
        {
            if (IsPositionExists(position, grid))
            {
                var chessUnit = grid.Get(position);
                return chessUnit == null || chessUnit.IsAvailable;
            }
            return false;
        }

        private bool IsPositionExists(Vector2Int position, ChessGrid grid)
        {
            var size = grid.Size;
            var x = position.x;
            var y = position.y;

            return x >= 0 && x < size.x && y >= 0 && y < size.y;
        }

        private bool IsExploredPosition(Vector2Int position, IReadOnlyList<Node> explored)
        {
            for (var i = 0; i < explored.Count; i++)
            {
                if (position == explored[i].Position)
                {
                    return true;
                }
            }
            return false;
        }
        
        private List<Vector2Int> BuildPath(Node node, Node starNode)
        {
            var path = new List<Vector2Int>();
            while (node.Position != starNode.Position)
            {
                path.Insert(0, node.Position);
                node = node.Previous;
            }
            return path;
        }

        private Node ChooseNextNode(IReadOnlyList<Node> reachable, Node endNode)
        {
            Node node = null;
            var minWeight = -1;
            
            for (var i = 0; i < reachable.Count; i++)
            {
                var reachableNode = reachable[i];
                var weight = reachableNode.Weight + EstimateManhattanDistance(reachableNode, endNode);
                
                if (minWeight > weight)
                {
                    minWeight = weight;
                    node = reachableNode;
                }
            }
            return node;
        }

        private int EstimateManhattanDistance(Node node, Node endNode)
        {
            var position = node.Position;
            var endNPosition = endNode.Position;

            return Math.Abs(position.x - endNPosition.x) + Math.Abs(position.y - endNPosition.y);
        }
    }
}