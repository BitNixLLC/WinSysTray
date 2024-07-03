Imports System.Net
Public Class Contact
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        Me.Hide()
        TextBox1.Text = ""
    End Sub
    Private Sub Contact_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim baseUrl As String = "https://exampleapi.com/"
        Dim token As String = "null"
        Try
            token = System.IO.File.ReadAllText("C:\ProgramData\TacticalRMM\token.txt").Replace(Environment.NewLine, "").Replace(vbLf, "").Replace(vbCr, "")
        Catch ex As Exception
            MessageBox.Show("Missing token...")
            TextBox1.Text = ""
            Me.Close()
        End Try
        Dim workstationName As String = Form1.L3.Text
        Dim workstationLan As String = Form1.Label3.Text
        Dim workstationMacaddr As String = Form1.Label7.Text
        Dim workstationWan As String = Form1.Label2.Text
        Dim serialNum As String = Form1.L2.Text
        Dim clientMessage As String = TextBox1.Text

        Dim url As String = $"{baseUrl}?token={WebUtility.UrlEncode(token)}&workstation_name={WebUtility.UrlEncode(workstationName)}&workstation_lan={WebUtility.UrlEncode(workstationLan)}&workstation_macaddr={WebUtility.UrlEncode(workstationMacaddr)}&workstation_wan={WebUtility.UrlEncode(workstationWan)}&serial_number={WebUtility.UrlEncode(serialNum)}&client_message={WebUtility.UrlEncode(clientMessage)}"

        Using client As New WebClient()
            Try
                Dim response As String = client.DownloadString(url)
                MessageBox.Show("Request sent successfully! A technician will reach out shortly.")
                TextBox1.Text = ""
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("Error sending request: " & ex.Message)
                Me.Close()
            End Try
        End Using
    End Sub


End Class