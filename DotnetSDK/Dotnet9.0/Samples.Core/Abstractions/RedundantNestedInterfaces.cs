namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions;

public interface IBase1<T>
{
    T GetSampleValue();
}
public interface IBase2<T1, T2> : IBase1<T1> { }
public interface ISample1<T> : IBase1<T> { }
public interface ISample2<T1, T2> : ISample1<T1>, IBase2<T1, T2>, IBase1<T1> { }