using System.Security.Cryptography;

namespace EnvLib
{
    public class EnvFileBuilder
    {
        public const string EnvFileName = "environment0f617e02.bin";

        private const int ModuleCountOffset = 8;
        private const byte ModuleCountXorValue = 0xA7;
        private const byte MatchTypeXorValue = 0x9F;
        private const byte MatchValueXorValue = 0xD3;
        private const int MaxModuleCount = 255;
        private const int MixSeedOffset = 20;
        private const int MatchTypeOffset = 25;
        private const int MatchValueOffset = 29;
        private const int FeatureFlagsOffset = 60;
        private const int ModuleDataOffset = 64;
        private const int KeySeedOffset = 16 * 1024;
        private const byte FeatureFlagsXorValue = 0x5C;
        private const int HashLength = 32;
        private const int VersionMajorOffset = 10;
        private const int VersionMinorOffset = 11;
        private const int VersionPatchOffset = 12;
        private const byte VersionMajorXorValue = 0x4A;
        private const byte VersionMinorXorValue = 0xC8;
        private const byte VersionPatchXorValue = 0x1C;
        private const uint KeySeedXorValue = 0x3B527B1C;
        private const int TimestampOffset = 30;
        private const long TimestampXorValue = 0x0A5DFB499C6830CD;

        byte[] data = new byte[20 * 1024];


        void SetHeaderAndFooter()
        {
            data[0] = 0x31;
            data[1] = 0x9A;
            data[2] = 0xFF;
            data[3] = 0xA2;

            data[data.Length - 4] = 0x44;
            data[data.Length - 3] = 0x5A;
            data[data.Length - 2] = 0x4D;
            data[data.Length - 1] = 0xAA;
        }

        public EnvFileBuilder FillData(SourceModuleInfo[] modules)
        {
#if NET5_0_OR_GREATER
            RandomNumberGenerator.Fill(data);
#else
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }
#endif
            SetHeaderAndFooter();
            SetModuleData(modules);
            SetMatchTypeInternal(ModuleCheckType.All, 0);
            SetFeatureFlags(new FeatureFlags(false, false, false, false, false, false));
            SetKeySeed(0);
            SetFileVersion(new VersionInfo(0, 0, 0));

            return this;
        }

        public EnvFileBuilder SetKeySeed(uint keySeed)
        {
            uint obfuscatedSeed = keySeed ^ KeySeedXorValue;
            data[KeySeedOffset + 0] = (byte)(obfuscatedSeed >> 24);
            data[KeySeedOffset + 1] = (byte)(obfuscatedSeed >> 16);
            data[KeySeedOffset + 2] = (byte)(obfuscatedSeed >> 8);
            data[KeySeedOffset + 3] = (byte)(obfuscatedSeed >> 0);
            return this;
        }

        private EnvFileBuilder AddSignature()
        {
#if NET5_0_OR_GREATER
            using var signingKey = ECDsa.Create();
            signingKey.ImportECPrivateKey(EnvFileKeyProvider.PrivateKey, out _);
            var hashAlgorithm = HashAlgorithmName.SHA256;
            var signature = signingKey.SignData(new ReadOnlySpan<byte>(data, 0, 18 * 1024), hashAlgorithm);
            Array.Copy(signature, 0, data, 18 * 1024, signature.Length);
#else
            // ECPrivateKey (RFC 5915) DER for P-256: D at offset 7, X at offset 57, Y at offset 89
            var privateKeyBytes = EnvFileKeyProvider.PrivateKey;
            byte[] d = new byte[32], x = new byte[32], y = new byte[32];
            Array.Copy(privateKeyBytes, 7, d, 0, 32);
            Array.Copy(privateKeyBytes, 57, x, 0, 32);
            Array.Copy(privateKeyBytes, 89, y, 0, 32);

            // BCRYPT_ECCPRIVATE_BLOB: magic (4) + keySize (4) + X (32) + Y (32) + D (32)
            byte[] blob = new byte[104];
            blob[0] = 0x45; blob[1] = 0x43; blob[2] = 0x53; blob[3] = 0x32; // BCRYPT_ECDSA_PRIVATE_P256_MAGIC
            blob[4] = 32;                                                       // key size in bytes
            Array.Copy(x, 0, blob, 8, 32);
            Array.Copy(y, 0, blob, 40, 32);
            Array.Copy(d, 0, blob, 72, 32);

            using (var cngKey = CngKey.Import(blob, CngKeyBlobFormat.EccPrivateBlob))
            using (var signingKey = new ECDsaCng(cngKey))
            {
                var signature = signingKey.SignData(data, 0, 18 * 1024, HashAlgorithmName.SHA256);
                Array.Copy(signature, 0, data, 18 * 1024, signature.Length);
            }
#endif
            return this;
        }

        public EnvFileBuilder SetMatchType(ModuleCheckType matchType, byte matchValue = 0)
        {
            SetMatchTypeInternal(matchType, matchValue);

            return this;
        }

        private void SetMatchTypeInternal(ModuleCheckType matchType, byte matchValue)
        {
            data[MatchTypeOffset] = (byte)((byte)matchType ^ MatchTypeXorValue);
            if (matchType == ModuleCheckType.AtLeast)
            {
                data[MatchValueOffset] = (byte)(matchValue ^ MatchValueXorValue);
            }
        }

        public EnvFileBuilder SetFeatureFlags(FeatureFlags featureFlags)
        {
            var featureFlagsByte = (byte)((featureFlags.ModuleCheckEnabled ? 1 : 0) |
                                      (featureFlags.DebugCheckEnabled ? 2 : 0) |
                                      (featureFlags.TimestampCheckEnabled ? 4 : 0));
            data[FeatureFlagsOffset] = (byte)(featureFlagsByte ^ FeatureFlagsXorValue);
            return this;
        }

        public EnvFileBuilder SetFileVersion(VersionInfo versionInfo)
        {
            data[VersionMajorOffset] = (byte)(versionInfo.Major ^ VersionMajorXorValue);
            data[VersionMinorOffset] = (byte)(versionInfo.Minor ^ VersionMinorXorValue);
            data[VersionPatchOffset] = (byte)(versionInfo.Patch ^ VersionPatchXorValue);
            return this;
        }

        public EnvFileBuilder SetModuleHashSeed(uint seed)
        {
            byte[] seedBytes = BitConverter.GetBytes(seed);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(seedBytes);
            }
            Array.Copy(seedBytes, 0, data, MixSeedOffset, seedBytes.Length);
            return this;
        }

        public EnvFileBuilder SaveToFile(string fullPath)
        {
            SetTimestamp();
            AddSignature();
            File.WriteAllBytes(fullPath, data);

            return this;
        }

        public void SetModuleData(SourceModuleInfo[] modules)
        {
            if (modules.Length > MaxModuleCount)
                throw new ArgumentException("Too many modules");
            data[ModuleCountOffset] = (byte)(modules.Length ^ ModuleCountXorValue);
            int offset = ModuleDataOffset;
            byte[] temp = new byte[32];
            foreach (var module in modules)
            {
                Array.Copy(module.FileNameHash, 0, data, offset, HashLength);
                offset += HashLength;
                Array.Copy(module.ContentHash, 0, data, offset, HashLength);
                offset += HashLength;
            }
        }

        private void SetTimestamp()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // convert to bytes using big-endian format and XOR with the obfuscation value
            byte[] timestampBytes = BitConverter.GetBytes(timestamp ^ TimestampXorValue);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }
            Array.Copy(timestampBytes, 0, data, TimestampOffset, timestampBytes.Length);
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

}
