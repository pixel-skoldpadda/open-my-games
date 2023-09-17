using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        private readonly AStar.AStar _aStar;

        public ChessGridNavigator()
        {
            _aStar = new AStar.AStar();
        }
        
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            return CanMoveChessTo(unit, from, to) ? _aStar.FindPath(from, to, grid, GetChessMovePattern(unit)) : null;
        }

        private bool CanMoveChessTo(ChessUnitType unit, Vector2Int from, Vector2Int to)
        {
            return unit switch
            {
                ChessUnitType.Pon => CanMovePawn(from, to),
                ChessUnitType.Bishop => CanMoveBishop(from, to),
                _ => true
            };
        }

        private bool CanMovePawn(Vector2Int from, Vector2Int to)
        {
            return from.x == to.x;
        }

        private bool CanMoveBishop(Vector2Int from, Vector2Int to)
        {
            return (from.x + from.y % 2) % 2 == (to.x + to.y % 2) % 2;
        }

        private List<Vector2Int> GetChessMovePattern(ChessUnitType unit)
        {
            switch (unit)
            {
                case ChessUnitType.Rook:
                    return GetRookMovePattern();
                case ChessUnitType.Bishop:
                    return GetBishopMovePattern();
                case ChessUnitType.Pon:
                    return GetPawnMovePattern();
                case ChessUnitType.Knight:
                    return GetKnightMovePattern();
                case ChessUnitType.Queen:
                case ChessUnitType.King:
                {
                    var movePattern = new List<Vector2Int>();
                    movePattern.AddRange(GetRookMovePattern());
                    movePattern.AddRange(GetBishopMovePattern());

                    return movePattern;
                }
                default:
                    throw new Exception($"Move pattern for unit type {unit} not founded.");
            }
        }

        private List<Vector2Int> GetKnightMovePattern()
        {
            return new List<Vector2Int>(new[]
            {
                new Vector2Int(-2, -1),
                new Vector2Int(-1, -2),
                new Vector2Int(-1, 2),
                new Vector2Int(-2, 1),
                new Vector2Int(1, -2),
                new Vector2Int(2, -1),
                new Vector2Int(2, 1),
                new Vector2Int(1, 2),
            });
        }

        private List<Vector2Int> GetPawnMovePattern()
        {
            return new List<Vector2Int>(new[]
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
            });
        }

        private static List<Vector2Int> GetBishopMovePattern()
        {
            return new List<Vector2Int>(new[]
            {
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, -1)
            });
        }

        private List<Vector2Int> GetRookMovePattern()
        {
            return new List<Vector2Int>(new[]
            {
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1)
            });
        }
    }
}