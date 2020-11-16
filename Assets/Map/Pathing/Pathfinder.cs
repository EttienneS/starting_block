using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Map.Pathing
{
    public class Pathfinder : MonoBehaviour
    {
        private static Pathfinder _instance;

        public static Pathfinder Instance
        {
            get
            {
                return _instance != null ? _instance : (_instance = FindObjectOfType<Pathfinder>());
            }
            set
            {
                _instance = value;
            }
        }

        private CellPriorityQueue _searchFrontier;
        private int _searchFrontierPhase;

        private Queue<PathRequest> _pathQueue = new Queue<PathRequest>();
        private Task _currentTask;

        public void Update()
        {
            ResolvePaths();
        }

        private void ResolvePaths()
        {
            if (_currentTask == null || _currentTask.IsCompleted)
            {
                if (_pathQueue.Count != 0)
                {
                    _currentTask = new Task(() => ResolvePathRequest(_pathQueue.Dequeue()));
                    _currentTask.Start();
                }
            }
        }

        private void ResolvePathRequest(PathRequest request)
        {
            try
            {
                if (SearchForPath(request))
                {
                    var fromCell = request.From;
                    var toCell = request.To;
                    if (fromCell != null && toCell != null)
                    {
                        var path = new List<IPathFindableCell> { toCell };
                        var current = toCell;
                        while (current != fromCell)
                        {
                            current = current.PathFrom;
                            path.Add(current);
                        }
                        path.Reverse();
                        request.PopulatePath(path);
                    }
                }
                else
                {
                    request.MarkPathInvalid();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                _currentTask = null;
            }
        }

        public PathRequest CreatePathRequest(IPathFindableCell source, IPathFindableCell target)
        {
            var request = new PathRequest(source, target);
            _pathQueue.Enqueue(request);
            return request;
        }

        private bool SearchForPath(PathRequest request)
        {
            try
            {
                var fromCell = request.From;
                var toCell = request.To;

                _searchFrontierPhase += 2;

                if (_searchFrontier == null)
                {
                    _searchFrontier = new CellPriorityQueue();
                }
                else
                {
                    _searchFrontier.Clear();
                }

                fromCell.SearchPhase = _searchFrontierPhase;
                fromCell.SearchDistance = 0;
                _searchFrontier.Enqueue(fromCell);

                while (_searchFrontier.Count > 0)
                {
                    var current = _searchFrontier.Dequeue();
                    current.SearchPhase++;

                    if (current == toCell)
                    {
                        return true;
                    }

                    for (var d = Direction.N; d <= Direction.NW; d++)
                    {
                        var neighbor = current.GetNeighbor(d);

                        var neighborTravelCost = 1f;
                        if (neighbor == null
                              || neighbor.SearchPhase > _searchFrontierPhase)
                        {
                            continue;
                        }

                        if (neighborTravelCost < 0)
                        {
                            continue;
                        }

                        var distance = current.SearchDistance + neighborTravelCost;
                        if (neighbor.SearchPhase < _searchFrontierPhase)
                        {
                            neighbor.SearchPhase = _searchFrontierPhase;
                            neighbor.SearchDistance = distance;
                            neighbor.PathFrom = current;
                            neighbor.SearchHeuristic = neighbor.DistanceTo(toCell);
                            _searchFrontier.Enqueue(neighbor);
                        }
                        else if (distance < neighbor.SearchDistance)
                        {
                            var oldPriority = neighbor.SearchPriority;
                            neighbor.SearchDistance = distance;
                            neighbor.PathFrom = current;
                            _searchFrontier.Change(neighbor, oldPriority);
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Pathing error: " + ex.ToString());
                throw;
            }
        }

        internal void ResolveAll()
        {
            while (_pathQueue.Count > 0)
            {
                ResolvePaths();
            }
        }

        public static void LinkCellsToNeighbors(PathableCell[,] cells, int lenght)
        {
            for (var z = 0; z < 0 + lenght; z++)
            {
                for (var x = 0; x < 0 + lenght; x++)
                {
                    var cell = cells[x, z];
                    if (x > 0)
                    {
                        cell.SetNeighbor(Direction.W, cells[x, z]);

                        if (z > 0)
                        {
                            cell.SetNeighbor(Direction.SW, cells[x - 1, z - 1]);

                            if (x < lenght - 1)
                            {
                                cell.SetNeighbor(Direction.SE, cells[x + 1, z - 1]);
                            }
                        }
                    }

                    if (z > 0)
                    {
                        cell.SetNeighbor(Direction.S, cells[x, z - 1]);
                    }
                }
            }
        }
    }
}