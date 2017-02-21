using System;
using System.Collections.Generic;
using BigMath;

namespace PrimeCounter.Helper
{
    class PrimeCheckingUtil
    {
        #region primetests

        //somehow this is extremely slow. maybe 
        //TODO: implement own BigInt?
        public static bool FermatTest(int candidate, int samples)
        {
            if (samples <= 0)
                throw new ArgumentException("take at least 1 sample");

            if (candidate == 1 || 
                candidate == 2)
            {
                return true;
            }
            

            samples = Math.Min(Math.Max(candidate - 2, 0), samples);
            var rnd = new Random();
            var allreadySampled = new List<int>();
            for (var i = 0; i < samples; i++)
            {
                var sample = rnd.Next(2, candidate -1);
                while(allreadySampled.Contains(sample))
                    sample = rnd.Next(2, candidate - 1);
                var withPowerOfCandidate = ((BigInteger)sample).Pow(candidate); 
                var modedByCandidate = (int)(withPowerOfCandidate % candidate);

                if (modedByCandidate != sample)
                    return false;
            }

            return true;
        }

        public static bool BruteForce(int candidate)
        {
            for (var i = 2; i < candidate; i++)
            {
                if (candidate % i == 0)
                    return false;
            }

            return true;
        }

        //wip
        public static bool MRT(int candidate, int quality)
        {
            throw new NotImplementedException();   
        }
        #endregion

    }
}
