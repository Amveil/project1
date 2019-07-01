Public Class Form1
    ' Ввод переменных
    Dim Vspeed As Single = -3
    Dim Hspeed As Single = -3
    Dim Rows As Integer = 8
    Dim Cols As Integer = 10
    Dim TopRow As Single = 0.1
    Dim RowHeight As Single = 0.05
    Dim Score As Integer = 0
    Dim SizeMode As String = "small"
    Dim r As Rectangle = ClientRectangle
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'проверка границ
        If ball.Top < 0 Then
            Vspeed = -Vspeed
        End If
        If ball.Left < 0 Then
            Hspeed = -Hspeed
        End If
        If ball.Bottom > ClientRectangle.Height Then ' условие при котором происходит поражение
            Timer1.Enabled = False
            Label1.Text = "Game over"
            Score = 0

        End If


        Dim C As Single = ball.Left + ball.Width / 2
        If C > paddle.Left And C < paddle.Right And Vspeed > 0 And
            ball.Bottom > paddle.Top And ball.Top < paddle.Top Then
            Vspeed = -Vspeed
            Dim Offset As Single = (ball.Left + ball.Width / 2) / (paddle.Left + paddle.Width)
            Dim Ratio As Single = Offset / (paddle.Width / 2)
            Hspeed += 2 * Ratio

        End If
        If ball.Right > ClientRectangle.Width Then
            Hspeed = -Hspeed
        End If
        For Each Cnt As Control In Me.Controls
            If Cnt.Name = "Кирпич" Then
                CheckBrick(Cnt, ball)
            End If
        Next
        ball.Left += Hspeed
        ball.Top += Vspeed
    End Sub
    Private Sub CheckBrick(ByVal Brick As Button, ByVal ball As Button)
        'проверка кирпич
        If Brick.Visible Then
            Dim Hit As Boolean = False
            Dim C As Single = ball.Left + ball.Width / 2

            If Vspeed < 0 And C > Brick.Left And C < Brick.Right And
                ball.Top < Brick.Bottom And ball.Bottom > Brick.Bottom Then
                Vspeed = -Vspeed
                Hit = True
            End If
            If Vspeed > 0 And C > Brick.Left And C < Brick.Right And
               ball.Bottom > Brick.Top And ball.Top < Brick.Top Then
                Vspeed = -Vspeed
                Hit = True
            End If
            C = ball.Top + ball.Height / 2

            If Hspeed > 0 And C > Brick.Top And C < Brick.Bottom And
               ball.Right > Brick.Left And ball.Left < Brick.Right Then
                Hspeed = -Hspeed
                Hit = True
            End If

            If Hspeed < 0 And C > Brick.Top And C < Brick.Bottom And
               ball.Left < Brick.Right And ball.Right > Brick.Right Then
                Hspeed = -Hspeed
                Hit = True
            End If
            If Hit Then
                Brick.Visible = False
                Score += Brick.Tag
                If Score > 30 Then
                    If Vspeed < 0 Then Vspeed = -6
                    If Vspeed > 0 Then Vspeed = 6
                Else
                    If Vspeed < 0 Then Vspeed = -3
                    If Vspeed > 0 Then Vspeed = 3
                End If
                LabelMessage.Text = Score.ToString
            End If
        End If
        'вывод сообщения победы при наборе определенного количества очков
        Select Case SizeMode.ToLower
            Case "small"
                If LabelMessage.Text = "360" Then
                    Label1.Text = "Win"
                    Timer1.Enabled = False
                End If
            Case "medium"
                If LabelMessage.Text = "660" Then
                    Label1.Text = "Win"
                    Timer1.Enabled = False
                End If
            Case "large"
                If LabelMessage.Text = "1092" Then
                    Label1.Text = "Win"
                    Timer1.Enabled = False
                End If
        End Select
    End Sub
    Private Sub MakeBricks()

        'удаление старых блоков
        For i As Integer = Me.Controls.Count - 1 To 0 Step -1
            If Me.Controls(i).Name = "Кирпич" Then
                Me.Controls.RemoveAt(i)
            End If
        Next

        Select Case SizeMode.ToLower
             'выбор уровня сложности
            Case "small" 'Легкий
                Me.Width = 800
                Me.Height = 700
                Rows = 8
                Cols = 10
            Case "medium" ' Средний
                Me.Width = 900
                Me.Height = 800
                Rows = 10
                Cols = 12
            Case "large" ' Сложный
                Me.Width = 1000
                Me.Height = 900
                Rows = 12
                Cols = 14
        End Select

        'генерация блоков
        For R As Integer = 0 To Rows - 1
            For C As Integer = 0 To Cols - 1
                Dim B As New Button
                Me.Controls.Add(B)
                B.Visible = True
                B.Name = "Кирпич"
                B.Tag = Rows - R
                B.Width = ClientRectangle.Width / Cols
                B.Height = ClientRectangle.Height * RowHeight
                B.Left = C * B.Width
                B.Top = ClientRectangle.Height * (TopRow + R * RowHeight)
                B.BackColor = Color.Red
                If (R + C) Mod 2 = 0 Then
                    B.BackColor = Color.Red
                Else
                    B.BackColor = Color.Blue
                End If
                B.FlatStyle = FlatStyle.Flat
            Next
        Next

        With ball
            .Left = ClientRectangle.Width / 2
            .Top = ClientRectangle.Height * 0.9
            Vspeed = -3
            Hspeed = 1
        End With

        Timer1.Enabled = False
    End Sub
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        MakeBricks()
    End Sub
    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove
        'движение платформы
        paddle.Width = 0.3 * Me.ClientRectangle.Width
        paddle.Height = 0.05 * Me.ClientRectangle.Height
        paddle.Top = 0.95 * Me.ClientRectangle.Height
        paddle.Left = e.X - (0.15 * Me.ClientRectangle.Width)
    End Sub

    Private Sub РестартToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ReStartToolStripMenuItem.Click

        MakeBricks()
        LabelMessage.Text = "0"  ' обнуление результатов при рестарте
        If Label1.Text = "Game over" Then
            Label1.Text = ""
            LabelMessage.Text = "0"
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub Start_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Start.Click
        Timer1.Enabled = True 'запуск игры
        LabelMessage.Text = "0"  ' обнуление результатов при рестарте
        If Label1.Text = "Game over" Then
            Label1.Text = ""
            LabelMessage.Text = "0"
        End If
    End Sub

    Private Sub smallStripMenuItem1_Click(sender As Object, e As EventArgs) Handles smallStripMenuItem1.Click, mediumStripMenuItem2.Click, largeStripMenuItem3.Click

        SizeMode = sender.text ' выбор уровня сложности
        MakeBricks()
        Score = 0
        If Label1.Text = "Game over" Then
            Label1.Text = ""
            LabelMessage.Text = "0"
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Dim result As DialogResult
        result = MessageBox.Show("Закрыть приложние", "Закрытие приложения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
        If result = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub
End Class
