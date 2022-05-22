namespace Prince
{
    /// <summary>
    /// Common interface of all state machine trackers.
    ///
    /// <ul>
    /// <li> T: Enum type with all state machine states.</li>
    /// </ul>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStateMachineStatus<T>
    {
        public T CurrentState { get; set; }
    }
}