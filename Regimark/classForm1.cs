using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Regimark
{
    public partial class classForm1 : Form
    {
        public classForm1()
        {
            InitializeComponent();
        }
        mainForm mainForm = new mainForm();
        
        Font buttonFont = new Font("Arial", 25, FontStyle.Bold);
        Font answersFont = new Font("Arial", 10, FontStyle.Regular);
        public string[] list;    // Get the list of all the students using get-function in Form1

        AnswerButton[] plusButtonArray;
        AnswerButton[] minusButtonArray;


        private void initializeEverything()
        {
            list = mainForm.listOfStudents;
            plusButtonArray = new AnswerButton[list.Length];
            minusButtonArray = new AnswerButton[list.Length];
            Width = Screen.PrimaryScreen.Bounds.Width - 50;
            Height = Screen.PrimaryScreen.Bounds.Height - 50;
            Font buttonFont = new Font("Arial", 25, FontStyle.Bold);
            Font answersFont = new Font("Arial", 10, FontStyle.Regular);
            Label[] markLabelArray = new Label[list.Length];
            Label[] answerLabelArray = new Label[list.Length];
            
            string marksFilePath = getMarksFilePath();

            int[] amountOfStudentsInRows = mainForm.numberOfStudentsInRows;
            int i = 0;
            for (int m = 0; m < amountOfStudentsInRows.Length; m++)
            {
                for (int n = 0; n < amountOfStudentsInRows[m]; n++)
                {

                    answerLabelArray[i] = new Label();  // Make label that shows correct and incorrect answers
                    makeAnswerLabel(answerLabelArray[i], m , n);

                    markLabelArray[i] = new Label();    // Make label that shows mark based on answers
                    makeMarkLabel(markLabelArray[i], m , n);

                    plusButtonArray[i] = new AnswerButton(list[i], i, true, answerLabelArray[i], markLabelArray[i], marksFilePath);    // Make correct answer button for every student in the list
                    makePlusButton(plusButtonArray[i], m, n);

                    minusButtonArray[i] = new AnswerButton(list[i], i, false, answerLabelArray[i], markLabelArray[i], marksFilePath);  // Make incorrect answer button for every student in the list
                    makeMinusButton(minusButtonArray[i], m, n);

                    makeNameLabel(plusButtonArray[i].studentName, m ,n);   // Make name label for every student in the list

                    string studentAnswers = "";
                    if (!(plusButtonArray[i].isTheLastAnswerAMark(plusButtonArray[i].index)))
                    {
                        studentAnswers = plusButtonArray[i].getLastAnswers(plusButtonArray[i].index);
                        plusButtonArray[i].deleteLastAnswersInTextFile(plusButtonArray[i].index);
                        for (int j = 0; j < studentAnswers.Length; j++)
                        {
                            if (studentAnswers[j] == '+')
                                plusButtonArray[i].PerformClick();
                            if (studentAnswers[j] == '-')
                                minusButtonArray[i].PerformClick();
                        }
                    }
                    i++;
                }
            }
        }

        private void classForm1_Load(object sender, EventArgs e)  // Set up a form size and load student list
        {
            initializeEverything();
        }
        
        private string getMarksFilePath()
        {
            string userPath = "C:\\Users\\" + Environment.UserName;
            if (!(Directory.Exists(userPath + "\\Regimark")))
                Directory.CreateDirectory(userPath + "\\Regimark");
            string folderPath = userPath + "\\Regimark\\";
            string newFilePath = folderPath + "marksPath.txt";
            string marksPath = File.ReadAllText(newFilePath);
            return marksPath;
        }

        private int amountOfButtons(int index)   // Get amount of button that can fit in the form vertically
        {
            int amountOfButtons = 0;
            int[] amountOfStudentsInRows = mainForm.numberOfStudentsInRows;
            int count = 0;
            int rows = amountOfStudentsInRows.Length;
            int[] sumsOfStudentsInRow = new int[rows];
            for (int i = 0; i < rows; i++)
            {
                int sum = 0;
                for (int j = 0; j <= count; j++)
                    sum += amountOfStudentsInRows[j];
                count++;
                sumsOfStudentsInRow[i] = sum;
            }
            if (index <= sumsOfStudentsInRow[0])
                amountOfButtons = amountOfStudentsInRows[0];
            else
            {
                for (int i = 1; i < rows; i++)
                {
                    if ((index > sumsOfStudentsInRow[i - 1]) & (index <= sumsOfStudentsInRow[i]))
                        amountOfButtons = amountOfStudentsInRows[i];
                }
            }
            if (amountOfButtons == 0)
                for (int i = 0; i < rows; i++)
                {
                    if (index == sumsOfStudentsInRow[i])
                        amountOfButtons = amountOfStudentsInRows[i];
                }
            return amountOfButtons;
        }
        
        private void makeAnswerLabel(Label answerLabel, int m, int n)  // Make and put on the form answerLabel
        {
            m = 225 * m;
            answerLabel.Top = n * 75 + 75;
            answerLabel.Left = 110 + m;
            answerLabel.Width = 50;
            answerLabel.Font = answersFont;
            Controls.Add(answerLabel);
        }
        private void makeMarkLabel(Label markLabel, int m, int n)  // Make and put on the form markLabel
        {
            m = 225 * m;
            markLabel.Top = n * 75 + 75;
            markLabel.Left = 160 + m;
            markLabel.Width = 10;
            markLabel.Font = answersFont;
            Controls.Add(markLabel);
        }
        private void makePlusButton(AnswerButton plusButton, int m, int n) // Make and put on the form button for correct answers
        {
            m = 225 * m;
            plusButton.Text = "+";
            plusButton.Left = 50 + m;
            plusButton.Top = n * 75 + 50;
            plusButton.Width = 50;
            plusButton.Height = 50;
            plusButton.Font = buttonFont;
            plusButton.Click += new EventHandler(plusButton.StudentAnswered);
            Controls.Add(plusButton);
        }
        private void makeMinusButton(AnswerButton minusButton, int m, int n)   // Make and put on the form button for incorrect answers
        {
            m = 225 * m;
            minusButton.Text = "-";
            minusButton.Left = 175 + m;
            minusButton.Top = n * 75 + 50;
            minusButton.Width = 50;
            minusButton.Height = 50;
            minusButton.Font = buttonFont;
            minusButton.Click += new EventHandler(minusButton.StudentAnswered);
            Controls.Add(minusButton);
        }
        private void makeNameLabel(string name, int m, int n) // Make an put on the form nameLabel
        {
            m = 225 * m;
            Label nameLabel = new Label();
            nameLabel.Text = name;
            nameLabel.Top = n * 75 + 55;
            nameLabel.Left = 105 + m;
            Controls.Add(nameLabel);
        }

        private void classForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.Show();
        }
    }
}