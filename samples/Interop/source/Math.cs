using System;
using System.Runtime.CompilerServices;

namespace NF.AwesomeLib
{
	public class Math
    {
        /// <summary>
        /// Crunches value through a super complicated and secret calculation algorithm.
        /// </summary>
        /// <param name="value">Value to crunch.</param>
        /// <returns></returns>
        public double SuperComplicatedCalculation(double value)
        {
            return NativeSuperComplicatedCalculation(value);
        }

        #region Stubs

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern double NativeSuperComplicatedCalculation(double value);

        #endregion stubs
    }
}
