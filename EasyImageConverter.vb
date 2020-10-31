Imports System.Threading
Public Class EasyImageConverter
    Dim formats() As String = {"jpg", "png", "bmp", "jpeg", "jfif", "jpe", "rle", "dib", "svg", "svgz"}
    Private Sub EasyImageConverter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High
        Dim totalString As String = "All files (*.*)|*.*"
        For Each format As String In formats
            totalString += "|" + format.ToUpper() + " Image (*." + format + ")|*." + format
        Next
        OpenFileDialog1.Filter = totalString
        SaveFileDialog1.Filter = totalString
        OpenFileDialog1.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        SaveFileDialog1.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    End Sub
    Private Sub MaterialButton1_Click(sender As Object, e As EventArgs) Handles MaterialButton1.Click
        Dim thread As Thread = New Thread(New ThreadStart(Sub() ConvertImage()))
        thread.Start()
    End Sub
    Private Sub MaterialButton2_Click(sender As Object, e As EventArgs) Handles MaterialButton2.Click
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            MaterialTextBox1.Text = OpenFileDialog1.FileName
        End If
    End Sub
    Private Sub MaterialButton3_Click(sender As Object, e As EventArgs) Handles MaterialButton3.Click
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            MaterialTextBox2.Text = SaveFileDialog1.FileName
        End If
    End Sub
    Public Sub EnableAll(ByVal toggle As Boolean)
        MaterialButton1.Enabled = toggle
        MaterialButton2.Enabled = toggle
        MaterialButton3.Enabled = toggle
        MaterialTextBox1.Enabled = toggle
        MaterialTextBox2.Enabled = toggle
    End Sub
    Public Sub ConvertImage()
        EnableAll(False)
        If System.IO.File.Exists(MaterialTextBox1.Text) Then
            Dim extension As String = System.IO.Path.GetExtension(MaterialTextBox1.Text).ToLower().Replace(".", "")
            Dim continueThat As Boolean = False
            For Each format As String In formats
                If format = extension Then
                    continueThat = True
                    Exit For
                End If
            Next
            If continueThat Then
                Try
                    extension = System.IO.Path.GetExtension(MaterialTextBox2.Text).ToLower().Replace(".", "")
                    continueThat = False
                    For Each format As String In formats
                        If format = extension Then
                            continueThat = True
                        End If
                    Next
                    If continueThat Then
                        If System.IO.File.Exists(MaterialTextBox2.Text) Then
                            Try
                                System.IO.File.Delete(MaterialTextBox2.Text)
                            Catch ex As Exception
                                MessageBox.Show("Tried to delete the existing output image file specified but it cannot be deleted!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                EnableAll(True)
                                Exit Sub
                            End Try
                        End If
                        Dim image As Bitmap = New Bitmap(MaterialTextBox1.Text)
                        image.Save(MaterialTextBox2.Text)
                        MessageBox.Show("Succesfully converted this image file!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("Invalid output image file format.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Catch ex As Exception
                    MessageBox.Show("Can not convert this image file!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                MessageBox.Show("Invalid input image file format.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("Can not find this image file.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        OpenFileDialog1.FileName = ""
        SaveFileDialog1.FileName = ""
        EnableAll(True)
    End Sub
End Class