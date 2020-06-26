using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace Octopus
{
    /// <summary>
    /// Interaction logic for ActivityDashboard.xaml
    /// </summary>
    /* https://parallelcodes.com/wpf-mvvm-listbox-binding-from-sql-database/ */

    public partial class ActivityDashboard : Page
    {
        public ActivityDashboard()
        {
            InitializeComponent();
            octopusEntities1 co = new octopusEntities1();
            quizListBox.ItemsSource = co.selectQuizzes();
            activityListBox.ItemsSource = co.selectActivities();

        }


        private void View_Activity_Click(object sender, RoutedEventArgs e)
        {
            ActivityView activityReportPage = new ActivityView(this.activityListBox.SelectedItem);
            this.NavigationService.Navigate(activityReportPage);
        }

        private void Edit_Quiz_Click(object sender, RoutedEventArgs e)
        {
            QuizView quizViewPage = new QuizView(this.quizListBox.SelectedItem);
            this.NavigationService.Navigate(quizViewPage);
        }
        private void Take_Quiz_Click(object sender, RoutedEventArgs e)
        {
            QuizTaker takeQuizPage = new QuizTaker(((Octopus.selectQuizzes_Result)quizListBox.SelectedItem).id, (int)((Octopus.selectQuizzes_Result)quizListBox.SelectedItem).count);
            this.NavigationService.Navigate(takeQuizPage);
        }

        private void Add_Quiz_Click(object sender, RoutedEventArgs e)
        {
            AddQuiz addQuizPage = new AddQuiz();
            this.NavigationService.Navigate(addQuizPage);
        }

      
    }
}
