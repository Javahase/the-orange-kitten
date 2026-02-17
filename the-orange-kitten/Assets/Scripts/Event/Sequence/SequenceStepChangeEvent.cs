namespace Event.Sequence
{
    public class SequenceStepChangeEvent<T>
    {
        public readonly GameSequence<T> Source;
        public int PreviousStep;
        public int CurrentStep;
        public T Previous;
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