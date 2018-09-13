using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeonDirectUI.Win32
{
    public static class Win32API
    {
        public const int GWL_WNDPROC = -4;

        [DllImport("user32", CharSet = CharSet.Ansi, EntryPoint = "GetWindowLongA", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32", CharSet = CharSet.Ansi, EntryPoint = "SetWindowLongA", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SetWindowLong(IntPtr hwnd, int nIndex, IntPtr dwNewinteger);

        [DllImport("user32", CharSet = CharSet.Ansi, EntryPoint = "CallWindowProcA", ExactSpelling = true, SetLastError = true)]
        public static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, int Msg, int wParam, int lParam);
    }
}
