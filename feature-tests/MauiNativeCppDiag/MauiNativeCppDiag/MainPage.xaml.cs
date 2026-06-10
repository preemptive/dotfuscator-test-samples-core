using MauiLib1;
using System;

namespace MauiNativeCppDiag;

public partial class MainPage : ContentPage
{
    private static byte[] _expectedHash = [0xE5, 0x00, 0xFB, 0x19, 0x33, 0xAC, 0x81, 0xB8, 0xE4, 0x00, 0x89, 0xBF, 0xE6, 0xC4, 0xAF, 0x45, 0x72, 0xAA, 0xA1, 0x74, 0x9C, 0x44, 0xC3, 0x9C, 0xA4, 0xF6, 0x3C, 0xA0, 0xBF, 0x74, 0xA8, 0x5C];

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnTestBtnClicked(object? sender, EventArgs e)
    {
        const uint KeySeedXorValue = 0x3B527B1C;
        const int KeySeedOffset = 16 * 1024;

        var envData = RuntimeExtensionsNative.GetEnvData();
        uint keySeed = (uint)(envData[KeySeedOffset + 0] << 24 |
                            envData[KeySeedOffset + 1] << 16 |
                            envData[KeySeedOffset + 2] << 8 |
                            envData[KeySeedOffset + 3] << 0);
        keySeed ^= KeySeedXorValue;

        SeedLabel.Text = $"Seed: {keySeed:X8}";

        var hash = RuntimeExtensionsNative.CreateKey2(
        [
            System.Text.Encoding.UTF8.GetBytes("Hello"),
            System.Text.Encoding.UTF8.GetBytes("World"),
        ]);
        Mix(hash, keySeed);

        var isValid = hash.SequenceEqual(_expectedHash);
        
        ResultLabel.Text = $"Hash: {BitConverter.ToString(hash.Take(8).ToArray()).Replace("-", "")}... Result: {(isValid ? "Valid" : "Invalid")}";
    }

    private void OnTestBtnDirectClicked(object? sender, EventArgs e)
    {
        var hash = RuntimeExtensionsNative.CreateKey(
        [
            System.Text.Encoding.UTF8.GetBytes("Hello"),
            System.Text.Encoding.UTF8.GetBytes("World"),
        ]);

        var isValid = hash.SequenceEqual(_expectedHash);
        ResultLabelDirect.Text = $"Hash: {BitConverter.ToString(hash.Take(8).ToArray()).Replace("-", "")}... Result: {(isValid ? "Valid" : "Invalid")}";
    }

    private void OnTestBtnNativeCppClicked(object? sender, EventArgs e)
    {
        var hash = NativeCPP.ComputeKey(
        [
            System.Text.Encoding.UTF8.GetBytes("Hello"),
            System.Text.Encoding.UTF8.GetBytes("World"),
        ]);

        var isValid = hash.SequenceEqual(_expectedHash);
        ResultLabelDirect.Text = $"Hash: {BitConverter.ToString(hash.Take(8).ToArray()).Replace("-", "")}... Result: {(isValid ? "Valid" : "Invalid")}";
    }

    private void OnSetSampleStringBtnClicked(object? sender, EventArgs e)
    {
        ResultLabel.Text = SampleClass.SampleMethod();
    }

    static void Mix(byte[] data, uint seed)
    {
        uint s = seed;
        for (int i = 0; i < data.Length; i++)
        {
            s ^= s << 13;
            s ^= s >> 17;
            s ^= s << 5;
            data[i] ^= (byte)(s & 0xFF);
        }
    }
}
