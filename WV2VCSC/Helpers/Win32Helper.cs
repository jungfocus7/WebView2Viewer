using System;
using System.Runtime.InteropServices;


namespace WV2VCSC.Helpers
{
    /*
    #region ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 1
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;


        public override string ToString()
        {
            string txt = $"X: {X}, Y: {Y}";
            return txt;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public RECT(int right, int bottom)
        {
            Left = 0;
            Top = 0;
            Right = right;
            Bottom = bottom;
        }

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;


        public int Width
        {
            get { return Right - Left; }
        }

        public int Height
        {
            get { return Bottom - Top; }
        }

        public override string ToString()
        {
            string txt =
                $"Left: {Left}, Top: {Top}, Width: {Width}, Height: {Height}";
            return txt;
        }
    }
    #endregion



    public static class Win32Helper
    {
        #region [01) Constants]
        //~~~~~~~ WindowStyle
        public const uint WS_OVERLAPPED = 0x00000000;
        public const uint WS_POPUP = 0x80000000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_MINIMIZE = 0x20000000;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_DISABLED = 0x08000000;
        public const uint WS_CLIPSIBLINGS = 0x04000000;
        public const uint WS_CLIPCHILDREN = 0x02000000;         // WS_EX_COMPOSITED
        public const uint WS_MAXIMIZE = 0x01000000;
        public const uint WS_CAPTION = 0x00C00000;
        public const uint WS_BORDER = 0x00800000;
        public const uint WS_DLGFRAME = 0x00400000;
        public const uint WS_VSCROLL = 0x00200000;
        public const uint WS_HSCROLL = 0x00100000;
        public const uint WS_SYSMENU = 0x00080000;
        public const uint WS_THICKFRAME = 0x00040000;
        public const uint WS_TABSTOP = 0x00010000;
        public const uint WS_MINIMIZEBOX = 0x00020000;
        public const uint WS_MAXIMIZEBOX = 0x00010000;
        public const uint WS_EX_DLGMODALFRAME = 0x00000001;
        public const uint WS_EX_MDICHILD = 0x00000040;
        public const uint WS_EX_TOOLWINDOW = 0x00000080;
        public const uint WS_EX_CLIENTEDGE = 0x00000200;
        public const uint WS_EX_CONTEXTHELP = 0x00000400;
        public const uint WS_EX_RIGHT = 0x00001000;
        public const uint WS_EX_LEFT = 0x00000000;
        public const uint WS_EX_RTLREADING = 0x00002000;
        public const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const uint WS_EX_CONTROLPARENT = 0x00010000;
        public const uint WS_EX_STATICEDGE = 0x00020000;
        public const uint WS_EX_APPWINDOW = 0x00040000;
        public const uint WS_EX_LAYERED = 0x00080000;
        public const uint WS_EX_TOPMOST = 0x00000008;
        public const uint WS_EX_LAYOUTRTL = 0x00400000;
        public const uint WS_EX_NOINHERITLAYOUT = 0x00100000;


        //~~~~~~~ GetWindowLong
        public const int GWL_WNDPROC = -4;
        public const int GWL_HWNDPARENT = -8;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int GWL_ID = -12;


        //~~~~~~~ ShowWindow
        public const int SW_HIDE = 0;
        public const int SW_NORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_MAX = 10;


        //~~~~~~~ SetWindowPos
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOZORDER = 0x0004;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const uint SWP_HIDEWINDOW = 0x0080;
        public const uint SWP_DRAWFRAME = 0x0020;
        public const uint SWP_NOOWNERZORDER = 0x0200;
        #endregion





        private const string _dfnmUser32 = "user32.dll";
        private const string _dfnmKernel32 = "kernel32.dll";



        [DllImport(_dfnmUser32, EntryPoint = "GetWindowRect", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport(_dfnmUser32, EntryPoint = "GetClientRect", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport(_dfnmUser32, EntryPoint = "SetWindowPos", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);


        [DllImport(_dfnmUser32, EntryPoint = "AdjustWindowRectEx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

        [DllImport(_dfnmUser32, EntryPoint = "AdjustWindowRectExForDpi", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AdjustWindowRectExForDpi(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle, uint dpi);



        #region [~~~~~~~~~~ WindowLong]
        [DllImport(_dfnmUser32, EntryPoint = "GetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport(_dfnmUser32, EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return GetWindowLong32(hWnd, nIndex);
            else
                return GetWindowLongPtr64(hWnd, nIndex);
        }


        [DllImport(_dfnmUser32, EntryPoint = "SetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(_dfnmUser32, EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
                return SetWindowLong32(hWnd, nIndex, dwNewLong);
            else
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }


        public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport(_dfnmUser32, EntryPoint = "SetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowLong32_wp(IntPtr hWnd, int nIndex, WndProc wndProc);

        [DllImport(_dfnmUser32, EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64_wp(IntPtr hWnd, int nIndex, WndProc wndProc);

        public static IntPtr SetWindowLong_wp(IntPtr hWnd, int nIndex, WndProc wndProc)
        {
            if (IntPtr.Size == 4)
                return SetWindowLong32_wp(hWnd, nIndex, wndProc);
            else
                return SetWindowLongPtr64_wp(hWnd, nIndex, wndProc);
        }
        #endregion



        [DllImport(_dfnmUser32, EntryPoint = "IsWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport(_dfnmUser32, EntryPoint = "ShowWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);








        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~1
        [DllImport(_dfnmUser32, EntryPoint = "GetWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport(_dfnmUser32, EntryPoint = "GetActiveWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetActiveWindow();

        [DllImport(_dfnmUser32, EntryPoint = "GetParent", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);



        [DllImport(_dfnmUser32, EntryPoint = "FindWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport(_dfnmUser32, EntryPoint = "FindWindowEx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className, string windowTitle);





        [DllImport(_dfnmKernel32, EntryPoint = "GetCurrentThreadId", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetCurrentThreadId();


        public delegate bool EnumThreadWndProc(IntPtr hwnd, IntPtr lParam);

        [DllImport(_dfnmUser32, EntryPoint = "EnumThreadWindows", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);


    }

    */
}
