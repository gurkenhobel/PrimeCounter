using System;

namespace PrimeCounter.Model
{
    [Flags]
    public enum TestResult
    {
        Passed = 0x1,
        Failed = 0x8,
        Pending = 0x2,
        Skipped = 0x4
    }
}
