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
        public QuizQuestions()
        {
            InitializeComponent();
        }

        public void Add_Question_Click(object sender, RoutedEventArgs e)
        {

        }

        public QuizQuestions(int id) : this()
        {
            octopusEntities1 db = new octopusEntities1();
            
            questionListBox.ItemsSource = db.getQuizQuestions(id);
        }
    }
}
