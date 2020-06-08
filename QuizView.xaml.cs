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
    /// Interaction logic for QuizView.xaml
    /// </summary>
    public partial class QuizView : Page
    {
        public QuizView()
        {
            InitializeComponent();
        }

        public QuizView(object data) : this()
        {
            this.DataContext = data;
        }

        // view questions opens quiz questions page where questions can be added or removed

        private void View_Questions_Click(object sender, RoutedEventArgs e)
        {
            /* Not sure how to get this one to work https://stackoverflow.com/questions/3841069/passing-a-value-along-from-a-button-in-a-wpf-listview
             * TextBox textBox = (TextBox)sender;

                int id =  ((TheBounObjectType)textBox.DataContext).Id;
             */
            int id = int.Parse(View_Quiz_Button.Tag.ToString());
            QuizQuestions quizQuestionsPage = new QuizQuestions(id);
            this.NavigationService.Navigate(quizQuestionsPage);
        }

        // update quiz updates name and count
        private void Update_Quiz_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
