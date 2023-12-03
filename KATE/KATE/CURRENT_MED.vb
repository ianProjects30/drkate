Imports MySql.Data.MySqlClient

Public Class CURRENT_MED
    Private Sub CURRENT_MED_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        full_name()
    End Sub

    Dim connectionString As String = "Server=localhost;Database=DR_KATE;User ID=root;Password=;"
    Dim connection As MySqlConnection = New MySqlConnection(connectionString)

    Private Sub LoadData()
        Dim patientID As String = TextBox2.Text

        Try
            connection.Open()
            Dim query As String = "SELECT MEDICATION.`MEDICATION ID`, MEDICATION.`MEDICINE NAME`, `MEDICATION`.`DOSES`
                                FROM patient_detail
                                JOIN MEDICATION ON PATIENT_DETAIL.`PATIENTS ID` = `MEDICATION`.`PATIENTS ID`
                                WHERE `PATIENT_DETAIL`.`PATIENTS ID` = '" & patientID & "';"

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

    Private Sub full_name()
        Dim patientID As String = TextBox2.Text

        Try
            connection.Open()

            Dim query As String = "SELECT `FULL NAME` FROM PATIENT_DETAIL WHERE `PATIENTS ID` = @PatientID"
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@PatientID", patientID)
            Dim result As Object = cmd.ExecuteScalar()

            If result IsNot Nothing Then
                Label3.Text = result.ToString()
            Else
                MessageBox.Show("No record found for the specified patient ID.")
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim searchText As String = TextBox1.Text.Trim()

        If Not String.IsNullOrEmpty(searchText) Then
            CType(DataGridView1.DataSource, DataTable).DefaultView.RowFilter = $"`medicine` LIKE '%{searchText}%'"
        Else
            CType(DataGridView1.DataSource, DataTable).DefaultView.RowFilter = ""
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        LoadData()
        full_name()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to update the table?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            UpdateTable()
        Else
            MessageBox.Show("Update canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


    Private Sub UpdateTable()
        Try
            Dim meddoses As String = TextBox4.Text
            Dim medid As String = TextBox3.Text
            connection.Open()

            Dim query As String = "UPDATE medication SET `DOSES` = '" & meddoses & "' WHERE `MEDICATION ID` = '" & medid & "'"
            Dim cmd As New MySqlCommand(query, connection)
            cmd.ExecuteNonQuery()

            MessageBox.Show("Table updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            DataGridView1.DataSource = Nothing
            DataGridView1.Rows.Clear()
            DataGridView1.Columns.Clear()
            Dim dataTable As New DataTable()
            Using adapter As New MySqlDataAdapter("SELECT * FROM medication", connection)
                adapter.Fill(dataTable)
            End Using
            DataGridView1.DataSource = dataTable

        Catch ex As Exception
            MessageBox.Show("Error updating table: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            connection.Close()
        End Try
    End Sub



End Class
