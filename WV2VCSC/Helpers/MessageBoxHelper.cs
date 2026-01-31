using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace WV2VCSC.Helpers
{
    public static class MessageBoxHelper
    {
        #region [~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Win32 APIs]
        [StructLayout(LayoutKind.Sequential)]
        private struct _SRECT
        {
            public _SRECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
            public _SRECT(int right, int bottom)
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


        private const uint _SWP_NOSIZE = 0x0001;


        private const string _dfnmUser32 = "user32.dll";
        private const string _dfnmKernel32 = "kernel32.dll";


        [DllImport(_dfnmUser32, EntryPoint = "GetParent", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr prGetParent(IntPtr hWnd);


        [DllImport(_dfnmUser32, EntryPoint = "GetWindowRect", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool prGetWindowRect(IntPtr hWnd, out _SRECT lpRect);


        [DllImport(_dfnmUser32, EntryPoint = "SetWindowPos", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool prSetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);


        [DllImport(_dfnmKernel32, EntryPoint = "GetCurrentThreadId", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int prGetCurrentThreadId();


        private delegate bool _EnumThreadWndProc(IntPtr hwnd, IntPtr lParam);

        [DllImport(_dfnmUser32, EntryPoint = "EnumThreadWindows", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool prEnumThreadWindows(uint dwThreadId, _EnumThreadWndProc lpfn, IntPtr lParam); 
        #endregion



        private static bool EnumThreadProc(IntPtr hWnd, IntPtr lParam)
        {
            IntPtr ht = prGetParent(hWnd);
            if (ht == _hp)
            {
                if (prGetWindowRect(hWnd, out _SRECT raw))
                {
                    int tx = _rct.Left + (_rct.Width / 2) - (raw.Width / 2);
                    int ty = _rct.Top + (_rct.Height / 2) - (raw.Height / 2);

                    prSetWindowPos(
                        hWnd, IntPtr.Zero,
                        tx, ty, 0, 0,
                        _SWP_NOSIZE);
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void InvokeProc()
        {
            prEnumThreadWindows((uint)_cid, EnumThreadProc, _hp);
        }


        private static Control _target;
        private static int _cid;
        private static IntPtr _hp;
        private static Rectangle _rct;

        public static DialogResult Show(Control owner, string title, string content)
        {
            _target = owner;
            _cid = prGetCurrentThreadId();
            _hp = _target.Handle;
            _rct = _target.Bounds;

            _target.BeginInvoke((MethodInvoker)InvokeProc);

            DialogResult rdr = MessageBox.Show(_target, content, title);
            _target = null;
            _cid = default;
            _hp = default;
            _rct = default;

            return rdr;
        }

        public static string Title = "알림(Notify)";
        public static DialogResult Show(Control owner, string content)
        {
            return Show(owner, Title, content);
        }

    }
}
