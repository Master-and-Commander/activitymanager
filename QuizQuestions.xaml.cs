using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IronXL;
namespace Octopus
{
    /// <summary>
    /// Interaction logic for QuizQuestions.xaml
    /// </summary>
    public partial class QuizQuestions : Page
    {
        int quizid;
        string filename;
        public QuizQuestions()
        {
            InitializeComponent();
        }
        public void Add_File_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "XLSX Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                //Supported spreadsheet formats for reading include: XLSX, XLS, CSV and TSV
                WorkBook workbook = WorkBook.Load(filename);
                WorkSheet sheet = workbook.DefaultWorkSheet;
                //Select cells easily in Excel notation and return the calculated value, date, text or formula
                int cellValue = sheet["A2"].IntValue;
                // Read from Ranges of cells elegantly.
                bool stop = false;
                int row = 0;
                octopusEntities1 db = new octopusEntities1();
                RangeRow rowdata;
                while (!stop)
                {
                    
                        // check if existing prior to inserting
                        ObjectResult<Octopus.checkForExistingQuestion_Result> number = db.checkForExistingQuestion(quizid, (string)sheet.GetCellAt(0, row).Text.Trim());
                        if (number.ToArray().Length == 0)
                        {
                            db.insertQuestionToQuiz(quizid, (string)sheet.GetCellAt(0, row).Text.Trim(), (string)sheet.GetCellAt(1, row).Text.Trim(), (string)sheet.GetCellAt(2, row).Text.Trim(), "basic");

                        }
                        else
                        {
                            Console.WriteLine("Question already existing");
                        }
                        

                    

                    row++;
                }
                
            }
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
                newQuestion.Text = "";
                newAnswer.Text = "";
                newOptions.Text = "";
                MessageBox.Show("Question inserted!", "Validation");


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
