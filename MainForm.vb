Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class MainForm

    Dim _client As TcpClient


    Private Sub Connect_Click(sender As Object, e As EventArgs) Handles Connect.Click
        Try
            Dim ip As String = "127.0.0.1"
            Dim port As Integer = 5432

            _client = New TcpClient(ip, port)

            CheckForIllegalCrossThreadCalls = False


            Threading.ThreadPool.QueueUserWorkItem(AddressOf ReceiveMessages)

            Me.AcceptButton = Send

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ReceiveMessages(state As Object)
        Try

            While True

                Dim ns As NetworkStream = _client.GetStream()
                Dim toReceive(10000000) As Byte

                ns.Read(toReceive, 0, toReceive.Length)
                Dim txt As String = Encoding.ASCII.GetString(toReceive)

                If RichTextBox1.TextLength > 0 Then
                    RichTextBox1.Text &= vbCrLf & txt

                Else
                    RichTextBox1.Text = txt
                End If

            End While

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Send_Click(sender As Object, e As EventArgs) Handles Send.Click
        Try
            Dim ns As NetworkStream = _client.GetStream()

            ns.Write(Encoding.ASCII.GetBytes(TextBox1.Text), 0, TextBox1.Text.Length)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class
