using System;
using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    /// <summary>
    /// Represents the squared distance to a particular neighbour.
    /// <br></br><br></br>
    /// IsValid exists to avoid a <i>nullcheck</i> against Neighbour, improving performance.
    /// </summary>
    public struct NeighbourDistanceInfo : IComparable<NeighbourDistanceInfo>
    {
        public FindNearestNeighbour Neighbour;
        public Vector3 NeighbourPosition;
        public float DistanceSqr;
        public bool IsValid;

        public int CompareTo(NeighbourDistanceInfo other)
        {
            return DistanceSqr.CompareTo(other.DistanceSqr);
        }
    }
}
