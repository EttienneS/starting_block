using System.Collections.Generic;
using UnityEngine;

namespace Assets.Map.Pathing
{
    public class PathRequest
    {
        private bool _invalid;
        private List<IPathFindableCell> _path;

        public PathRequest(IPathFindableCell from, IPathFindableCell to)
        {
            From = from;
            To = to;
        }

        public IPathFindableCell From { get; set; }
        public IPathFindableCell To { get; set; }

        public List<IPathFindableCell> GetPath()
        {
            return _path;
        }

        public void MarkPathInvalid()
        {
            Debug.LogWarning($"No path found from {From} to {To}");
            _invalid = true;
        }

        public void PopulatePath(List<IPathFindableCell> path)
        {
            _path = path;
        }

        public bool Ready()
        {
            if (_invalid)
            {
                throw new InvalidPathException(this);
            }
            return _path != null;
        }
    }
}