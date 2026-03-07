namespace Nyxpiri.ULTRAKILL.NyxLib
{
    /* 
        Reference to an EventMethodCancellationTracker that can see if a CancellationTracker has Cancelled an event, and can cancel the event, but *cannot* reset it. 
        These are intended to be passed to listeners.
    */
    public struct EventMethodCanceler
    {
        public EventMethodCanceler(EventMethodCancellationTracker tracker)
        {
            _tracker = tracker;
        }

        public bool Cancelled { get => _tracker.Cancelled; }

        public void CancelMethod()
        {
            _tracker.CancelMethod();
        }

        private EventMethodCancellationTracker _tracker;
    }

    /*
        Simply a reference to a boolean tracking if a called method should be cancelled, intended for event pairs for methods using the Pre & Post event pattern.
        This reference should be shared across all listeners to the event, hence being a class.
    */
    public class EventMethodCancellationTracker
    {
        public bool Cancelled { get => _cancelled; }
        
        public void CancelMethod()
        {
            _cancelled = true;
        }

        public void Reset()
        {
            _cancelled = false;
        }

        public EventMethodCanceler GetCanceler()
        {
            return new EventMethodCanceler(this);
        }

        private bool _cancelled = false;
    }
}