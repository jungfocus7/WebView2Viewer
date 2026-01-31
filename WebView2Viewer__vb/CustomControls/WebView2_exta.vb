Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms
Imports Newtonsoft.Json
Imports WebView2Viewer__vb.Helpers



Namespace CustomControls
    Public NotInheritable Class DummyData
        Public Type As String
        Public Dump As String
    End Class


    Public NotInheritable Class WebView2_exta : Inherits WebView2
        Private Shared _wv2ex As WebView2_exta
        Private Shared _cwv2 As CoreWebView2
        Private Shared _cbf As Action(Of String, String)

        Public Shared Sub ClearFrom(pnl As Panel)
            If _wv2ex Is Nothing Then
                Return
            Else
                pnl.SuspendLayout()

                _wv2ex.Stop()
                Try
                    _wv2ex.Source = New Uri("about:blank")
                Catch
                End Try
                _wv2ex = Nothing
                _cwv2 = Nothing
                _cbf = Nothing

                For Each ctr As Control In pnl.Controls
                    Try
                        ctr.Dispose()
                    Catch
                    End Try
                Next
                pnl.Controls.Clear()

                pnl.ResumeLayout(False)
                pnl.PerformLayout()

                MainProxy.GCCall()
            End If
        End Sub

        Private Shared Sub prCreateFrom(pnl As Panel)
            ClearFrom(pnl)

            _wv2ex = New WebView2_exta()
            CType(_wv2ex, ISupportInitialize).BeginInit()
            pnl.SuspendLayout()

            _wv2ex.AllowExternalDrop = True
            _wv2ex.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            _wv2ex.CreationProperties = Nothing
            _wv2ex.DefaultBackgroundColor = Color.White
            _wv2ex.Dock = DockStyle.Fill
            _wv2ex.Name = "WebView21"
            _wv2ex.TabIndex = 0
            _wv2ex.ZoomFactor = 1.0R
            pnl.Controls.Add(_wv2ex)

            CType(_wv2ex, ISupportInitialize).EndInit()
            pnl.ResumeLayout(False)
            pnl.PerformLayout()

            AddHandler _wv2ex.CoreWebView2InitializationCompleted, AddressOf prCoreWebView2InitializationCompleted
            prEnsureCoreWebView2Async()
        End Sub

        Private Shared Sub prCoreWebView2InitializationCompleted(sd As Object, ea As CoreWebView2InitializationCompletedEventArgs)
            If ea.IsSuccess Then
                _cwv2 = _wv2ex.CoreWebView2

                'Dim cwvpf As CoreWebView2Profile = _cwv2.Profile
                'cwvpf.PreferredColorScheme = CoreWebView2PreferredColorScheme.Dark
                'cwvpf.

                Dim cwvss As CoreWebView2Settings = _cwv2.Settings
                cwvss.IsPinchZoomEnabled = False
                cwvss.IsZoomControlEnabled = False

                Dim addScript As String = My.Resources.FromReady
                _cwv2.AddScriptToExecuteOnDocumentCreatedAsync(addScript)
                AddHandler _cwv2.DOMContentLoaded,
                    Sub()
                        Activated()
                    End Sub

                AddHandler _cwv2.WebMessageReceived, AddressOf prWebMessageReceived
                AddHandler _cwv2.ContextMenuRequested, AddressOf prContextMenuRequested
            End If
        End Sub

        Public Shared CbtFocus As String = "focus"
        Public Shared CbtClick As String = "click"
        Public Shared CbtGetHtmlText As String = "getHtmlText"
        Private Shared Sub prWebMessageReceived(sd As Object, ea As CoreWebView2WebMessageReceivedEventArgs)
            Dim wmsg As String = ea.WebMessageAsJson
            If Not String.IsNullOrWhiteSpace(wmsg) Then
                Try
                    Dim data As DummyData = JsonConvert.DeserializeObject(Of DummyData)(wmsg)
                    _cbf?.Invoke(data.Type, data.Dump)
                Catch
                End Try
            End If
        End Sub

        Private Shared Sub prContextMenuRequested(sd As Object, ea As CoreWebView2ContextMenuRequestedEventArgs)
            ea.Handled = True
        End Sub

        Private Shared Async Sub prEnsureCoreWebView2Async()
            Dim cweo As New CoreWebView2EnvironmentOptions("--disable-web-security")
            Dim env As CoreWebView2Environment = Await CoreWebView2Environment.CreateAsync(Nothing, Nothing, cweo)

            Await _wv2ex.EnsureCoreWebView2Async(env)
        End Sub


        Public Shared Sub OpenFrom(pnl As Panel, uri As Uri, Optional cbf As Action(Of String, String) = Nothing)
            prCreateFrom(pnl)
            _wv2ex.Source = uri
            _cbf = cbf
        End Sub

        Public Shared Sub OpenDevToolsWindow()
            _cwv2?.OpenDevToolsWindow()
        End Sub

        Public Shared Sub OpenTaskManagerWindow()
            _cwv2?.OpenTaskManagerWindow()
        End Sub

        Public Shared Sub ReloadFrom()
            _cwv2?.Reload()
        End Sub

        Public Shared Sub Activated()
            MainProxy.MainForm.Activate()
            MainProxy.MainForm.ActiveControl = _wv2ex
        End Sub

        'Public Shared Sub Deactivate()
        'End Sub

        Public Shared Async Sub GetHtmlText()
            If _wv2ex Is Nothing Then Return
            Await _wv2ex.ExecuteScriptAsync("fn_getHtmlText();")
        End Sub


        Private Sub New()
        End Sub






        'Protected Overrides Sub OnGotFocus(e As EventArgs)
        '    Debug.WriteLine("xxxxx-a")
        '    MyBase.OnGotFocus(e)
        'End Sub

        'Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        '    Debug.WriteLine("xxxxx-b")
        '    Return MyBase.ProcessCmdKey(msg, keyData)
        'End Function
    End Class
End Namespace
