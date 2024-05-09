﻿Imports System.Windows.Forms



Public NotInheritable Class MainProxy
    Private Sub New()
    End Sub


    ''' <summary>
    ''' 타이틀 문자열 가져오기
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetTitleText(Optional ics As String = Nothing) As String
        Dim dvi As String = "WebView2Viewer__vb"
        Dim vnb As String = "v1.5.6"
        If Not String.IsNullOrWhiteSpace(ics) Then
            Return $"[ {dvi},  {vnb} ]   ""{ics}"""
        Else
            Return $"[ {dvi},  {vnb} ]"
        End If
    End Function


    Public Shared _mainForm As MainForm
    ''' <summary>
    ''' 메인폼 참조하기
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property MainForm As MainForm
        Get
            If _mainForm Is Nothing Then
                _mainForm = TryCast(Application.OpenForms("MainForm"), MainForm)
            End If
            Return _mainForm
        End Get
    End Property
End Class

