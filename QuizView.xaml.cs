﻿using System;
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

        private void View_Questions_Click(object sender, RoutedEventArgs e)
        {
            QuizQuestions quizQuestionsPage = new QuizQuestions();
            this.NavigationService.Navigate(quizQuestionsPage);
        }

        private void Update_Quiz_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
