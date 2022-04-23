using System;
using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    /// <summary>
    /// Represents the squared distance to a particular neighbour. All fields are readonly to
    /// stop developers from trying to manually set IsValid to true.
    /// <br></br><br></br>
    /// IsValid exists to avoid a <i>nullcheck</i> against Neighbour, improving performance.
    /// </summary>
    public struct NeighbourDistanceInfo : IComparable<NeighbourDistanceInfo>
    {
        public readonly FindNearestNeighbour Neighbour;
        public readonly Vector3 NeighbourPosition;
        public readonly float DistanceSqr;
        public readonly bool IsValid;

        public NeighbourDistanceInfo(FindNearestNeighbour neighbour, Vector3 neighbourPosition, float distanceSqr)
        {
            Neighbour = neighbour;
            NeighbourPosition = neighbourPosition;
            DistanceSqr = distanceSqr;
            IsValid = true;
        }

        public int CompareTo(NeighbourDistanceInfo other)
        {
            return DistanceSqr.CompareTo(other.DistanceSqr);
        }
    }
}
