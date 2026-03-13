using System;

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

        public bool CancelMethodForReimplementation(Action reimplementationAction)
        {
            return _tracker.CancelForReimplementation(reimplementationAction);
        }

        public bool CancelMethodForReplicatingReimplementation(Action reimplementationAction)
        {
            return _tracker.CancelForReplicatingReimplementation(reimplementationAction);
        }

        private EventMethodCancellationTracker _tracker;
    }

    public struct EventMethodCancelInfo
    {
        public bool Cancelled { get => _cancelled; }
        public EventMethodCancelReason? Reason 
        { 
            get
            {
                if (Cancelled)
                {
                    return _cancelReason;
                }
                else
                {
                    return null;
                }
            }
        }

        private bool _cancelled;
        private EventMethodCancelReason _cancelReason;

        internal EventMethodCancelInfo(bool cancelled, EventMethodCancelReason? reason)
        {
            _cancelled = cancelled;
            _cancelReason = reason.GetValueOrDefault(EventMethodCancelReason.CancelAction);
        }
    }

    public enum EventMethodCancelReason : byte
    {
        CancelAction = 8, // Cancelled to simply fully cancel the action. For example, if the event is a hurt method call, and this is the cancel reason, the cancel caller presumably wanted the hurt target not to be hurt
        SoftCancelAction = 9, // Cancelled to cancel the original implementation, or replicating implementations
        Reimplement = 16, // Cancelled to reimplement the method. This doesn't imply the reimplemntation is meant to replicate the original, but there presumably is some sane linkage.
        ReimplementAndReplicate = 32, // Cancelled to reimplement the method in a way that replicates the original with some minor modification. Probably not generally preferable to do this one (see Harmony Transpiling instead), but sometimes it's the fastest way to get something working.
    }
    
    /*
        Simply a reference to a boolean tracking if a called method should be cancelled, intended for event pairs for methods using the Pre & Post event pattern.
        This reference should be shared across all listeners to the event, hence being a class.
    */
    public class EventMethodCancellationTracker
    {
        public bool Cancelled { get => _cancelled; }
        public EventMethodCancelReason? Reason 
        { 
            get
            {
                if (Cancelled)
                {
                    return _cancelReason;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool TryInvokeReimplementation()
        {
            if (_reimplementationAction == null)
            {
                return false;
            }

            _reimplementationAction.Invoke();
            _reimplementationAction = null;
            return true;
        }

        public EventMethodCancelInfo GetCancelInfo()
        {
            return new EventMethodCancelInfo(Cancelled, Reason);
        }

        public void CancelMethod()
        {
            if (Cancelled && (_cancelReason == EventMethodCancelReason.Reimplement || _cancelReason == EventMethodCancelReason.ReimplementAndReplicate))
            {
                _reimplementationAction = null;
            }
            
            _cancelReason = EventMethodCancelReason.CancelAction;
            _cancelled = true;
        }

        public void SoftlyCancelMethod()
        {
            if (Cancelled && _cancelReason == EventMethodCancelReason.ReimplementAndReplicate)
            {
                _reimplementationAction = null;
            }

            _cancelReason = EventMethodCancelReason.SoftCancelAction;
            _cancelled = true;
        }

        public bool CancelForReimplementation(Action reimplementationAction)
        {
            if ((Cancelled && (_cancelReason == EventMethodCancelReason.CancelAction)) || _reimplementationAction != null )
            {
                return false;
            }

            _cancelReason = EventMethodCancelReason.Reimplement;
            _cancelled = true;
            _reimplementationAction = reimplementationAction;
            return true;
        }
        public bool CancelForReplicatingReimplementation(Action reimplementationAction)
        {
            if ((Cancelled && (_cancelReason == EventMethodCancelReason.SoftCancelAction || _cancelReason == EventMethodCancelReason.CancelAction)) || _reimplementationAction != null )
            {
                return false;
            }

            _cancelReason = EventMethodCancelReason.ReimplementAndReplicate;
            _cancelled = true;
            _reimplementationAction = reimplementationAction;
            return true;
        }

        public void Reset()
        {
            _cancelled = false;
            _reimplementationAction = null;
        }

        public EventMethodCanceler GetCanceler()
        {
            return new EventMethodCanceler(this);
        }

        private bool _cancelled = false;
        private EventMethodCancelReason _cancelReason = EventMethodCancelReason.CancelAction;
        private Action _reimplementationAction = null;
    }
}