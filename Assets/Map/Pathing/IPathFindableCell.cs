namespace Assets.Map.Pathing
{
    public interface IPathFindableCell
    {
        IPathFindableCell NextWithSamePriority { get; set; }
        IPathFindableCell PathFrom { get; set; }
        float SearchDistance { get; set; }
        int SearchHeuristic { get; set; }
        int SearchPhase { get; set; }

        int SearchPriority { get; }
        float TravelCost { get; }

        int X { get; set; }

        int Z { get; set; }

        int DistanceTo(IPathFindableCell other);

        IPathFindableCell GetNeighbor(Direction direction);
    }
}