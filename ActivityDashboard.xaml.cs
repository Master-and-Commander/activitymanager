﻿using System;
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
using System.Linq;
namespace Octopus
{
    /// <summary>
    /// Interaction logic for ActivityDashboard.xaml
    /// </summary>
    public partial class ActivityDashboard : Page
    {
        public ActivityDashboard()
        {
            InitializeComponent();
        }

        private void View_Activity_Click(object sender, RoutedEventArgs e)
        {
            ActivityView activityReportPage = new ActivityView(this.peopleListBox.SelectedItem);
            this.NavigationService.Navigate(activityReportPage);
        }

        private void Edit_Quiz_Click(object sender, RoutedEventArgs e)
        {
            QuizView quizViewPage = new QuizView(this.quizListBox.SelectedItem);
            this.NavigationService.Navigate(quizViewPage);
        }
        private void Take_Quiz_Click(object sender, RoutedEventArgs e)
        {
            QuizTaker takeQuizPage = new QuizTaker(this.quizListBox.SelectedItem);
            this.NavigationService.Navigate(takeQuizPage);
        }

        private void Add_Quiz_Click(object sender, RoutedEventArgs e)
        {
            AddQuiz addQuizPage = new AddQuiz();
            this.NavigationService.Navigate(addQuizPage);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
