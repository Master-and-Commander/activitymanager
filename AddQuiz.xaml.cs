using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using Octopus;


namespace Octopus
{
    /// <summary>
    /// Interaction logic for AddQuiz.xaml
    /// </summary>
    /// Based on https://www.c-sharpcorner.com/article/wpf-database-communication-adding-new-record-to-the-database/
   
    public partial class AddQuiz : Page
    {
        public AddQuiz() {InitializeComponent();}


        public void Insert_Quiz_Click(object sender, RoutedEventArgs e)
        {
            octopusEntities1 db = new octopusEntities1();
            var special = 100;
            // check validity of fields
            if (txtQuizName.Text.Trim() == "")
            {
                txtQuizName.Focus();
                MessageBox.Show("Quiz name is required!", "Validation");
            }

            else if (txtDescription.Text.Trim() == "")
            {
                txtDescription.Focus();
                MessageBox.Show("Description is required!", "Validation");
            }

            else if (txtNumber.Text.Trim() == "")
            {
                txtDescription.Focus();
                MessageBox.Show("Description is required!", "Validation");
            }

            else
            {
                
                //var newQuiz = new quiz { name = txtQuizName.Text, description = txtDescription.Text, count = int.Parse(txtNumber.Text) };
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    /*
                   db.Database.SqlQuery<quiz>(
                    "EXEC insertQuiz @quizname, @quizcount, @quizdescription ",
                    new SqlParameter("quizname", txtQuizName.Text), 
                    new SqlParameter("quizcount", int.Parse(txtNumber.Text)),
                    new SqlParameter("quizdescription", txtDescription.Text)
                      );
                    */
                    // ObjectParameter param = new ObjectParameter("identity", 0);
                    System.Data.Entity.Core.Objects.ObjectParameter param = new System.Data.Entity.Core.Objects.ObjectParameter("Identity",0);
                   var id = db.insertQuiz(txtQuizName.Text, int.Parse(txtNumber.Text), txtDescription.Text, param);
                    
                }
                catch (Exception er)
                { MessageBox.Show(er.Message, "Error"); }

            }
        }
            
    }
        }





