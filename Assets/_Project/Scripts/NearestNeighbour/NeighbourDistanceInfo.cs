using System;
using UnityEngine;

namespace NearestNeighbour
{
    /// <summary>
    /// Represents the squared distance to a particular neighbour. Readonly properties were used instead of standard public fields to ensure devs cannot
    /// manually force IsValid to be true, and are forced to construct using the constructor.
    /// <br></br><br></br>
    /// IsValid exists to avoid a <i>nullcheck</i> against Neighbour, improving performance.
    /// </summary>
    public struct NeighbourDistanceInfo : IComparable<NeighbourDistanceInfo>
    {
        public NeighbourDistanceInfo(GameObject neighbour, float distanceSqr)
        {
            Neighbour = neighbour;
            DistanceSqr = distanceSqr;
            IsValid = true;
        }

        public GameObject Neighbour { get; }
        public float DistanceSqr { get; }
        public bool IsValid { get; }

        public int CompareTo(NeighbourDistanceInfo other)
        {
            return DistanceSqr.CompareTo(other.DistanceSqr);
        }
    }
}
