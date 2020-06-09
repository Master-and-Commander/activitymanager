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

namespace Octopus
{
    /// <summary>
    /// Interaction logic for QuizQuestions.xaml
    /// </summary>
    public partial class QuizQuestions : Page
    {
        int quizid;
        public QuizQuestions()
        {
            InitializeComponent();
        }

        public void Add_Question_Click(object sender, RoutedEventArgs e)
        {
            bool pass = true;
            octopusEntities1 db = new octopusEntities1();
            if (newQuestion.Text.Trim() == "" || newAnswer.Text.Trim() == "" || newOptions.Text.Trim() == "")
                pass = false;

            if(pass)
            {
                db.insertQuestionToQuiz(quizid, newQuestion.Text, newAnswer.Text, newOptions.Text, "basic");

            }
            else
            {
                MessageBox.Show("Missing entry" ,"Validation");
            }
            // insert question
        }

        public QuizQuestions(int id) : this()
        {
            octopusEntities1 db = new octopusEntities1();
            quizid = id;
            
            questionListBox.ItemsSource = db.getQuizQuestions(id);
        }
    }
}
