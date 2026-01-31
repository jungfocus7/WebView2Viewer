Imports System
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports WebView2Viewer__vb.Models


Namespace Helpers
    Public NotInheritable Class HotkeyHelper
#Region "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 1"
        Private Sub New()
        End Sub


        Private Const _WM_HOTKEY As Integer = &H312

        Private Const _HOTKEY_ID As Integer = 31197 + 30000

        Public Const Kmf_None As Integer = 0
        Public Const Kmf_Alt As Integer = 1
        Public Const Kmf_Control As Integer = 2
        Public Const Kmf_Shift As Integer = 4
        Public Const Kmf_Windows As Integer = 8


        Private Const _dllfpUser32 = "user32.dll"

        <DllImport(_dllfpUser32, EntryPoint:="RegisterHotKey",
                    CharSet:=CharSet.Auto, SetLastError:=True)>
        Private Shared Function prRegisterHotHey(
            hwnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer) As Integer
        End Function

        <DllImport(_dllfpUser32, EntryPoint:="UnregisterHotKey",
                    CharSet:=CharSet.Auto, SetLastError:=True)>
        Private Shared Function prUnregisterHotKey(
            hwnd As IntPtr, id As Integer) As Integer
        End Function
#End Region



        Private Shared _hkidcnt As Integer = _HOTKEY_ID
        Private Shared Function prGetHotkeyIdCount() As Integer
            Dim rv As Integer = _hkidcnt + 1
            _hkidcnt = rv
            Return rv
        End Function

        Private Shared _hotkeyInfos As New HashSet(Of HotkeyInfo)


        Public Shared Sub AddHotkey(hki As HotkeyInfo)
            hki.id = prGetHotkeyIdCount()
            _hotkeyInfos.Add(hki)
            prRegisterHotHey(hki.hwnd, hki.id, hki.fsModifiers, hki.vk)
        End Sub

        Public Shared Sub ClearAllHotkey()
            If _hotkeyInfos.Count > 0 Then
                For Each hki As HotkeyInfo In _hotkeyInfos
                    prUnregisterHotKey(hki.hwnd, hki.id)
                Next
                _hotkeyInfos.Clear()
            End If
        End Sub

        Public Shared Sub SetEnabled(be As Boolean)
            For Each hki As HotkeyInfo In _hotkeyInfos
                If be Then
                    prRegisterHotHey(hki.hwnd, hki.id, hki.fsModifiers, hki.vk)
                Else
                    prUnregisterHotKey(hki.hwnd, hki.id)
                End If
            Next
        End Sub


        Public Shared Function CheckWndProc(frm As Form, ByRef m As Message) As Boolean
            Dim br As Boolean = False

            If m.Msg = _WM_HOTKEY Then
                ' 메인폼이 활성화 중이면
                If Form.ActiveForm Is frm Then
                    Dim wp As Integer = m.WParam.ToInt32()
                    For Each hki As HotkeyInfo In _hotkeyInfos
                        If hki.id = wp Then
                            hki.cbf()
                            br = True

                            Exit For
                        End If
                    Next
                End If
            End If

            Return br
        End Function

    End Class
End Namespace
