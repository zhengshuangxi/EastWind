namespace Util.Operation
{
    public interface IOperation
    {
        IOperation Start();

        bool IsFinished();

        void Click(string item);
    }
}
