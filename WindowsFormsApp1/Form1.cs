using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SqlConnection conn = null;
        SqlDataAdapter adapter = null;
        DataSet dataSet = null;
        public Form1()
        {
            InitializeComponent();

            conn = new SqlConnection(ConfigurationManager.AppSettings.Get("Key"));
        }


        private void AuthorReader()
        {
            dataSet = new DataSet();
            adapter = new SqlDataAdapter("SELECT * FROM Authors", conn);
            adapter.Fill(dataSet);

            foreach (DataTable data in dataSet.Tables)
                dataGridView1.DataSource = data;
        }

        private void AuthorUpdate()
        {
            SqlCommand updateCommand = new SqlCommand()
            {
                CommandText = "sp_Update",
                Connection = conn,
                CommandType = CommandType.StoredProcedure,
            };

            updateCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
            updateCommand.Parameters["@Id"].SourceVersion = DataRowVersion.Current;
            updateCommand.Parameters["@Id"].SourceColumn = "Id";

            updateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
            updateCommand.Parameters["@Name"].SourceVersion = DataRowVersion.Current;
            updateCommand.Parameters["@Name"].SourceColumn = "FirstName";

            updateCommand.Parameters.Add(new SqlParameter("@Surname", SqlDbType.NVarChar));
            updateCommand.Parameters["@Surname"].SourceVersion = DataRowVersion.Current;
            updateCommand.Parameters["@Surname"].SourceColumn = "LastName";

            adapter.UpdateCommand = updateCommand;

            adapter.Update(dataSet);

        }


        private void AuthorInsert()
        {
            SqlCommand insertCommand = new SqlCommand()
            {
                CommandText = "INSERT Authors\r\nVALUES(@Id, @Name, @Surname)",
                Connection = conn,
            };

            insertCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
            insertCommand.Parameters["@Id"].SourceVersion = DataRowVersion.Current;
            insertCommand.Parameters["@Id"].SourceColumn = "Id";

            insertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
            insertCommand.Parameters["@Name"].SourceVersion = DataRowVersion.Current;
            insertCommand.Parameters["@Name"].SourceColumn = "FirstName";

            insertCommand.Parameters.Add(new SqlParameter("@Surname", SqlDbType.NVarChar));
            insertCommand.Parameters["@Surname"].SourceVersion = DataRowVersion.Current;
            insertCommand.Parameters["@Surname"].SourceColumn = "LastName";

            adapter.InsertCommand = insertCommand;

            adapter.Update(dataSet);
        }

        private void AuthorDeleter()
        {
            SqlCommand delCommand = new SqlCommand()
            {
                CommandText = "\tDELETE Authors\r\n\tWHERE Id = @Id",
                Connection = conn,
            };
            delCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
            delCommand.Parameters["@Id"].SourceVersion = DataRowVersion.Current;
            delCommand.Parameters["@Id"].SourceColumn = "Id";

            adapter.DeleteCommand = delCommand;

            adapter.Update(dataSet);
        }


        private void AllCommands()
        {
            SqlCommandBuilder sq = new SqlCommandBuilder(adapter);
            sq.RefreshSchema();

            adapter.Update(dataSet);
        }

        private void ExecuteCommands()
        {
            try
            {
                dataSet = new DataSet();
                adapter = new SqlDataAdapter(textBox1.Text, conn);

                adapter.Fill(dataSet);

                foreach (DataTable data in dataSet.Tables)
                    dataGridView1.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn == button1)
                AuthorReader();
            else if (btn == button2)
                AuthorUpdate();
            else if (btn == button3)
                ExecuteCommands();
            else if (btn == button4)
                AuthorInsert();
            else if (btn == button5)
                AllCommands();
            else if(btn == button6)
                AuthorDeleter();
        }

    }
}
