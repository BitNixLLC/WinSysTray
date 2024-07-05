Imports System.Windows.Forms
Imports System.Management
Imports System.Net.Sockets
Imports System.Net
Imports System.Net.NetworkInformation
Public Class Form1
    Protected Overrides Sub SetVisibleCore(ByVal value As Boolean)
        If Not Me.IsHandleCreated Then
            Me.CreateHandle()
            value = False
        End If
        MyBase.SetVisibleCore(value)
    End Sub

    Public Function GetScreenWithMouse() As Screen
        Dim mousePosition As Point = Cursor.Position
        Return Screen.FromPoint(mousePosition)
    End Function
    Public Sub SetFormToBottomRight()
        Dim screen As Screen = GetScreenWithMouse()
        Dim screenBounds As Rectangle = screen.Bounds
        Dim taskBarHeight As Integer = screenBounds.Height - screen.WorkingArea.Height
        Dim formX As Integer = screenBounds.Right - Me.Width
        Dim formY As Integer = screenBounds.Bottom - Me.Height - taskBarHeight
        Me.Location = New Point(formX, formY)
    End Sub
    Private Sub GetExternalIP()
        Using client As New WebClient()
            Try
                Dim externalIP As String = client.DownloadString("https://whatip.xelara.network/").Trim()
                Label2.Text = externalIP
                Button1.Text = "Need Support?"
                Button1.Enabled = True
            Catch ex As Exception
                Label2.Text = "No Network"
                Button1.Text = "No Internet" & vbCrLf & "Please call (502) 206-1005"
                Button1.Enabled = False
            End Try
        End Using
    End Sub
    Private Sub GetLANIP()
        Dim host As String = Dns.GetHostName()
        Dim ip As String = Dns.GetHostEntry(host).AddressList.FirstOrDefault(Function(addr) addr.AddressFamily = AddressFamily.InterNetwork).ToString()
        Try
            Label3.Text = ip
        Catch ex As Exception
            Label3.Text = "No Network"
        End Try
    End Sub
    Private Sub GetMACAddress()
        Dim macAddress As String = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(Function(nic) nic.OperationalStatus = OperationalStatus.Up).GetPhysicalAddress().ToString()

        Try
            Label7.Text = macAddress
        Catch ex As Exception
            Label7.Text = "No Adapters"
        End Try
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        Me.Hide()
    End Sub
    Private Sub GetSerial()
        Try
            L2.Text = (New ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS").Get().Cast(Of ManagementObject)().FirstOrDefault()("SerialNumber").ToString())
        Catch ex As Exception
            L2.Text = "Unknown"
        End Try
        Try
            L3.Text = Environment.MachineName
        Catch ex As Exception
            L3.Text = "Unknown"
        End Try
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1.Enabled = False
        SetFormToBottomRight()
        MaximizeBox = False
        MinimizeBox = False
        GetSerial()
        GetLANIP()
        GetMACAddress()
        GetExternalIP()
        Timer1.Start()
    End Sub
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        SetFormToBottomRight()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Contact.Show()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Environment.Exit(0)
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Me.Show()
        SetFormToBottomRight()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        SetFormToBottomRight()
        GetSerial()
        GetLANIP()
        GetMACAddress()
        GetExternalIP()
    End Sub
End Class