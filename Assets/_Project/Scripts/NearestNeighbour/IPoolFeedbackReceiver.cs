namespace NearestNeighbour
{
    /// <summary>
    /// Only the first component which implements this interface will be notified by the pooling service
    /// </summary>
    public interface IPoolFeedbackReceiver
    {
        /// <summary>
        /// Called when the object is first instantiated
        /// </summary>
        void HandleInstantiated();
    }
}
