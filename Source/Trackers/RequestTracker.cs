namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class RequestTracker
    {
        public uint Requests { get; private set; } = 0;
        public bool Requested => Requests > 0;
        public bool Unrequested => Requests == 0;

        public void Request()
        {
            Requests += 1;
        }

        public void Unrequest()
        {
            Assert.IsFalse(Requests == 0);
            Requests -= 1;
        }
    }
}