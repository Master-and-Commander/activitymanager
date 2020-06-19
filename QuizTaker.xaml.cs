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
    /// Interaction logic for QuizTaker.xaml
    /// </summary>
    /// 
    
    public partial class QuizTaker : Page
    {
        int pointer = 0;
        Octopus.getQuizQuestions_Result myQuestion;
        bool[] answers = new bool[10];
        public QuizTaker()
        {
            InitializeComponent();
        }

        
        public QuizTaker(object data) : this()
        {
            this.DataContext = data;
            
        }

        public QuizTaker(int id) :  this()
        {
            octopusEntities1 db = new octopusEntities1();
            questionListBox.ItemsSource = db.getQuizQuestions(id);
            myQuestion = (Octopus.getQuizQuestions_Result)this.questionListBox.Items[pointer];
            questionHandler();
        }

        public void checkAnswer()
        {
            bool correctAnswer = false;
            // find the selected answer and save it
            foreach (var child in questionpane.Children.OfType<StackPanel>())
            {
                foreach (var childchild in child.Children.OfType<CheckBox>().Where(cb => (bool)cb.IsChecked))
                {
                    if ((string)childchild.Tag == "right")
                    {correctAnswer = true;}
                    else
                    {correctAnswer = false;}
                }
            }
            if (!correctAnswer)
            {answers[pointer] = false;}
            else
            {answers[pointer] = true;}
        }

        public void questionHandler()
        {
            // remove old question and options
            questionpane.Children.Clear();

            // add new question and options
            Label newQuestionLabel = new Label();
            newQuestionLabel.Name = "QuestionTitle"+pointer;
            newQuestionLabel.Content = myQuestion.quizquestion;

            questionpane.Children.Add(newQuestionLabel);
            Random rnd = new Random();
            
            string[] options = myQuestion.quizoptions.Split('|').Select(str => str.Trim()).ToArray();
            int spot = rnd.Next(0, options.Length);
            StackPanel newQuestionOptionsPane = new StackPanel();
            newQuestionOptionsPane.Name = "Questions"+pointer;

            CheckBox box = new CheckBox();
            TextBlock block = new TextBlock();
            StackPanel newOptionsPane = new StackPanel();
            bool added = false;
            int counter = 0;
            foreach(string option in options)
            {
                newOptionsPane = new StackPanel();
                newOptionsPane.Orientation = Orientation.Horizontal;

                box = new CheckBox();
                box.Margin = new Thickness(30,10,0,5);
                box.Tag = "wrong";

                block = new TextBlock();
                block.Name = "option" + counter;
                block.Text = option.Trim();
                block.Margin = new Thickness(10);
                if(counter == spot)
                {
                    StackPanel answerPane = new StackPanel();
                    answerPane.Orientation = Orientation.Horizontal;
                    
                    CheckBox boxAnswer = new CheckBox();
                    boxAnswer.Margin = new Thickness(30, 10, 0, 5);
                    boxAnswer.Tag = "right";

                    TextBlock blockAnswer = new TextBlock();
                    
                    blockAnswer.Text = myQuestion.quizanswer.Trim();
                    blockAnswer.Margin = new Thickness(10);
                    added = true;

                    answerPane.Children.Add(boxAnswer);
                    answerPane.Children.Add(blockAnswer);
                    questionpane.Children.Add(answerPane);
                }

                newOptionsPane.Children.Add(box);
                newOptionsPane.Children.Add(block);
                questionpane.Children.Add(newOptionsPane);
                counter++;

            }
            if(!added)
            {
                StackPanel answerPane = new StackPanel();
                CheckBox boxAnswer = new CheckBox();
                boxAnswer.Margin = new Thickness(30, 10, 0, 5);
                TextBlock blockAnswer = new TextBlock();
                boxAnswer.Tag = "right";
                blockAnswer.Text = myQuestion.quizanswer.Trim();
                added = true;

                answerPane.Children.Add(boxAnswer);
                answerPane.Children.Add(blockAnswer);
                questionpane.Children.Add(answerPane);
            }



        }

        public void Submit_Quiz_Click(object sender, RoutedEventArgs e)
        {
           // quizid, number of questions, score, userid
        }

        public void Next_Question_Click(object sender, RoutedEventArgs e)
        {
            if (pointer < answers.Length)
            {
                checkAnswer();
                pointer++;
                if (pointer == answers.Length -1) 
                {next_button.Content = "Submit";}
                
                else
                {
                    myQuestion = (Octopus.getQuizQuestions_Result)this.questionListBox.Items[pointer];
                    questionHandler();
                    next_button.Content = "Next";
                }
            }
            else
            {
                MessageBox.Show("Now submitting quiz with a score of " + getNumberCorrect() + "/" + answers.Length, "Validation");
            }

        }

        public void Previous_Question_Click(object sender, RoutedEventArgs e)
        {
            checkAnswer();
            pointer--;
            myQuestion = (Octopus.getQuizQuestions_Result)this.questionListBox.Items[pointer];
            questionHandler();
        }

        public int getNumberCorrect()
        {
            int response = 0;
            foreach (bool value in answers)
            { if (value) {response++;} }
            return response;
        }
    }
}
