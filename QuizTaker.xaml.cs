using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.ComponentModel;
using System.Net.Mail;
using System.IO;

namespace Octopus
{
    /// <summary>
    /// Interaction logic for QuizTaker.xaml
    /// </summary>
    /// 

    public class QuizData : INotifyPropertyChanged
    {
        private int quizid;
        private int userid;
        private int time;
        private int increment = 1000;
        private string timeString;
        private System.Timers.Timer aTimer = new System.Timers.Timer(1000);
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler quizEnded;

        public void stopTimer()
        {
            aTimer.Stop();
        }
        public int QuizID
        {
            get
            {
                return quizid;
            }
            set
            {
                quizid = value;
            }
        }

        public int UserID
        {
            get
            {
                return userid;
            }
            set
            {
                userid = value;
            }
        }

        public int QuizTime
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                TimeSpan t = TimeSpan.FromMilliseconds(time);
                this.QuizTimeString = string.Format("{0:D2}:{1:D2}",
                                        t.Minutes,
                                        t.Seconds);
                aTimer = new System.Timers.Timer(increment);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
        }
        
        public string QuizTimeString
        {
            get
            {
                return timeString;
            }
            set
            {
                timeString = value;
                NotifyPropertyChanged("QuizTimeString");
            }
        }
        

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            
            // update quiz time string
            time -= increment;
            TimeSpan t = TimeSpan.FromMilliseconds(time);
            this.QuizTimeString = string.Format("{0:D2}:{1:D2}",
                                    t.Minutes,
                                    t.Seconds);
            if(time <= 0)
            {
                aTimer.Stop();
                this.QuizTimeString = "Time's up!";
                quizEnded?.Invoke(this, e);

            }
        }
    }
    public partial class QuizTaker : Page
    {
        

        int pointer = 0;
        Octopus.fetchRandomQuestionsfromQuiz_Result myQuestion;
        bool[] answers = new bool[10];
        QuizData quizdata;

        public QuizTaker()
        {InitializeComponent();}
        
        public QuizTaker(object data) : this()
        {
            this.DataContext = data;
        }

        public QuizTaker(int id, int number) :  this()
        {
            quizdata = new QuizData
            {
                QuizID = id,
                UserID = 0,
                QuizTime = 100000
            };

            this.DataContext = quizdata;
            quizdata.quizEnded += Quizdata_quizEnded;
            octopusEntities1 db = new octopusEntities1();
            questionListBox.ItemsSource = db.fetchRandomQuestionsfromQuiz(id,number);
            answers = new bool[number];
           for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = false;
            }
            
            // randomly select 10 of these questions

            myQuestion = (Octopus.fetchRandomQuestionsfromQuiz_Result)this.questionListBox.Items[pointer];
            questionHandler();
            
        }

        private void Quizdata_quizEnded(object sender, EventArgs e)
        {
            Submit_Quiz();
            quizdata.stopTimer();
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

            // randomize ordering of options
            for (int i = 0; i < options.Length - 1; i++)
            {
                int j = rnd.Next(i, options.Length);
                string temp = options[i];
                options[i] = options[j];
                options[j] = temp;
            }

            foreach (string option in options)
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


        public string returnMissedQuestions()
        {
            string missedQuestions = "Here are the questions you missed:\n\n";
            Octopus.fetchRandomQuestionsfromQuiz_Result currentQuestion;
            for (int i = 0; i < answers.Length; i++)
            {
                
                
                if(!answers[i])
                {

                    currentQuestion = (Octopus.fetchRandomQuestionsfromQuiz_Result)this.questionListBox.Items[i];
                    missedQuestions += currentQuestion.quizquestion + "\n";
                    missedQuestions += "Answer: " + currentQuestion.quizanswer + "\n\n";
                }
            }
            return missedQuestions;
        }
        public void writeToFile()
        {
            string[] lines = { "Hello User", "you took a quiz with a score of " + getNumberCorrect() + "/" + answers.Length, returnMissedQuestions() };

            // Set a variable to the Documents path.
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Random rnd = new Random();

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "quizresults" + rnd.Next(0, 100000) + ".txt")))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
        }

        public void Submit_Quiz()
        {
            // QuizTaken should have 0. id, 1. userid, 2. quizid, 3. numberofcorrectanswers, 4. totalnumberofquestions, failedquestion ids, failed answers
            MessageBox.Show("Now submitting quiz with a score of " + getNumberCorrect() + "/" + answers.Length + ". A file has been uploaded to your my documents folder with more info ", "Validation");
            octopusEntities1 db = new octopusEntities1();
            writeToFile();
            //db.insertCompletedQuiz(quizdata.UserID, quizdata.QuizID,getNumberCorrect(),answers.Length, "5|10");

        }

        public void Submit_Quiz_Click(object sender, RoutedEventArgs e)
        {
            Submit_Quiz();
        }

        public void Next_Question_Click(object sender, RoutedEventArgs e)
        {
            if ((string)next_button.Content == "Submit")
            {
                Submit_Quiz();
            }
            else if (pointer < answers.Length)
            {
                checkAnswer();
                pointer++;
                if (pointer == answers.Length -1) 
                {next_button.Content = "Submit";}
                
                else
                {
                    myQuestion = (Octopus.fetchRandomQuestionsfromQuiz_Result)this.questionListBox.Items[pointer];
                    questionHandler();
                    next_button.Content = "Next";
                }
            }

        }

        public void Previous_Question_Click(object sender, RoutedEventArgs e)
        {
            checkAnswer();
            pointer--;
            myQuestion = (Octopus.fetchRandomQuestionsfromQuiz_Result)this.questionListBox.Items[pointer];
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
