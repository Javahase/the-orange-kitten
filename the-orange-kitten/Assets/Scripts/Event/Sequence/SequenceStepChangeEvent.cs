using Sequence;

namespace Event.Sequence
{
    /// <summary>
    /// Event that is called when a <see cref="GameSequence{T}"/> changes its current step.
    /// </summary>
    /// <typeparam name="T">The type of items the <see cref="GameSequence{T}"/> deals with.</typeparam>
    public class SequenceStepChangeEvent<T>
    {
        /// <summary>
        /// The sequence the event happened in.
        /// </summary>
        public readonly GameSequence<T> Source;
        
        /// <summary>
        /// The step the sequence previously had.
        /// </summary>
        public int PreviousStep;
        
        /// <summary>
        /// The step the sequence now has.
        /// </summary>
        public int CurrentStep;
        
        /// <summary>
        /// The object at index <see cref="PreviousStep"/>.
        /// </summary>
        public T Previous;
        
        /// <summary>
        /// The object at index <see cref="CurrentStep"/>.
        /// </summary>
        public T Current;

        public SequenceStepChangeEvent(GameSequence<T> source, int previousStep, int currentStep, T previous, T current)
        {
            Source = source;
            PreviousStep = previousStep;
            CurrentStep = currentStep;
            Previous = previous;
            Current = current;
        }
    }
}