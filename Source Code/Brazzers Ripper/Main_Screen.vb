Imports System.IO


Public Class Main_Screen

    Dim progresslabel As String = ""
    Dim shownminimizetip As Boolean = False


    Dim busyworking As Boolean = False
    Dim ripfoldername As String = ""

    Private AutoUpdate As Boolean = False

    Dim DownloadFolder As String = (Application.StartupPath & "\Rip").Replace("\\", "\")
    Dim DownloadMainPage As String = (Application.StartupPath & "\Rip\main.htm").Replace("\\", "\")
    Dim counter As Integer
    Dim downloadlist As ArrayList
    Dim idlist As ArrayList

    Dim lastpicturebox As String = "PictureBox1"



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
                Label2.Text = "Error encountered in last action"
            End If
        Catch exc As Exception
            MsgBox("An error occurred in the application's error handling routine. The application will try to recover from this serious error." & vbCrLf & vbCrLf & exc.ToString, MsgBoxStyle.Critical, "Critical Error Encountered")
        End Try
    End Sub

    Private Sub Activity_Handler(ByVal message As String)
        Try
            Dim dir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo((Application.StartupPath & "\").Replace("\\", "\") & "Activity Logs")
            If dir.Exists = False Then
                dir.Create()
            End If
            dir = Nothing
            Dim filewriter As System.IO.StreamWriter = New System.IO.StreamWriter((Application.StartupPath & "\").Replace("\\", "\") & "Activity Logs\" & Format(Now(), "yyyyMMdd") & "_Activity_Log.txt", True)
            filewriter.WriteLine("#" & Format(Now(), "dd/MM/yyyy hh:mm:ss tt") & " - " & message)
            filewriter.WriteLine("")
            filewriter.Flush()
            filewriter.Close()
            filewriter = Nothing
        Catch ex As Exception
            Error_Handler(ex, "Activity Handler")
        End Try
    End Sub


    Private Sub RunWorker()
        Try
            If busyworking = False Then
                busyworking = True
                Control_Enabler(False)
                Label2.Text = "Preparing to rip site..."
                progresslabel = ""
                lastpicturebox = "PictureBox1"
                If Not PictureBox1.Image Is Nothing Then
                    PictureBox1.Image = Nothing
                End If
                If Not PictureBox2.Image Is Nothing Then
                    PictureBox2.Image = Nothing
                End If
                If Not PictureBox3.Image Is Nothing Then
                    PictureBox3.Image = Nothing
                End If
                If Not PictureBox4.Image Is Nothing Then
                    PictureBox4.Image = Nothing
                End If
                If Not PictureBox5.Image Is Nothing Then
                    PictureBox5.Image = Nothing
                End If
                ToolTip1.SetToolTip(PictureBox1, "Last downloaded images")
                ToolTip1.SetToolTip(PictureBox2, "Last downloaded images")
                ToolTip1.SetToolTip(PictureBox3, "Last downloaded images")
                ToolTip1.SetToolTip(PictureBox4, "Last downloaded images")
                ToolTip1.SetToolTip(PictureBox5, "Last downloaded images")
                Label7.Text = "0"
                Label9.Text = "0"
                WebBrowser1.Navigate((Application.StartupPath & "\Images\Monitoring-Animation.htm").Replace("\\", "\"))
                BackgroundWorker1.RunWorkerAsync()
            End If
        Catch ex As Exception
            Error_Handler(ex, "Run Worker")
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            shownminimizetip = False
            Control.CheckForIllegalCrossThreadCalls = False
            Me.Text = My.Application.Info.ProductName & " (" & Format(My.Application.Info.Version.Major, "0000") & Format(My.Application.Info.Version.Minor, "00") & Format(My.Application.Info.Version.Build, "00") & "." & Format(My.Application.Info.Version.Revision, "00") & ")"
            WebBrowser1.Navigate((Application.StartupPath & "\Images\Monitoring-Still.htm").Replace("\\", "\"))
            ripfoldername = (Application.StartupPath & "\Rip").Replace("\\", "\")
            If My.Computer.FileSystem.DirectoryExists(ripfoldername) = False Then
                My.Computer.FileSystem.CreateDirectory(ripfoldername)
            End If
            loadSettings()
            AboutToolStripMenuItem1.Text = "About " & My.Application.Info.ProductName
            Label2.Text = "Application loaded"
            downloadlist = New ArrayList
            idlist = New ArrayList
            labelpicturebox1.Visible = False
            labelpicturebox2.Visible = False
            labelpicturebox3.Visible = False
            labelpicturebox4.Visible = False
            labelpicturebox5.Visible = False
        Catch ex As Exception
            Error_Handler(ex, "Form Load")
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            downloadlist.Clear()
            idlist.Clear()
            Dim WbReq As New Net.WebClient
            WbReq.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials
            If My.Computer.FileSystem.DirectoryExists(DownloadFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(DownloadFolder)
            End If
            If My.Computer.FileSystem.FileExists(DownloadMainPage) = True Then
                My.Computer.FileSystem.DeleteFile(DownloadMainPage)
            End If
            Try
                Label2.Text = "Downloading main page: " & vbCrLf & "http://www.brazzers.com/t1"
                ToolTip1.SetToolTip(Label2, "Downloading main page: " & vbCrLf & "http://www.brazzers.com/t1")
                WbReq.DownloadFile(New Uri("http://www.brazzers.com/t1"), DownloadMainPage)
            Catch ex As Exception
                Error_Handler(ex, "Downloading Main Page: http://www.brazzers.com/t1")
                If My.Computer.FileSystem.FileExists(DownloadMainPage) = True Then
                    My.Computer.FileSystem.DeleteFile(DownloadMainPage)
                End If
            End Try

            If Me.BackgroundWorker1.CancellationPending Then
                e.Cancel = True
                WbReq.Dispose()
                Exit Sub
            End If

            If My.Computer.FileSystem.FileExists(DownloadMainPage) = True Then
                Dim reader As StreamReader = New StreamReader(DownloadMainPage)
                Dim lineread As String = ""
                Dim url As String = ""
                counter = 0
                While reader.Peek <> -1
                    Label2.Text = "Parsing downloaded main page"
                    ToolTip1.SetToolTip(Label2, "Parsing downloaded main page")
                    If Me.BackgroundWorker1.CancellationPending Then
                        e.Cancel = True
                        reader.Close()
                        reader = Nothing
                        WbReq.Dispose()
                        Exit Sub
                    End If
                    lineread = reader.ReadLine
                    If lineread.ToLower.IndexOf("href=""preview.php?update_id=") <> -1 Then
                        Label2.Text = "Found preview page link"
                        ToolTip1.SetToolTip(Label2, "Found preview page link")
                        url = lineread.Remove(0, lineread.ToLower.IndexOf("href=""preview.php?update_id="))
                        url = url.Remove(0, 6)
                        url = url.Substring(0, url.IndexOf(""""))
                        Dim previewurl As Uri = New Uri("http://tour.brazzers.com/t1/" & url)
                        Dim checkindex As Integer = previewurl.ToString.IndexOf("&")
                        If checkindex = -1 Then
                            checkindex = 1
                        End If
                        If idlist.IndexOf(previewurl.ToString.Substring(0, checkindex)) = -1 Then
                            idlist.Add(previewurl.ToString.Substring(0, checkindex))
                            If downloadlist.IndexOf(previewurl.ToString) = -1 Then
                                downloadlist.Add(previewurl.ToString)
                                'counter = counter + 1
                                Dim counterurl As String = MakeFileNameFit(url)
                                If My.Computer.FileSystem.DirectoryExists((DownloadFolder & "\" & counterurl).Replace("\\", "\")) = False Then
                                    My.Computer.FileSystem.CreateDirectory((DownloadFolder & "\" & counterurl).Replace("\\", "\"))
                                End If
                                Label2.Text = "Downloading preview page: " & vbCrLf & previewurl.ToString
                                ToolTip1.SetToolTip(Label2, "Downloading preview page: " & vbCrLf & previewurl.ToString)
                                Try
                                    My.Computer.Network.DownloadFile(previewurl, (DownloadFolder & "\" & counterurl & "\preview.htm").Replace("\\", "\"), "", "", False, 100000, True)
                                    Label7.Text = Integer.Parse(Label7.Text) + 1
                                Catch ex As Exception
                                    Error_Handler(ex, "Downloading Preview Page: " & previewurl.ToString)
                                    If My.Computer.FileSystem.FileExists((DownloadFolder & "\" & counterurl & "\preview.htm").Replace("\\", "\")) = True Then
                                        My.Computer.FileSystem.DeleteFile((DownloadFolder & "\" & counterurl & "\preview.htm").Replace("\\", "\"))
                                    End If
                                End Try

                                If My.Computer.FileSystem.FileExists((DownloadFolder & "\" & counterurl & "\preview.htm").Replace("\\", "\")) = True Then
                                    Label2.Text = "Parsing downloaded preview page"
                                    ToolTip1.SetToolTip(Label2, "Parsing downloaded preview page")
                                    If Me.BackgroundWorker1.CancellationPending Then
                                        e.Cancel = True
                                        reader.Close()
                                        reader = Nothing
                                        WbReq.Dispose()
                                        Exit Sub
                                    End If
                                    Dim reader2 As StreamReader = New StreamReader((DownloadFolder & "\" & counterurl & "\preview.htm").Replace("\\", "\"))
                                    Dim lineread2 As String = ""
                                    While reader2.Peek <> -1
                                        Label2.Text = "Parsing downloaded preview page"
                                        ToolTip1.SetToolTip(Label2, "Parsing downloaded preview page")
                                        If Me.BackgroundWorker1.CancellationPending Then
                                            e.Cancel = True
                                            reader2.Close()
                                            reader2 = Nothing
                                            reader.Close()
                                            reader = Nothing
                                            WbReq.Dispose()
                                            Exit Sub
                                        End If
                                        lineread2 = reader2.ReadLine
                                        If lineread2.ToLower.IndexOf("href=""http://tour.brazzerspass.com/t1/pics/") <> -1 Then
                                            url = lineread2.Remove(0, lineread2.ToLower.IndexOf("href=""http://tour.brazzerspass.com/t1/pics/"))
                                            url = url.Remove(0, 6)
                                            url = url.Substring(0, url.IndexOf(""""))
                                            Dim basepicurl As String = url
                                            basepicurl = basepicurl.Remove(url.Length - 6, 6)
                                            Dim counter2 As Integer
                                            For counter2 = 1 To 100
                                                If Me.BackgroundWorker1.CancellationPending Then
                                                    e.Cancel = True
                                                    reader2.Close()
                                                    reader2 = Nothing
                                                    reader.Close()
                                                    reader = Nothing
                                                    WbReq.Dispose()
                                                    Exit Sub
                                                End If
                                                Dim counter2str As String
                                                counter2str = counter2.ToString
                                                While counter2str.Length < 2
                                                    counter2str = "0" & counter2str
                                                End While
                                                Label2.Text = "Downloading image file: " & vbCrLf & basepicurl & counter2str & ".jpg"
                                                ToolTip1.SetToolTip(Label2, "Downloading image file: " & vbCrLf & basepicurl & counter2str & ".jpg")
                                                Dim img As Image
                                                Try
                                                    If (My.Computer.FileSystem.FileExists((DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")) = False) Or ((My.Computer.FileSystem.FileExists((DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")) = True And CheckBox1.Checked = True)) Then
                                                        My.Computer.Network.DownloadFile(basepicurl & counter2str & ".jpg", (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"), "", "", False, 100000, True)
                                                    End If
                                                    Try
                                                        img = Image.FromFile((DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                        Label9.Text = Integer.Parse(Label9.Text) + 1
                                                        Select Case lastpicturebox
                                                            Case "PictureBox1"
                                                                If Not PictureBox1.Image Is Nothing Then
                                                                    PictureBox1.Image = Nothing
                                                                End If
                                                                PictureBox1.Image = img
                                                                PictureBox1.Tag = (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")
                                                                ToolTip1.SetToolTip(PictureBox1, (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                                labelpicturebox1.Visible = True
                                                                labelpicturebox5.Visible = False
                                                                lastpicturebox = "PictureBox2"
                                                            Case "PictureBox2"
                                                                If Not PictureBox2.Image Is Nothing Then
                                                                    PictureBox2.Image = Nothing
                                                                End If
                                                                PictureBox2.Image = img
                                                                PictureBox2.Tag = (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")
                                                                ToolTip1.SetToolTip(PictureBox2, (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                                labelpicturebox2.Visible = True
                                                                labelpicturebox1.Visible = False
                                                                lastpicturebox = "PictureBox3"
                                                            Case "PictureBox3"
                                                                If Not PictureBox3.Image Is Nothing Then
                                                                    PictureBox3.Image = Nothing
                                                                End If
                                                                PictureBox3.Image = img
                                                                PictureBox3.Tag = (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")
                                                                ToolTip1.SetToolTip(PictureBox3, (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                                labelpicturebox3.Visible = True
                                                                labelpicturebox2.Visible = False
                                                                lastpicturebox = "PictureBox4"
                                                            Case "PictureBox4"
                                                                If Not PictureBox4.Image Is Nothing Then
                                                                    PictureBox4.Image = Nothing
                                                                End If
                                                                PictureBox4.Image = img
                                                                PictureBox4.Tag = (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")
                                                                ToolTip1.SetToolTip(PictureBox4, (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                                labelpicturebox4.Visible = True
                                                                labelpicturebox3.Visible = False
                                                                lastpicturebox = "PictureBox5"
                                                            Case "PictureBox5"
                                                                If Not PictureBox5.Image Is Nothing Then
                                                                    PictureBox5.Image = Nothing
                                                                End If
                                                                PictureBox5.Image = img
                                                                PictureBox5.Tag = (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")
                                                                ToolTip1.SetToolTip(PictureBox5, (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                                labelpicturebox5.Visible = True
                                                                labelpicturebox4.Visible = False
                                                                lastpicturebox = "PictureBox1"
                                                            Case Else
                                                                If Not PictureBox1.Image Is Nothing Then
                                                                    PictureBox1.Image = Nothing
                                                                End If
                                                                PictureBox1.Image = img
                                                                PictureBox1.Tag = (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")
                                                                ToolTip1.SetToolTip(PictureBox1, (DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                                labelpicturebox1.Visible = True
                                                                labelpicturebox5.Visible = False
                                                                lastpicturebox = "PictureBox2"
                                                        End Select
                                                    Catch ex As Exception
                                                        img = Nothing
                                                        If My.Computer.FileSystem.FileExists((DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\")) = True Then
                                                            My.Computer.FileSystem.DeleteFile((DownloadFolder & "\" & counterurl & "\" & counter2str & ".jpg").Replace("\\", "\"))
                                                        End If
                                                        Exit For
                                                    End Try
                                                Catch ex As Exception
                                                    Error_Handler(ex, "Downloading Image File")
                                                End Try
                                            Next
                                            Exit While
                                        End If
                                    End While
                                    reader2.Close()
                                    reader2 = Nothing
                                End If
                            End If
                        End If
                    End If
                    If Me.BackgroundWorker1.CancellationPending Then
                        e.Cancel = True
                        reader.Close()
                        reader = Nothing
                        WbReq.Dispose()
                        Exit Sub
                    End If
                End While
                reader.Close()
                reader = Nothing
                My.Computer.FileSystem.DeleteFile(DownloadMainPage)
                progresslabel = "Site successfully ripped"
            Else
                progresslabel = "The site rip was unsuccessful:" & vbCrLf & "The main page could not be downloaded"
            End If
            WbReq.Dispose()

        Catch ex As Exception
            Error_Handler(ex, "Background Worker: " & progresslabel)
            progresslabel = "Operation Failed: Error reported (" & progresslabel & ")"
        End Try
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Try
            Label2.Text = progresslabel
        Catch ex As Exception
            Error_Handler(ex, "Worker Progress Changed")
        End Try
    End Sub

    Private Sub Control_Enabler(ByVal IsEnabled As Boolean)
        Try
            Select Case IsEnabled
                Case True
                    Button2.Enabled = True
                    Button1.Enabled = False
                    MenuStrip1.Enabled = True
                    Me.ControlBox = True
                    Button1.Visible = False
                    Label3.Visible = False
                Case False
                    Button2.Enabled = False
                    Button1.Enabled = True
                    MenuStrip1.Enabled = False
                    Me.ControlBox = False
                    Button1.Visible = True
                    Label3.Visible = True
            End Select
        Catch ex As Exception
            Error_Handler(ex, "Control Enabler")
        End Try
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            Control_Enabler(True)
            If e.Error Is Nothing Then
                If e.Cancelled = True Then
                    Label2.Text = "Ripping of the site was cancelled"
                Else
                    Label2.Text = progresslabel
                End If
            Else
                Label2.Text = "There was an error in ripping the site"
            End If
            ToolTip1.SetToolTip(Label2, "")
            WebBrowser1.Navigate((Application.StartupPath & "\Images\Monitoring-Still.htm").Replace("\\", "\"))
        Catch ex As Exception
            Error_Handler(ex, "Run Worker Completed")
        End Try
        busyworking = False
    End Sub

    

    Private Sub HelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem.Click
        Try
            Label2.Text = "Help displayed"
            HelpBox1.ShowDialog()
        Catch ex As Exception
            Error_Handler(ex, "Display Help Screen")
        End Try
    End Sub



    Private Sub loadSettings()
        Try
            Label2.Text = "Loading application settings..."
            Dim configfile As String = (Application.StartupPath & "\config.sav").Replace("\\", "\")
            If My.Computer.FileSystem.FileExists(configfile) Then
                Dim reader As StreamReader = New StreamReader(configfile)
                Dim lineread As String
                Dim variablevalue As String
                While reader.Peek <> -1
                    lineread = reader.ReadLine
                    If lineread.IndexOf("=") <> -1 Then
                        variablevalue = lineread.Remove(0, lineread.IndexOf("=") + 1)
                        If lineread.StartsWith("checkbox1=") Then
                            If variablevalue = "True" Then
                                CheckBox1.Checked = True
                            Else
                                CheckBox1.Checked = False
                            End If
                        End If
                    End If
                End While
                reader.Close()
                reader = Nothing
            End If
            Label2.Text = "Application Settings successfully loaded"
        Catch ex As Exception
            Error_Handler(ex, "Load Settings")
        End Try
    End Sub


    Private Sub SaveSettings()
        Try
            Label2.Text = "Saving application settings..."
            Dim configfile As String = (Application.StartupPath & "\config.sav").Replace("\\", "\")
            Dim writer As StreamWriter = New StreamWriter(configfile, False)
            writer.WriteLine("checkbox1=" & CheckBox1.Checked)
            writer.Flush()
            writer.Close()
            writer = Nothing
            Label2.Text = "Application Settings successfully saved"
        Catch ex As Exception
            Error_Handler(ex, "Save Settings")
        End Try
    End Sub


    Private Sub Main_Screen_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            SaveSettings()
            If AutoUpdate = True Then
                If My.Computer.FileSystem.FileExists((Application.StartupPath & "\AutoUpdate.exe").Replace("\\", "\")) = True Then
                    Dim startinfo As ProcessStartInfo = New ProcessStartInfo
                    startinfo.FileName = (Application.StartupPath & "\AutoUpdate.exe").Replace("\\", "\")
                    startinfo.Arguments = "force"
                    startinfo.CreateNoWindow = False
                    Process.Start(startinfo)
                End If
            End If
        Catch ex As Exception
            Error_Handler(ex, "Closing Application")
        End Try
    End Sub



    Private Sub NotifyIcon1_BalloonTipClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyIcon1.BalloonTipClicked
        Try
            Me.WindowState = FormWindowState.Normal
            Me.ShowInTaskbar = True
            NotifyIcon1.Visible = False
            Me.Refresh()
        Catch ex As Exception
            Error_Handler(ex, "Click on NotifyIcon")
        End Try
    End Sub


    Private Sub NotifyIcon1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        Try
            Me.WindowState = FormWindowState.Normal
            Me.ShowInTaskbar = True
            NotifyIcon1.Visible = False
            Me.Refresh()
        Catch ex As Exception
            Error_Handler(ex, "Click on NotifyIcon")
        End Try
    End Sub


    Private Sub NotifyIcon1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyIcon1.Click
        Try
            Me.WindowState = FormWindowState.Normal
            Me.ShowInTaskbar = True
            NotifyIcon1.Visible = False
            Me.Refresh()
        Catch ex As Exception
            Error_Handler(ex, "Click on NotifyIcon")
        End Try
    End Sub

    Private Sub Main_Screen_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Try
            If Me.WindowState = FormWindowState.Minimized Then
                Me.ShowInTaskbar = False
                NotifyIcon1.Visible = True
                If shownminimizetip = False Then
                    NotifyIcon1.ShowBalloonTip(1)
                    shownminimizetip = True
                End If
            End If
        Catch ex As Exception
            Error_Handler(ex, "Change Window State")
        End Try
    End Sub



    Private Sub AutoUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoUpdateToolStripMenuItem.Click
        Try
            AutoUpdate = True
            Me.Close()
        Catch ex As Exception
            Error_Handler(ex, "AutoUpdate")
        End Try
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        Try
            Label2.Text = "About displayed"
            AboutBox1.ShowDialog()
        Catch ex As Exception
            Error_Handler(ex, "Display About Screen")
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        RunWorker()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' Cancel the asynchronous operation.
        Me.BackgroundWorker1.CancelAsync()
        ' Disable the Cancel button.
        Button1.Enabled = False
        sender = Nothing
        e = Nothing
    End Sub

    Private Function MakeFileNameFit(ByVal url As String) As String
        Dim filename As String = url
        Try
            filename = filename.Replace("\", "")
            filename = filename.Replace("/", "")
            filename = filename.Replace(":", "")
            filename = filename.Replace("*", "")
            filename = filename.Replace("?", "")
            filename = filename.Replace("""", "")
            filename = filename.Replace("<", "")
            filename = filename.Replace(">", "")
            filename = filename.Replace("|", "")
        Catch ex As Exception
            Error_Handler(ex, "Make Filename Fit")
        End Try
        Return filename
    End Function

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        Try
            Me.WindowState = FormWindowState.Minimized
        Catch ex As Exception
            Error_Handler(ex, "Minimize Label Clicked")
        End Try
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click, PictureBox2.Click, PictureBox3.Click, PictureBox4.Click, PictureBox5.Click
        Try
            Dim pbox As PictureBox = sender
            If Not pbox.Image Is Nothing Then
                If (ToolTip1.GetToolTip(pbox)).Length > 0 Then

                    Process.Start("""" & ToolTip1.GetToolTip(pbox) & """")
                End If
            End If
            pbox = Nothing
        Catch ex As Exception
            Error_Handler(ex, "Display Image")
        End Try
    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click
        Try
            Dim df As Display_Folders = New Display_Folders
            df.downloadfolder = DownloadFolder
            df.Show()
            'BackgroundWorker2.RunWorkerAsync()
        Catch ex As Exception
            Error_Handler(ex, "Launch Downloaded Folders Viewer")
        End Try
    End Sub

    Private Sub BackgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Try
            Dim df As Display_Folders = New Display_Folders
            df.downloadfolder = DownloadFolder
            df.Show()
        Catch ex As Exception
            Error_Handler(ex, "Launch Downloaded Folders Viewer")
        End Try
    End Sub
End Class
