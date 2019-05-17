using System.Windows.Forms;
using System;
using System.IO;

namespace Regimark
{
    public class AnswerButton : Button
    {
        public bool answer;
        public string studentName;
        public int index;
        public Label answerLabel;
        public Label markLabel;
        public string marksPath;

        public AnswerButton (string nameOfStudent, int studentIndex, bool correctAnswer, Label studentAnswerLabel, Label studentMarkLabel, string pathOfMarksFile)  // Class constructor
        {
            studentName = nameOfStudent;
            index = studentIndex;
            answer = correctAnswer;
            answerLabel = studentAnswerLabel;
            markLabel = studentMarkLabel;
            marksPath = pathOfMarksFile;
        }
        
        int countAnswers;

        public void StudentAnswered (object sender, EventArgs e)
        {
            if (answer)
            {
                // Correct Answer
                countAnswers = answerLabel.Text.Length;
                if (countAnswers < 3)   // Student hasn't answered 3 times
                {
                    answerLabel.Text = answerLabel.Text + "+";  // Show correct answer on the label
                    putAnswerIntoTextFile("+", index, false);
                    if (countAnswers == 2)
                    {
                        string resultingMark = getResultMark(index);  // Get the resulting mark based on answers
                        markLabel.Text = resultingMark;    // If student answered 3rd time, show the mark
                        putAnswerIntoTextFile(resultingMark, index, true);  // Save mark in the text file
                    }
                }
                else    // Student has already answered 3 times
                {
                    countAnswers = 0;
                    DialogResult dialogResult = MessageBox.Show("The student has answered 3 times. Do you want to remove answers?", "Regimark", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)   // Clear previous results if user agrees
                    {
                        answerLabel.Text = "";
                        markLabel.Text = "";
                        countAnswers = answerLabel.Text.Length;
                    }
                    else if (dialogResult == DialogResult.No)   // Do nothing if user disagrees
                    {
                        return;
                    }
                }
            }
            else
            {
                // Incorrect answer
                countAnswers = answerLabel.Text.Length;
                if (countAnswers < 3)   // Student hasn't answered 3 times
                {
                    answerLabel.Text = answerLabel.Text + "-";  // Show false answer on the label
                    putAnswerIntoTextFile("-", index, false);
                    if (countAnswers == 2)
                    {
                        string resultingMark = getResultMark(index);  // Get the resulting mark based on answers
                        markLabel.Text = resultingMark;    // If student answered 3rd time, show the mark
                        putAnswerIntoTextFile(resultingMark, index, true);  // Save mark in the text file
                    }
                }
                else    // Student has already answered 3 times
                {
                    countAnswers = 0;
                    DialogResult dialogResult = MessageBox.Show("The student has answered 3 times. Do you want to remove answers?", "Regimark", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)   // Clear previous results if user agrees
                    {
                        answerLabel.Text = "";
                        markLabel.Text = "";
                        countAnswers = answerLabel.Text.Length;
                    }
                    else if (dialogResult == DialogResult.No)   // Do nothing if user disagrees
                    {
                        return;
                    }
                }
            }
        }

        public void putAnswerIntoTextFile(string answer, int indexStudent, bool deleteLastAnswers)
        {
            string filePath = marksPath;
            string[] lines;
            if (File.Exists(filePath))  // Check if file exists
            {
                if (deleteLastAnswers)
                    deleteLastAnswersInTextFile(indexStudent);  // Delete last answers if necessary
                lines = File.ReadAllLines(filePath);    // Get lines of the text file
                StreamWriter writer = new StreamWriter(filePath, false);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == indexStudent)
                    {
                        writer.WriteLine(lines[i] + " " + answer);  // Add answer to the line
                    }
                    else
                    {
                        writer.WriteLine(lines[i]); // Leave the rest the same
                    }
                }
                writer.Close();
            }
        }

        public bool isTheLastAnswerAMark (int studentIndex)
        {
            string filePath = marksPath;
            string[] lines;
            if (File.Exists(filePath))  // Check if file exitst
            {
                lines = File.ReadAllLines(filePath);    // Get the content line by line
                string stringToCheck = lines[studentIndex];  // Line that must be checked
                string[] marks = { "2", "3", "4", "5" };
                string[] strAnswers = { "+", "-" };
                for (int i = stringToCheck.Length-1; i >= 0; i--)
                {
                    for (int j = 0; j < marks.Length; j++)
                    {
                        if (stringToCheck[i].ToString() == marks[j])    // Check if it is mark
                            return true;
                    }
                    for (int k = 0; k < strAnswers.Length; k++)
                    {
                        if (stringToCheck[i].ToString() == strAnswers[k])   // Check if it is answer
                            return false;
                    }
                }
            }
            return false;
        }

        public string getLastAnswers (int index)
        {
            string strAnswer = "";
            string filePath = marksPath;
            string[] lines;
            if (File.Exists(filePath))  // Check if file exitst
            {
                lines = File.ReadAllLines(filePath);    // Get the content line by line
                string stringToCheck = lines[index];  // Line that must be checked
                int count = 0;
                int countLimit = 0;
                int[] digits = { 2, 3, 4, 5 };
                bool stopCounting = false;
                if (!stopCounting)
                {
                    for (int i = stringToCheck.Length - 1; i >= 0; i--)
                    {
                        if (!stopCounting)
                        {
                            for (int j = 0; j < digits.Length; j++)
                            {
                                if (!stopCounting)
                                {
                                    if ((stringToCheck[i] != digits[j]) & ((stringToCheck[i].ToString() == "+") | (stringToCheck[i].ToString() == "-")) & (countLimit <= 3*digits.Length))
                                        countLimit++;   // Find out how many answers must be deleted
                                    else if (stringToCheck[i].ToString() == digits[j].ToString())
                                        stopCounting = true;
                                }
                            }
                        }
                    }
                }
                countLimit /= digits.Length;
                for (int i = stringToCheck.Length-1; i >= 0; i--)
                {
                    if (count != countLimit)
                    {
                        if (stringToCheck[i] == '+')
                        {
                            count++;
                            strAnswer += "+";
                        }
                        if (stringToCheck[i] == '-')
                        {
                            count++;
                            strAnswer += "-";
                        }
                    }
                }
            }
            string reversedStr = "";
            for (int k = strAnswer.Length - 1; k >= 0; k--)
                reversedStr += strAnswer[k].ToString();
            return reversedStr;
        }

        public void deleteLastAnswersInTextFile(int indexOfStudent) // Function that deletes last answers in the text file
        {
            string filePath = marksPath;
            string[] lines;
            if (File.Exists(filePath))  // Check if file exitst
            {
                lines = File.ReadAllLines(filePath);    // Get the content line by line
                string stringToChange = lines[indexOfStudent];  // Line that must be changed
                char[] letters = stringToChange.ToCharArray();
                string[] newLetters = new string[letters.Length];
                bool[] lettersForRemoval = new bool[letters.Length];
                string newString = "";
                int count = 0;
                int countLimit = 0;
                int[] digits = { 2, 3, 4, 5 };
                bool stopCounting = false;
                if (!stopCounting)
                {
                    for (int i = stringToChange.Length - 1; i >= 0; i--)
                    {
                        if (!stopCounting)
                        {
                            for (int j = 0; j < digits.Length; j++)
                            {
                                if (!stopCounting)
                                {
                                    if ((stringToChange[i] != digits[j]) & ((stringToChange[i].ToString() == "+") | (stringToChange[i].ToString() == "-")))
                                        countLimit++;   // Find out how many answers must be deleted
                                    else if (stringToChange[i].ToString() == digits[j].ToString())
                                        stopCounting = true;
                                }
                            }
                        }
                    }
                }
                countLimit /= digits.Length;
                for (int i = stringToChange.Length-1; i >= 0; i--)
                {
                    if (count != countLimit)
                    {
                        if ((stringToChange[i] == '+') | (stringToChange[i] == '-'))
                        {
                            count++;
                            for (int j = 0; j < stringToChange.Length; j++)
                            {
                                if (j == i)
                                    lettersForRemoval[j] = true;    // Mark letters (answers) that must be removed
                            }
                        }
                    }
                    else if (count == countLimit)
                        break;
                }
                for (int n = 0; n < lettersForRemoval.Length; n++)
                {
                    if (lettersForRemoval[n])
                        newLetters[n] = ""; // Delete letters that were marked for deletion
                    else
                        newLetters[n] = letters[n].ToString();  // Leave the rest of letters
                }
                for (int k = 0; k < stringToChange.Length; k++)
                    newString += newLetters[k]; // Make a string of previously formed letters
                StreamWriter writer = new StreamWriter(filePath, false);
                for (int m = 0; m < lines.Length; m++)
                {
                    if (m == indexOfStudent)
                        writer.WriteLine(newString);    // Put new string instead of old one
                    else
                        writer.WriteLine(lines[m]); // Leave the rest
                }
                writer.Close();
            }
        }

        private string getResultMark(int index)   // Get resulting mark based on amount of correct answers
        {
            string strAnswers = getLastAnswers(index);
            char[] chrArrAnswers = strAnswers.ToCharArray();
            int[] intArrAnswers = new int[chrArrAnswers.Length];
            for (int i = 0; i < chrArrAnswers.Length; i++)
            {
                if (chrArrAnswers[i] == '+')
                    intArrAnswers[i] = 1;
                if (chrArrAnswers[i] == '-')
                    intArrAnswers[i] = -1;
                else if ((chrArrAnswers[i] != '+') & (chrArrAnswers[i] != '-'))
                    intArrAnswers[i] = 0;
            }
            int correctAnswerCount = 0;
            if (intArrAnswers.Length == 3)
            {
                for (int i = 0; i < intArrAnswers.Length; i++)
                {
                    if (intArrAnswers[i] == 1)
                        correctAnswerCount++;
                }
                return (correctAnswerCount + 2).ToString();
            }
            return "Error";
        }
    }
}