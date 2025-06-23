namespace PreEmptive.Dotfuscator.TestSamples.ConsoleApp.Interfaces
{
    public interface IProcessor
    {
        void Process();
    }

    // abstract static implementation
    public interface IProcessFactory<TSelf>
    where TSelf : IProcessFactory<TSelf>
    {
        // Static abstract method to enforce consistent static creation pattern
        static abstract TSelf CreateFromId(int processId);

        void DisplayProcessDetails();
    }
}