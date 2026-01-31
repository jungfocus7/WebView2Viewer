Imports System.Diagnostics
Imports System.Windows.Forms


Namespace Helpers
    Public NotInheritable Class DebugHelper
        Private Sub New()
        End Sub


        ''' <summary>
        ''' 디버그 모드 여부
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property IsDebugMode As Boolean
            Get
#If DEBUG Then
                Return True
#Else
                Return False
#End If
            End Get
        End Property


        'Public Shared IsUseAlert As Boolean = False
        '''' <summary>
        '''' 메세지 출력
        '''' </summary>
        'Public Shared Sub Alert(msg As String)
        '    If IsUseAlert Then
        '        MsgBox(msg)
        '        'AlertForm.Open(MainForm, msg)
        '    End If
        'End Sub


        Private Const _frb As String = "[hb] "
        ''' <summary>
        ''' 로그 출력
        ''' </summary>
        Public Shared Sub Log(msg As String)
            Dim txt As String = _frb & msg
            Debug.WriteLine(txt)
            'Trace.WriteLine(txt)
        End Sub
    End Class
End Namespace
