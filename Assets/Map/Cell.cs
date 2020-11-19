using Assets.Map.Pathing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Map
{
    public class Cell : PathableCell
    {
        private List<Cell> _nonNullNeighbours;

        public Cell(int x, int z, float height, Color color)
        {
            X = x;
            Z = z;
            Y = height;
            Color = color;
        }

        public Color Color { get; set; }

        public new List<Cell> NonNullNeighbors
        {
            get
            {
                if (_nonNullNeighbours == null)
                {
                    _nonNullNeighbours = base.NonNullNeighbors.ConvertAll(c => c as Cell).ToList();
                }
                return _nonNullNeighbours;
            }
        }

        public override float TravelCost
        {
            get
            {
                return Y;
            }
        }
    }
}