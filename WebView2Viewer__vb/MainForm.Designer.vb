<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기에서는 수정하지 마세요.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.m_txbInput = New System.Windows.Forms.TextBox()
        Me.m_panelRoot = New System.Windows.Forms.Panel()
        Me.m_btnFunction = New WebView2Viewer__vb.CustomControls.ButtnEx71()
        Me.SuspendLayout()
        '
        'm_txbInput
        '
        Me.m_txbInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.m_txbInput.BackColor = System.Drawing.SystemColors.Window
        Me.m_txbInput.Enabled = False
        Me.m_txbInput.ForeColor = System.Drawing.SystemColors.ScrollBar
        Me.m_txbInput.Location = New System.Drawing.Point(6, 561)
        Me.m_txbInput.Margin = New System.Windows.Forms.Padding(0)
        Me.m_txbInput.Name = "m_txbInput"
        Me.m_txbInput.ReadOnly = True
        Me.m_txbInput.Size = New System.Drawing.Size(666, 23)
        Me.m_txbInput.TabIndex = 2
        '
        'm_panelRoot
        '
        Me.m_panelRoot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.m_panelRoot.BackColor = System.Drawing.Color.LightGray
        Me.m_panelRoot.Location = New System.Drawing.Point(0, 0)
        Me.m_panelRoot.Margin = New System.Windows.Forms.Padding(0)
        Me.m_panelRoot.Name = "m_panelRoot"
        Me.m_panelRoot.Size = New System.Drawing.Size(800, 546)
        Me.m_panelRoot.TabIndex = 4
        '
        'm_btnFunction
        '
        Me.m_btnFunction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.m_btnFunction.BackColor = System.Drawing.Color.DarkMagenta
        Me.m_btnFunction.Cursor = System.Windows.Forms.Cursors.Hand
        Me.m_btnFunction.Font = New System.Drawing.Font("맑은 고딕", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.m_btnFunction.ForeColor = System.Drawing.SystemColors.Control
        Me.m_btnFunction.Location = New System.Drawing.Point(676, 551)
        Me.m_btnFunction.Margin = New System.Windows.Forms.Padding(0)
        Me.m_btnFunction.Name = "m_btnFunction"
        Me.m_btnFunction.Size = New System.Drawing.Size(120, 40)
        Me.m_btnFunction.TabIndex = 1
        Me.m_btnFunction.Text = "GO"
        Me.m_btnFunction.UseVisualStyleBackColor = False
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.m_panelRoot)
        Me.Controls.Add(Me.m_txbInput)
        Me.Controls.Add(Me.m_btnFunction)
        Me.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(100, 40)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "MainForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents m_btnFunction As CustomControls.ButtnEx71
    Friend WithEvents m_txbInput As System.Windows.Forms.TextBox
    Friend WithEvents m_panelRoot As System.Windows.Forms.Panel
End Class
