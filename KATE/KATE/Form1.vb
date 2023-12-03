Imports MySql.Data.MySqlClient

Public Class Form1
    Dim connectionString As String = "Server=localhost;Database=DR_KATE;User ID=root;Password=;"
    Dim connection As MySqlConnection = New MySqlConnection(connectionString)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        Try
            connection.Open()
            Dim query As String = "SELECT * FROM PATIENT_DETAIL"
            Dim adapter As New MySqlDataAdapter(query, connection)
            Dim dataTable As New DataTable()
            adapter.Fill(dataTable)
            DataGridView1.DataSource = dataTable
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrEmpty(CURRENT_MED.TextBox2.Text) Then
            MessageBox.Show("Please select patient before continuing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Me.Hide()
        CURRENT_MED.Show()
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim firstColumnIndex As Integer = 0
            Dim secondColumnIndex As Integer = 1


            Dim firstColumnValue As Object = DataGridView1.Rows(e.RowIndex).Cells(firstColumnIndex).Value
            Dim secondColumnValue As Object = DataGridView1.Rows(e.RowIndex).Cells(secondColumnIndex).Value

            CURRENT_MED.TextBox2.Text = If(firstColumnValue IsNot Nothing, firstColumnValue.ToString(), "")
            CURRENT_MED.Label3.Text = If(secondColumnValue IsNot Nothing, secondColumnValue.ToString(), "")
        End If
    End Sub

End Class
