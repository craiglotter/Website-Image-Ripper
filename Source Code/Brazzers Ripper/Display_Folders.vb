Imports System.IO

Public Class Display_Folders


    Public downloadfolder As String = ""
    Private row As Integer = 1
    Private column As Integer = 1



    Private Sub Error_Handler(ByVal ex As Exception, Optional ByVal identifier_msg As String = "")
        Try
            If ex.Message.IndexOf("Thread was being aborted") < 0 Then
                Dim Display_Message1 As New Display_Message()
                Display_Message1.Message_Textbox.Text = "The Application encountered the following problem: " & vbCrLf & identifier_msg & ": " & ex.ToString
                Display_Message1.Timer1.Interval = 1000
                Display_Message1.ShowDialog()
                Dim dir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs")
                If dir.Exists = False Then
                    dir.Create()
                End If
                dir = Nothing
                Dim filewriter As System.IO.StreamWriter = New System.IO.StreamWriter((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs\" & Format(Now(), "yyyyMMdd") & "_Error_Log.txt", True)
                filewriter.WriteLine("#" & Format(Now(), "dd/MM/yyyy hh:mm:ss tt") & " - " & identifier_msg & ": " & ex.ToString)
                filewriter.WriteLine("")
                filewriter.Flush()
                filewriter.Close()
                filewriter = Nothing
            End If
        Catch exc As Exception
            MsgBox("An error occurred in the application's error handling routine. The application will try to recover from this serious error." & vbCrLf & vbCrLf & exc.ToString, MsgBoxStyle.Critical, "Critical Error Encountered")
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        'BackgroundWorker1.RunWorkerAsync()
        Try
            row = 0
            column = 0
            Dim counter As Integer = 0
            Dim dinfo As DirectoryInfo = New DirectoryInfo(downloadfolder)
            If dinfo.Exists = True Then
                For Each dinfo2 As DirectoryInfo In dinfo.GetDirectories
                    Try

                        If My.Computer.FileSystem.FileExists(dinfo2.FullName & "\01.jpg") = True Then
                            'MsgBox(dinfo2.FullName & "\01.jpg")
                            Dim pbox As PictureBox = New PictureBox
                            pbox.BorderStyle = BorderStyle.FixedSingle
                            pbox.SizeMode = PictureBoxSizeMode.StretchImage
                            pbox.Height = PictureBox1.Height
                            pbox.Width = PictureBox1.Width
                            pbox.Image = Image.FromFile(dinfo2.FullName & "\01.jpg")
                            pbox.Tag = dinfo2.FullName & "\01.jpg"
                            pbox.Enabled = True
                            pbox.Visible = True
                            pbox.Top = 12 + (row * 86)
                            pbox.Left = 14 + (column * 59)
                            column = column + 1
                            If column > 5 Then
                                column = 0
                                row = row + 1
                            End If
                            AddHandler pbox.Click, AddressOf Me.PictureBox1_Click
                            counter = counter + 1
                            Panel1.Controls.Add(pbox)
                        End If

                    Catch ex As Exception
                        Error_Handler(ex, "Load Folders: " & dinfo2.FullName)
                    End Try
                Next
            End If
            Label2.Text = "(" & counter & " Folders)"
        Catch ex As Exception
            Error_Handler(ex, "Load Folders")
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            MsgBox("Here")
            row = 0
            column = 0
            Dim dinfo As DirectoryInfo = New DirectoryInfo(downloadfolder)
            If dinfo.Exists = True Then
                For Each dinfo2 As DirectoryInfo In dinfo.GetDirectories
                    Try

                        If My.Computer.FileSystem.FileExists(dinfo2.FullName & "\01.jpg") = True Then
                            MsgBox(dinfo2.FullName & "\01.jpg")
                            Dim pbox As PictureBox = New PictureBox
                            pbox.Height = PictureBox1.Height
                            pbox.Width = PictureBox1.Width
                            pbox.Image = Image.FromFile(dinfo2.FullName & "\01.jpg")
                            pbox.Enabled = True
                            pbox.Visible = True
                            pbox.Top = 12 + (row * 86)
                            pbox.Left = 14 + (column * 59)
                            row = row + 1
                            If row > 6 Then
                                row = 0
                                column = column + 1
                            End If
                            Panel1.Controls.Add(pbox)
                        End If

                    Catch ex As Exception
                        Error_Handler(ex, "Load Folders: " & dinfo2.FullName)
                    End Try
                Next
            End If

        Catch ex As Exception
            Error_Handler(ex, "Load Folders")
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Display_Folders_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Control.CheckForIllegalCrossThreadCalls = False
        Catch ex As Exception
            Error_Handler(ex, "Display Folders Load")
        End Try
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Try
            Dim pbox As PictureBox = sender
            If Not pbox.Image Is Nothing Then

                Process.Start("""" & pbox.Tag & """")
            End If
            pbox = Nothing
        Catch ex As Exception
            Error_Handler(ex, "Display Image")
        End Try
    End Sub
End Class