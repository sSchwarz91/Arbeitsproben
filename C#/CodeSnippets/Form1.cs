using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace Bootsfahrschule_OWL
{
    public partial class SKS : Form
    {

        string connectionString;
        public int KatFragID = 0;
        public string Unterkategorie;
        public int AnzFragUnterkat;
        public int Input;
        public bool hasImage = false;
        public SKS()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["Bootsfahrschule_OWL.Properties.Settings.DatabaseSKSConnectionString"].ConnectionString;
        }

        //function to test, whether a Sting is numeric (int)
        public static bool IsNumeric(string val)
        {
            if (val.Length <= 9)
            {
                try
                {
                    System.Int32.Parse(val);
                    return true;
                }
                catch (System.FormatException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private void ButtonStart_Click(object sender, EventArgs e)
        {
            Start.Visible = false;
        }


        private void ButtonBeenden_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Count the numbers of Questions in the Subcategories
        private void CountQuestions()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("Select Count(*) from SksData where Unterkategorie = @Kat", connection);
            cmd.Parameters.AddWithValue("@Kat", Unterkategorie);
            SqlDataReader da = cmd.ExecuteReader();

            while (da.Read())
            {
                AnzFragUnterkat = (int)da.GetValue(0);

            }
            connection.Close();
        }


        private void setAntwortImage()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select Bild2 from SksData where UnterkategorieFrageId = @Id AND Unterkategorie =@Kat AND Bild2 Is NOT NUll", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", KatFragID);
                    cmd.Parameters.AddWithValue("@Kat", Unterkategorie);
                    connection.Open();
                    byte[] bytes2 = (byte[])cmd.ExecuteScalar();
                    connection.Close();
                    if (bytes2 != null)
                    {
                        hasImage = true;
                        AntwortBild.Image = Image.FromStream(new MemoryStream(bytes2));
                    }
                }
            }
        }

        private void setFragenImage()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select Bild from SksData where UnterkategorieFrageId = @Id AND Unterkategorie =@Kat AND Bild Is NOT NUll", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", KatFragID);
                    cmd.Parameters.AddWithValue("@Kat", Unterkategorie);
                    connection.Open();
                    byte[] bytes = (byte[])cmd.ExecuteScalar();
                    connection.Close();
                    if (bytes != null)
                    {
                        FragenBild.Visible = true;
                        FragenBild.Image = Image.FromStream(new MemoryStream(bytes));
                    }
                }
            }
        }



        //Set the Questione and Answer Labels 
        private void SetTextLabels()
        {
            FragenBild.Visible = false;
            AntwortBild.Visible = false;
            AntwortLabel.Visible = false;
            hasImage = false;
            setFragenImage();
            setAntwortImage();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("Select Frage, Antwort, UnterkategorieFrageId from SksData where UnterkategorieFrageId = @Id AND Unterkategorie =@Kat", connection);
            cmd.Parameters.AddWithValue("@Id", KatFragID);
            cmd.Parameters.AddWithValue("@Kat", Unterkategorie);
            SqlDataReader da = cmd.ExecuteReader();
            while (da.Read())
            {
                FragenLabel.Text = da.GetValue(0).ToString();
                AntwortLabel.Text = da.GetValue(1).ToString();

                CurrentQueTxt.Text = ("Frage: " + da.GetValue(2).ToString() + " / " + AnzFragUnterkat);
            }

            connection.Close();            
           
            if (FragenBild.Visible == true)
            {
                FragenBild.Location = new Point(QuestionPane.Size.Width / 2 - FragenBild.Size.Width / 2, FragenLabel.Location.Y + FragenLabel.Size.Height + 10);
                AntwortLabel.Location = new Point(FragenLabel.Location.X, FragenLabel.Location.Y + FragenBild.Size.Height + FragenLabel.Size.Height + 30);
                AntwortBild.Location = new Point(QuestionPane.Size.Width / 2 - FragenBild.Size.Width / 2, FragenLabel.Location.Y + FragenLabel.Size.Height + FragenBild.Size.Height + AntwortLabel.Size.Height + 30);
            }
            else
            {
                AntwortLabel.Location = new Point(FragenLabel.Location.X, FragenLabel.Location.Y + FragenLabel.Size.Height + 30);
                AntwortBild.Location = new Point(QuestionPane.Size.Width / 2 - FragenBild.Size.Width / 2, FragenLabel.Location.Y + FragenLabel.Size.Height + AntwortLabel.Size.Height + 30);

            }
        }


        private void NextQuestionButton_Click(object sender, EventArgs e)
        {
            if (KatFragID >= 1 && KatFragID <= AnzFragUnterkat - 1)
            {
                KatFragID++;
                AntwortLabel.Visible = false;
                SetTextLabels();
            }
            else if (KatFragID > AnzFragUnterkat)
            {
                KatFragID = 0;
            }
            else
            {
                SelectQuestionTxt.Text = "Keine vorherige Frage vorhanden";
            }
        }

        private void PreviousQuestButton_Click(object sender, EventArgs e)
        {
            if (KatFragID >= 2 && KatFragID <= AnzFragUnterkat)
            {
                KatFragID--;
                AntwortLabel.Visible = false;
                SetTextLabels();
            }
            else if (KatFragID < 1)
            {
                KatFragID = 0;
            }
            else
            {
                SelectQuestionTxt.Text = "Keine vorherige Frage vorhanden";
            }
        }


        private void solution_Click(object sender, EventArgs e)
        {
            if (KatFragID != 0)
            {
                AntwortLabel.Visible = true;
                if (hasImage == true)
                {
                    AntwortBild.Visible = true;
                }
            }

        }


        //Select a Question from the Subcategories
        private void SelectQuestion_Click(object sender, EventArgs e)
        {
            AntwortLabel.Visible = false;
            if (IsNumeric(SelectQuestionTxt.Text))
            {
                int Input = Int32.Parse(SelectQuestionTxt.Text);
                if (KatFragID != 0 && Input <= AnzFragUnterkat && Input > 0)
                {
                    KatFragID = Input;
                    SetTextLabels();
                    SelectQuestionTxt.Text = "";
                }
                else if (KatFragID == 0)
                {
                    SelectQuestionTxt.Text = "Kategorie wählen";
                }
                else
                {
                    SelectQuestionTxt.Text = "Ungültige Fragennummer";
                }
            }
            else
            {
                SelectQuestionTxt.Text = "Ungültige Eingabe";
            }

        }


        private void NavButton1_Click(object sender, EventArgs e)
        {
            KatFragID = 1;
            Unterkategorie = "Seekarten";
            CountQuestions();
            SetTextLabels();
        }

       

        private void Database_Click(object sender, EventArgs e)
        {
            if (UserText.Text == "Admin" && PwTextBox.Text == "1234")
            {
                Debug.Print(UserText.Text);
                Form Database = new Database(UserText.Text);                
                Database.Show();
            }
            else
            {
                MessageBox.Show("Benutzername oder Passwort falsch.", "FormClosing", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

    }
}
