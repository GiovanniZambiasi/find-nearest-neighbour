namespace NearestNeighbour
{
    public struct PlayerStats
    {
        public int FiredProjectiles;
        public int Hits;

        public float Accuracy => FiredProjectiles > 0 ? Hits / (float)FiredProjectiles : 0f;
    }
}
