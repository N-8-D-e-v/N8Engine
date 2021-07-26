﻿using System;
using System.Runtime.InteropServices;

namespace N8Engine.Native
{
    internal static class ConsoleWindow
    {
        /// <summary>
        /// Returns a handle to to the specified standard device (input, output, or error).
        /// </summary>
        /// <param name="nStdHandle"> The number of the standard device. </param>
        /// <returns> A handle to to the specified standard device (input, output, or error). </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        public const int STANDARD_INPUT_HANDLE_NUMBER = -10;
        public const int STANDARD_OUTPUT_HANDLE_NUMBER = -11;
        public const int STANDARD_ERROR_HANDLE_NUMBER = -12;

        public static readonly IntPtr StandardInputHandle = GetStdHandle(STANDARD_INPUT_HANDLE_NUMBER);
        public static readonly IntPtr StandardOutputHandle = GetStdHandle(STANDARD_OUTPUT_HANDLE_NUMBER);
        public static readonly IntPtr StandardErrorHandle = GetStdHandle(STANDARD_ERROR_HANDLE_NUMBER);
    }
}