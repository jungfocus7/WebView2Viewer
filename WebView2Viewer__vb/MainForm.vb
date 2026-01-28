Imports System
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports WebView2Viewer__vb.CustomControls
Imports WebView2Viewer__vb.Extensions
Imports WebView2Viewer__vb.Helpers
Imports WebView2Viewer__vb.Models
Imports WebView2Viewer__vb.PopupForms
Imports WV2VCSC.Helpers



Public NotInheritable Class MainForm
#Region "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 1) 기본설정"
    ''' <summary>
    ''' 생성자
    ''' </summary>
    Public Sub New()
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        prAfterInit()
    End Sub


    ''' <summary>
    ''' Load 이벤트
    ''' </summary>
    ''' <param name="ea"></param>
    Protected Overrides Sub OnLoad(ea As EventArgs)
        MyBase.OnLoad(ea)

        Text = MainProxy.GetTitleText()
        MinimumSize = Size - New Size(300, 300)
        AlignBottomRight()
        AddResizeRenderCancel()

        _cdp = Environment.GetCommandLineArgs()(0)
        _cdp = Path.GetDirectoryName(_cdp)

        prFooterSetting()

        prOpenFileFromCmdArgs()
    End Sub


    ''' <summary>
    ''' CurrentDirectoryPath
    ''' </summary>
    Private _cdp As String

    ''' <summary>
    ''' OpenFileDialog
    ''' </summary>
    Private _ofd_pr As OpenFileDialog
    Private ReadOnly Property _ofd As OpenFileDialog
        Get
            If _ofd_pr Is Nothing Then
                _ofd_pr = New OpenFileDialog With {
                    .InitialDirectory = _cdp,
                    .Filter = "html files (*.html)|*.html",
                    .RestoreDirectory = True,
                    .Multiselect = False
                }
            End If
            Return _ofd_pr
        End Get
    End Property

    ''' <summary>
    ''' InputSource
    ''' </summary>
    Private _ips As String

    ''' <summary>
    ''' HtmlDirectoryPath
    ''' </summary>
    Private _hdp As String


    ''' <summary>
    ''' 컨텍스트 메뉴
    ''' </summary>
    Private _cms As ContextMenuStrip



    ''' <summary>
    ''' 하단 세팅
    ''' </summary>
    Private Sub prFooterSetting()
        _cms = New ContextMenuStrip()
        Dim tsia() As ToolStripItem = {
            New ToolStripMenuItem("O) 파일 열기", Nothing, AddressOf prCmsAllClick),
            New ToolStripMenuItem("P) 클립보드 열기", Nothing, AddressOf prCmsAllClick),
            New ToolStripMenuItem("C) 뷰 닫기", Nothing, AddressOf prCmsAllClick),
            New ToolStripSeparator(),
            New ToolStripMenuItem("1) 폴더위치 열기", Nothing, AddressOf prCmsAllClick),
            New ToolStripMenuItem("2) VSCode 열기", Nothing, AddressOf prCmsAllClick),
            New ToolStripSeparator(),
            New ToolStripMenuItem("3) 개발자도구 열기", Nothing, AddressOf prCmsAllClick),
            New ToolStripMenuItem("4) 작업관리자 열기", Nothing, AddressOf prCmsAllClick),
            New ToolStripMenuItem("5) 새로 고침", Nothing, AddressOf prCmsAllClick),
            New ToolStripSeparator(),
            New ToolStripMenuItem("S) 이미지 캡처", Nothing, AddressOf prCmsAllClick),
            New ToolStripMenuItem("A) HTML 저장", Nothing, AddressOf prCmsAllClick),
            New ToolStripSeparator(),
            New ToolStripMenuItem("X) 메뉴 닫기", Nothing, AddressOf prCmsAllClick)
        }
        _cms.Cursor = Cursors.Hand
        _cms.Items.AddRange(tsia)

        AddHandler m_btnFunction.MouseDown, AddressOf prBtnFunctionMouseDown
        AddHandler m_btnFunction.Click, AddressOf prBtnFunctionClick

        m_txbInput.Clear()
    End Sub


    ''' <summary>
    ''' 컨텍스트 메뉴 모든 클릭
    ''' </summary>
    ''' <param name="sd"></param>
    ''' <param name="ea"></param>
    Private Sub prCmsAllClick(sd As Object, ea As EventArgs)
        Dim tsi As ToolStripItem = TryCast(sd, ToolStripItem)
        If tsi.Text.StartsWith("O) ") Then
            Try
                If (_ofd.ShowDialog(Me) = DialogResult.OK) Then
                    prOpenFile(_ofd.FileName)
                End If
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("P) ") Then
            Try
                prBtnFunctionClick(Nothing, Nothing)
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("C) ") Then
            Try
                WebView2_exta.ClearFrom(m_panelRoot)

                _ips = Nothing
                _hdp = Nothing

                Text = MainProxy.GetTitleText()
                m_txbInput.Clear()
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("1) ") Then
            Try
                If Directory.Exists(_hdp) Then
                    Process.Start(_hdp)
                Else
                    Process.Start(_cdp)
                End If
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("2) ") Then
            Try
                If Directory.Exists(_hdp) Then
                    Dim psi As New ProcessStartInfo() With {
                        .FileName = "code",
                        .WorkingDirectory = _hdp,
                        .Arguments = $"""{_hdp}""",
                        .UseShellExecute = True,
                        .CreateNoWindow = False,
                        .WindowStyle = ProcessWindowStyle.Hidden
                    }
                    Process.Start(psi)
                Else
                End If
            Catch
                DebugHelper.Log("실패")
            End Try
        ElseIf tsi.Text.StartsWith("3) ") Then
            Try
                WebView2_exta.OpenDevToolsWindow()
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("4) ") Then
            Try
                WebView2_exta.OpenTaskManagerWindow()
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("5) ") Then
            Try
                WebView2_exta.ReloadFrom()
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("S) ") Then
            Try
                'AlertForm.Open(Me, "준비중")
                MessageBoxHelper.Show(Me, "준비중")
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("A) ") Then
            Try
                WebView2_exta.GetHtmlText()
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("X) ") Then
            Try
                _cms.Close()
            Catch
            End Try
        ElseIf tsi.Text.StartsWith("프로그램 종료") Then
            Try
                Application.Exit()
            Catch
            End Try
        End If
    End Sub


    ''' <summary>
    ''' 에러 표시하기
    ''' </summary>
    ''' <param name="msg"></param>
    Private Sub prErrorDisplay(msg As String)
        If String.IsNullOrWhiteSpace(msg) Then
            m_txbInput.Clear()
        Else
            m_txbInput.Text = $"[#Error: {msg}]"
        End If
    End Sub


    ''' <summary>
    ''' 클립보드에 Uri열기
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub prBtnFunctionClick(sender As Object, e As EventArgs)
        If Clipboard.ContainsFileDropList() Then
            Try
                Dim lst As StringCollection = Clipboard.GetFileDropList()
                prOpenUrl(lst(0))
            Catch
            End Try
        ElseIf Clipboard.ContainsText() Then
            Try
                prOpenUrl(Clipboard.GetText())
            Catch
            End Try
        End If
    End Sub


    ''' <summary>
    ''' 버튼 오른쪽 클릭으로 컨텍스트메뉴 열기
    ''' </summary>
    ''' <param name="sd"></param>
    ''' <param name="ea"></param>
    Private Sub prBtnFunctionMouseDown(sd As Object, ea As MouseEventArgs)
        If ea.Button = MouseButtons.Right Then
            If _cms.Visible Then
                _cms.Close()
            Else
                Dim btnRct As Rectangle = RectangleToScreen(m_btnFunction.Bounds)
                Dim cmsRct As Rectangle = RectangleToScreen(_cms.Bounds)
                Dim pt As New Point(btnRct.Right - (cmsRct.Width + 4), btnRct.Top - (cmsRct.Height + 4))
                _cms.Show(pt, ToolStripDropDownDirection.Default)
                ActiveControl = m_btnFunction
            End If
        End If
    End Sub
#End Region



#Region "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 2) 기능성 코드"
    ''' <summary>
    ''' ???
    ''' </summary>
    ''' <param name="fp"></param>
    ''' <returns></returns>
    Private Function prCheckCritical(fp As String) As Boolean
        Dim excepts As String() = {".exe", ".dll", ".cmd", ".bat", ".lnk"}
        Dim ext As String = Path.GetExtension(fp)
        Return excepts.Contains(ext)
    End Function


    ''' <summary>
    ''' WebView2 연동콜백 (chrome.webview.postMessage) 
    ''' </summary>
    ''' <param name="type"></param>
    ''' <param name="dump"></param>
    Private Sub prWebView2BrowserCallback(type As String, dump As String)
        'prErrorDisplay($"{type}, {dump}")

        Select Case type
            Case WebView2_exta.CbtFocus
                _cms?.Close()

            Case WebView2_exta.CbtClick
                'AlertForm.Open(Me, dump)

            Case WebView2_exta.CbtGetHtmlText
                Dim htmlText As String = dump
                If Not String.IsNullOrWhiteSpace(htmlText) Then
                    'AlertForm.Open(Me, htmlText)
                    Try
                        DataHelper.SaveHtmlText(htmlText)
                        'AlertForm.Open(Me, "저장 성공")
                        MessageBoxHelper.Show(Me, "저장 성공")
                    Catch
                        'AlertForm.Open(Me, "저장 에러")
                        MessageBoxHelper.Show(Me, "저장 에러")
                    End Try

                    MainProxy.GCCall()
                End If

        End Select
    End Sub


    ''' <summary>
    ''' 파일 열기
    ''' </summary>
    ''' <param name="fp"></param>
    Private Sub prOpenFile(fp As String)
        If File.Exists(fp) Then
            If prCheckCritical(fp) Then
                prErrorDisplay("보안상 열수없는 파일임")
                Return
            End If

            Dim uri As New Uri(fp)
            WebView2_exta.OpenFrom(m_panelRoot, uri, AddressOf prWebView2BrowserCallback)

            _ips = fp
            _hdp = Path.GetDirectoryName(_ips)

            Text = MainProxy.GetTitleText(_ips)
            m_txbInput.Text = _ips
            _ofd.InitialDirectory = _hdp
        End If
    End Sub


    ''' <summary>
    ''' Url 열기
    ''' </summary>
    ''' <param name="url"></param>
    Private Sub prOpenUrl(url As String)
        Dim uri As New Uri(url)
        WebView2_exta.OpenFrom(m_panelRoot, uri, AddressOf prWebView2BrowserCallback)

        _ips = url
        _hdp = Nothing

        Text = MainProxy.GetTitleText(_ips)
        m_txbInput.Text = _ips
        '_ofd.InitialDirectory = _hdp
    End Sub


    ''' <summary>
    ''' 드래그 엔터
    ''' </summary>
    ''' <param name="ea"></param>
    Protected Overrides Sub OnDragEnter(ea As DragEventArgs)
        MyBase.OnDragEnter(ea)

        ea.Effect = DragDropEffects.All
    End Sub


    ''' <summary>
    ''' 드래그 드롭
    ''' </summary>
    ''' <param name="ea"></param>
    Protected Overrides Sub OnDragDrop(ea As DragEventArgs)
        MyBase.OnDragDrop(ea)

        Dim ido As IDataObject = ea.Data
        If ido.GetDataPresent(DataFormats.FileDrop) Then
            '#File Path
            Try
                Dim fpa As String() = TryCast(ido.GetData(DataFormats.FileDrop), String())
                prOpenFile(fpa(0))
            Catch
            End Try
        ElseIf ido.GetDataPresent(DataFormats.Text) Then
            '#Url String
            Try
                Dim url As String = TryCast(ido.GetData(DataFormats.Text), String)
                prOpenUrl(url)
            Catch
            End Try
        End If
    End Sub


    ''' <summary>
    ''' ???
    ''' </summary>
    Private Sub prOpenFileFromCmdArgs()
        Dim args As String() = Environment.GetCommandLineArgs()
        If (Not args Is Nothing) AndAlso (args.Length = 2) Then
            Try
                prOpenUrl(args(1))
            Catch
            End Try
        End If
    End Sub



    Protected Overrides Sub OnActivated(e As EventArgs)
        MyBase.OnActivated(e)
        'Debug.WriteLine(">>> OnActivated")
        HotkeyHelper.SetEnabled(True)
        'WebView2_exta.Activated()
    End Sub


    Protected Overrides Sub OnDeactivate(e As EventArgs)
        MyBase.OnDeactivate(e)
        'Debug.WriteLine(">>> OnDeactivate")
        HotkeyHelper.SetEnabled(False)
    End Sub


    Protected Overrides Sub WndProc(ByRef m As Message)
        If HotkeyHelper.CheckWndProc(Me, m) Then
            Return
        End If

        MyBase.WndProc(m)
    End Sub


    Private Sub prAfterInit()
        HotkeyHelper.AddHotkey(New HotkeyInfo() With {
            .hwnd = Handle,
            .fsModifiers = HotkeyHelper.Kmf_Alt,
            .vk = Keys.Apps,
            .cbf =
                Sub()
                    Dim ea As New MouseEventArgs(MouseButtons.Right, 0, 0, 0, 0)
                    prBtnFunctionMouseDown(Me, ea)
                End Sub
        })

        AddHandler FormClosing,
            Sub(sd As Object, ea As FormClosingEventArgs)
                HotkeyHelper.ClearAllHotkey()
            End Sub
    End Sub
#End Region










#Region "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 3) 단축키 코드"
    'Private Const _dllfpUser32 = "user32.dll"

    '<DllImport(_dllfpUser32, EntryPoint:="RegisterHotKey", CharSet:=CharSet.Auto, SetLastError:=True)>
    'Private Shared Function prRegisterHotHey(
    '    hwnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer) As Integer
    'End Function

    '<DllImport(_dllfpUser32, EntryPoint:="UnregisterHotKey", CharSet:=CharSet.Auto, SetLastError:=True)>
    'Private Shared Function prUnregisterHotKey(
    '    hwnd As IntPtr, id As Integer) As Integer
    'End Function


    'Private Const _HOTKEY_ID As Integer = 31197 + 30000

    'Private Enum _KeyModifiers
    '    None = 0
    '    Alt = 1
    '    Control = 2
    '    Shift = 4
    '    Windows = 8
    'End Enum

    'Private _hkidcnt As Integer = _HOTKEY_ID
    'Private _hotkeyInfos As New List(Of HotkeyInfo)

    'Private Sub prAddHotkey(hki As HotkeyInfo)
    '    hki.id = _hkidcnt
    '    _hkidcnt += 1
    '    _hotkeyInfos.Add(hki)
    '    prRegisterHotHey(hki.hwnd, hki.id, hki.fsModifiers, hki.vk)
    'End Sub

    'Private Sub prHotkeyAllClear()
    '    For Each hki As HotkeyInfo In _hotkeyInfos
    '        prUnregisterHotKey(hki.hwnd, hki.id)
    '    Next
    '    _hotkeyInfos.Clear()
    'End Sub


    'Private Const _WM_HOTKEY As Integer = &H312
    'Protected Overrides Sub WndProc(ByRef m As Message)
    '    MyBase.WndProc(m)

    '    Select Case m.Msg
    '        Case _WM_HOTKEY
    '            ' 메인폼이 활성화 중이면
    '            If ActiveForm Is Me Then
    '                For Each hki As HotkeyInfo In _hotkeyInfos
    '                    If m.WParam = CType(hki.id, IntPtr) Then
    '                        hki.cbf()
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '    End Select
    'End Sub



    ''### 레지스트 핫키를 사용하면 다른프로그램에서 키가 십힌다 문제해결은 나중에...
    'Private Sub prAfterInit()
    '    prAddHotkey(New HotkeyInfo() With {
    '        .hwnd = Handle,
    '        .fsModifiers = _KeyModifiers.None,
    '        .vk = Keys.Apps,
    '        .cbf = Sub()
    '                   'MessageBox.Show("ㅋㅋㅋ 개발자")
    '                   Dim btnRct As Rectangle = RectangleToScreen(m_btnFunction.Bounds)
    '                   Dim cmsRct As Rectangle = RectangleToScreen(_cms.Bounds)
    '                   Dim pt As New Point(btnRct.Right - (cmsRct.Width + 4), btnRct.Top - (cmsRct.Height + 4))
    '                   _cms.Show(pt, ToolStripDropDownDirection.Default)
    '                   ActiveControl = m_btnFunction
    '               End Sub
    '    })

    '    AddHandler FormClosing,
    '        Sub(sd As Object, ea As FormClosingEventArgs)
    '            prHotkeyAllClear()
    '        End Sub

    '    KeyPreview = True
    'End Sub
#End Region


#Region "######################################################### 코드 백업"
    'Private Const WM_NCHITTEST As Integer = 132
    'Private Const HTCLIENT As Integer = 1
    'Private Const HTCAPTION As Integer = 2
    'Protected Overrides Sub WndProc(ByRef m As Message)
    '    MyBase.WndProc(m)

    '    Select Case m.Msg
    '        Case WM_NCHITTEST
    '            If m.Result.ToInt32() = HTCLIENT Then
    '                m.Result = New IntPtr(HTCAPTION)
    '            End If
    '    End Select
    'End Sub


    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
    '    prErrorDisplay("ㅋㅋㅋㅋ >>> " & keyData)
    '    Return MyBase.ProcessCmdKey(msg, keyData)
    'End Function

    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
    '    prErrorDisplay("ㅋㅋㅋㅋ")
    '    Return MyBase.ProcessCmdKey(msg, keyData)
    'End Function
    'Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
    '    prErrorDisplay("ㅋㅋㅋㅋ")
    '    MyBase.OnKeyDown(e)
    'End Sub


    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
    '    Return MyBase.ProcessCmdKey(msg, keyData)
    'End Function

    'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
    '    Dim key As Keys = keyData And Not (Keys.Shift Or Keys.Control)
    '    Select Case key
    '        Case Keys.F
    '            If (keyData Or Keys.Control) <> 0 Then
    '                MsgBox("Ctrl+F")
    '            End If
    '    End Select

    '    Return True
    'End Function

    '<DllImport("user32.dll", EntryPoint:="RegisterHotKey", CharSet:=CharSet.Auto, SetLastError:=True)>
    'Private Shared Function prRegisterHotHey(hwnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer) As Integer
    'End Function

    '<DllImport("user32.dll", EntryPoint:="UnregisterHotKey", CharSet:=CharSet.Auto, SetLastError:=True)>
    'Private Shared Function prUnregisterHotKey(hwnd As IntPtr, id As Integer) As Integer
    'End Function

    'Protected Overrides Sub OnShown(e As EventArgs)
    '    MyBase.OnShown(e)
    '    Dim x1 = prRegisterHotHey(Handle, 990, &H0, Keys.Up)
    '    Dim x2 = prRegisterHotHey(Handle, 991, &H0, Keys.Down)
    'End Sub
    'Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
    '    MyBase.OnFormClosed(e)
    '    prUnregisterHotKey(Handle, 990)
    '    prUnregisterHotKey(Handle, 991)
    'End Sub



    'Protected Overrides Sub OnShown(e As EventArgs)
    '    MyBase.OnShown(e)
    '    '// 0x0 : 조합키 없이 사용, 0x1: ALT, 0x2: Ctrl, 0x3: Shift
    '    HotkeyTool.Register(Handle, 990, 2 Or 4, Keys.W)
    'End Sub
    'Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
    '    MyBase.OnFormClosed(e)
    '    HotkeyTool.Unregister(Handle, 990)
    'End Sub
    'Protected Overrides Sub WndProc(ByRef m As Message)
    '    MyBase.WndProc(m)
    '    If m.Msg = &H312 Then
    '        If m.WParam = New IntPtr(990) Then
    '            If ActiveForm Is Me Then
    '                Debug.WriteLine($">>> {ActiveForm?.ToString()}")
    '                MsgBox("Target#0")
    '            End If
    '        End If
    '    End If
    'End Sub
#End Region
End Class










