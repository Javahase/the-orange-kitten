using Models.PlayerData;

namespace Data.Collector
{
    /// <summary>
    /// The Data collector collects player metrics throughout the game.
    /// </summary>
    public abstract class DataCollector: SingletonObject<DataCollector>
    {

        /// <summary>
        /// The ID of the participant who started the game.
        /// </summary>
        protected string ParticipantId;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    
        /// <summary>
        /// Assigns a participant id and starts the necessary services to perform data collection.
        /// </summary>
        /// <param name="participantId">The participant ID the player got assigned.</param>
        public abstract void Init(string participantId);

        /// <summary>
        /// Adds a new puzzle-entry.
        /// </summary>
        /// <param name="data">The level-data.</param>
        public abstract void StartLevel(PuzzleData data);

        /// <summary>
        /// Saves the data from the internal <see cref="PlayerData"/>
        /// </summary>
        public abstract void Save();

    }
}