using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace flightBooking
{
    public partial class addFlightForm : Form
    {
        private string comp,s="";
        private string fNumber="";
        public addFlightForm(string comp)
        {
            this.comp = comp;
            
            InitializeComponent();
        }

        public addFlightForm(string comp, string fNumber)
        {
            this.fNumber = fNumber;
            this.comp = comp;

            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            string dt, at, dtt, att;
            
            if(textBox5.Text.Length!=8||textBox2.Text.Length<1|| textBox1.Text.Length < 1|| 
                textBox3.Text.Length < 1|| textBox4.Text.Length < 1||!maskedTextBox1.MaskCompleted|| !maskedTextBox2.MaskCompleted)
            {
                MessageBox.Show("wrong input!");
            }
            else 
            {
                
                    dt = dateTimePicker1.Value.Day + "." + dateTimePicker1.Value.Month +
                        "." + dateTimePicker1.Value.Year;
                    dtt = maskedTextBox1.Text;
                    at = dateTimePicker2.Value.Day + "." + dateTimePicker2.Value.Month +
                        "." + dateTimePicker2.Value.Year;
                    att = maskedTextBox2.Text;



                    
                if (fNumber == "")
                {
                    AddFlight(textBox5.Text, dt, at,
                        comboBox1.Text, comboBox3.Text, Convert.ToInt32(textBox2.Text),
                        Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox3.Text),
                        Convert.ToInt32(textBox4.Text), dtt, att);
                }
                else

                {
                      EditFlight(textBox5.Text, dt, at,
                            comboBox1.Text, comboBox3.Text, Convert.ToInt32(textBox2.Text),
                            Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox3.Text),
                            Convert.ToInt32(textBox4.Text), dtt, att);
                }
                Form1 main = this.Owner as Form1;
                if (main != null)
                {
                    main.added();
                }
                res_label.Visible = true;
                next_button.Visible = true;
            }
            
        }

        DateTime dt_begin = new DateTime(1970, 1, 1);
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void next_button_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void add_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void addFlightForm_Load(object sender, EventArgs e)
        {
            FillAirportCollection();
            if(fNumber!="")
            {
                fillfields();
                button1.Text = "edit";

            }
           
        }
        Form1 f1 = new Form1();
        
        private void AddFlight(string f_num, string d_t, string a_t,  
            string d, string a, int e_q, int e_p, int b_q, int b_p, string att, string dtt)
        {
            using (OracleConnection connection = new OracleConnection(f1.connectionstring))
            {
                OracleCommand cmd = new OracleCommand();
                //cmd.Parameters.Clear();
                cmd.Connection = connection;
                cmd.CommandText = "ADDFLIGHT";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("FN", OracleType.Char).Value = f_num;
                cmd.Parameters.Add("FDT", OracleType.Char).Value = d_t;
                cmd.Parameters.Add("FAT", OracleType.Char).Value = a_t;
                cmd.Parameters.Add("FD", OracleType.Char).Value = d;
                cmd.Parameters.Add("FC", OracleType.Char).Value = comp.ToUpper();
                cmd.Parameters.Add("FA", OracleType.Char).Value = a;
                cmd.Parameters.Add("EQ", OracleType.Number).Value = e_q;
                cmd.Parameters.Add("EP", OracleType.Number).Value = e_p;
                cmd.Parameters.Add("BQ", OracleType.Number).Value = b_q;
                cmd.Parameters.Add("BP", OracleType.Number).Value = b_p;
                cmd.Parameters.Add("DTT", OracleType.Char).Value = dtt;
                cmd.Parameters.Add("ATT", OracleType.Char).Value = att;

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        

        private void EditFlight(string f_num, string d_t, string a_t,
           string d, string a, int e_q, int e_p, int b_q, int b_p, string att, string dtt)
        {
            using (OracleConnection connection = new OracleConnection(f1.connectionstring))
            {
                OracleCommand cmd = new OracleCommand();
                //cmd.Parameters.Clear();
                cmd.Connection = connection;
                cmd.CommandText = "EDITFLIGHT";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("OLDFN", OracleType.Char).Value = fNumber;
                cmd.Parameters.Add("FN", OracleType.Char).Value = f_num;
                cmd.Parameters.Add("FDT", OracleType.Char).Value = d_t;
                cmd.Parameters.Add("FAT", OracleType.Char).Value = a_t;
                cmd.Parameters.Add("FD", OracleType.Char).Value = d;
                cmd.Parameters.Add("FA", OracleType.Char).Value = a;
                cmd.Parameters.Add("EQ", OracleType.Number).Value = e_q;
                cmd.Parameters.Add("EP", OracleType.Number).Value = e_p;
                cmd.Parameters.Add("BQ", OracleType.Number).Value = b_q;
                cmd.Parameters.Add("BP", OracleType.Number).Value = b_p;
                cmd.Parameters.Add("DTT", OracleType.Char).Value = dtt;
                cmd.Parameters.Add("ATT", OracleType.Char).Value = att;

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    fNumber = f_num;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void fillfields()
        {
            string queryString =
                "SELECT * FROM "+f1.username.Trim()+".FLIGHTS";
            using (OracleConnection connection = new OracleConnection(f1.connectionstring))
            {
                OracleCommand command = new OracleCommand(queryString, connection);
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        if(reader.GetString(0).Trim() == fNumber.Trim())
                        {
                            comboBox1.Text = reader.GetString(3).Trim();
                            comboBox3.Text = reader.GetString(5).Trim();
                            dateTimePicker1.Value= reader.GetDateTime(1);
                            dateTimePicker2.Value = reader.GetDateTime(2);
                            maskedTextBox1.Text = reader.GetString(10).Trim();
                            maskedTextBox2.Text = reader.GetString(11).Trim();
                            textBox5.Text = reader.GetString(0).Trim();
                            textBox2.Text = reader.GetInt32(6).ToString();
                            textBox1.Text = reader.GetInt32(7).ToString();
                            textBox3.Text = reader.GetInt32(8).ToString();
                            textBox4.Text = reader.GetInt32(9).ToString();

                        }

                    }
                }
                finally
                {
                    // always call Close when done reading.
                    reader.Close();
                }
               
            }
        }

        private void FillAirportCollection()
        {
            string[] List = new string[100];
            int i = 0, j; string s; i = 0;
            string queryString =
                "SELECT AIRPORT_CODE FROM " + f1.username.Trim() + ".AIRPORTS";
            using (OracleConnection connection = new OracleConnection(f1.connectionstring))
            {
                OracleCommand command = new OracleCommand(queryString, connection);
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        List[i] = reader.GetString(0).Trim();
                        i++;
                    }
                }
                finally
                {
                    // always call Close when done reading.
                    reader.Close();
                }
                string[] myList = new string[i];
                for (i = 0; i < myList.Length; i++)
                {
                    myList[i] = List[i];
                }
                comboBox3.Items.AddRange(myList);
                comboBox1.Items.AddRange(myList);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
