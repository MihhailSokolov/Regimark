using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Regimark
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)   // Go to classForm1
        {
            startFileDialog2();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) // Exit program
        {
            Application.Exit();
        }

        private string filePathClass;

        private string marksFileName;
        
        public string[] listOfStudents  // Get the list of students
        {
            get
            {
                if (filePath == null | filePath == "")
                {
                    if (studentList == null)
                    {
                        MessageBox.Show("Firstly, choose file with the list of students", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        startFileDialog1();
                    }
                }
                else
                {
                    if (File.Exists("C:\\Users\\" + Environment.UserName + "\\Regimark\\filePathClass.txt"))
                        studentList = File.ReadAllLines(File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Regimark\\filePathClass.txt"));
                }
                string[] readyListOfStudents = studentList;
                char chr = '-';
                readyListOfStudents = readyListOfStudents.Where(val => val != "").ToArray();
                studentList = readyListOfStudents;
                readyListOfStudents = readyListOfStudents.Where(val => val[0] != chr).ToArray();
                return readyListOfStudents;
            }
        }

        public int[] numberOfStudentsInRows
        {
            get
            {
                string[] readyListOfStudents = studentList;
                char chr = '-';
                readyListOfStudents = readyListOfStudents.Where(val => val != "").ToArray();
                studentList = readyListOfStudents;
                readyListOfStudents = readyListOfStudents.Where(val => val[0] != chr).ToArray();
                int amountOfRows = 1;
                for (int i = 0; i < studentList.Length; i++)
                {
                    if (studentList[i][0] == chr)
                        amountOfRows++;
                }
                int[] amountOfStudentsInRow = new int[amountOfRows];
                int elementsCounter = 0;
                int studentsCounter = 1;
                for (int i = 0; i < studentList.Length; i++)
                {
                    if (studentList[i][0] == chr)
                    {
                        amountOfStudentsInRow[elementsCounter] = studentsCounter - 1;
                        elementsCounter++;
                        studentsCounter = 0;
                    }
                    studentsCounter++;
                }
                int c = 0;
                for (int i = 0; i < amountOfStudentsInRow.Length - 1; i++)
                    c += amountOfStudentsInRow[i];
                amountOfStudentsInRow[amountOfStudentsInRow.Length - 1] = studentList.Length - (amountOfRows - 1) - c;
                return amountOfStudentsInRow;
            }
        }
        
        private string path;

        private string getPath (string fullPath) // Extract and return only directory path without file
        {
            string path = "";
            string[] directory = fullPath.Split('\\').ToArray();
            for (int i = 0; i < (directory.Length - 1); i++)
                path = path + directory[i] + "\\";
            return path;
        }

        public string filePath
        {
            get
            {
                if (path != "" & path != null)
                {
                    return path;
                }
                else
                {
                    string tempPath = "C:\\Users\\" + Environment.UserName + "\\Regimark";
                    string tempFilePath = tempPath + "\\" + "filePathClass.txt";
                    if (Directory.Exists(tempPath))
                        if (File.Exists(tempFilePath))
                        {
                            filePathClass = File.ReadAllText(tempFilePath);
                            string newPath = getPath(File.ReadAllText(tempFilePath));
                            return newPath;
                        }
                        else
                            return "";
                    else
                        return "";
                }
            }
            set
            {
                if (filePath != "" | path != null)
                    path = filePath;
            }
        }
        private void startFileDialog1()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
        }

        private void referenceButton1_Click(object sender, EventArgs e)
        {
            startFileDialog1();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (filePath == "")
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
                textBox1.Text = filePath;
            }
            else if (filePath != "" & filePath != null)
            {
                radioButton2.Checked = false;
                radioButton1.Checked = true;
                textBox1.Text = filePathClass;
                listFileLoaded = true;
            }
            string pathOfMarksFile = getPath(getMarksFilePath());
            if (pathOfMarksFile != "")
            {
                textBox4.Text = pathOfMarksFile;
                marksFilePath = pathOfMarksFile;
                marksPathChosen = true;
            }
            else
            {
                if (textBox1.Text != "")
                {
                    marksFilePath = getPath(textBox1.Text);
                    textBox4.Text = marksFilePath;
                    marksPathChosen = true;
                }
            }
        }

        private string getMarksFilePath()
        {
            string userPath = "C:\\Users\\" + Environment.UserName;
            if (!(Directory.Exists(userPath + "\\Regimark")))
                Directory.CreateDirectory(userPath + "\\Regimark");
            string folderPath = userPath + "\\Regimark\\";
            string newFilePath = folderPath + "marksPath.txt";
            string marksPath = "";
            if (File.Exists(newFilePath))
                marksPath = File.ReadAllText(newFilePath);
            return marksPath;
        }

        private string getDateName()
        {
            string date = "";
            DateTime today = DateTime.Today;
            date = today.ToString("dd-MM-yyyy");
            return date + ".txt";
        }

        private void createTextFile(string pathOfFile)
        {
            if (pathOfFile != null & pathOfFile != "")
            {
                if (File.Exists(pathOfFile))
                {
                    string userPath = "C:\\Users\\" + Environment.UserName;
                    if (!(Directory.Exists(userPath + "\\Regimark")))
                        Directory.CreateDirectory(userPath + "\\Regimark");
                    string folderPath = userPath + "\\Regimark\\";
                    string newFilePath = folderPath + "filePathClass.txt";
                    File.WriteAllText(newFilePath, pathOfFile);
                }
            }
        }

        private string[] studentList;

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StreamReader reader = new StreamReader(openFileDialog1.OpenFile());
            string[] stringList = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray());
            filePathClass = openFileDialog1.FileName;
            path = getPath(filePathClass);
            createTextFile(filePathClass);
            studentList = new string[(stringList.Length + 1) / 2];
            int a = 0;
            for (int i = 0; i < stringList.Length; i++)
            {
                if (stringList[i] != "")
                {
                    studentList[a] = stringList[i];
                    a++;
                }
            }
            textBox1.Text = filePathClass;
            listFileLoaded = true;
            if (textBox4.Text == "")
            {
                textBox4.Text = getPath(filePathClass);
                marksFilePath = getPath(filePathClass);
            }
            if (listFileLoaded & marksFileCreated)
                button2.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (textBox1.Text != "")
                {
                    textBox1.Enabled = true;
                    label4.Enabled = true;
                    referenceButton1.Enabled = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox1.Enabled = false;
                label4.Enabled = false;
                referenceButton1.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox2.Enabled = true;
                label5.Enabled = true;
                classButton1.Enabled = true;

            }
            else
            {
                textBox2.Enabled = false;
                label5.Enabled = false;
                classButton1.Enabled = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                label6.Enabled = true;
                button3.Enabled = true;
                textBox4.Enabled = true;
                label3.Enabled = true;
                textBox3.Enabled = true;
                button1.Enabled = true;
                checkBox1.Enabled = true;
            }
            else
            {
                label6.Enabled = false;
                button3.Enabled = false;
                textBox4.Enabled = false;
                label3.Enabled = false;
                textBox3.Enabled = false;
                button1.Enabled = false;
                checkBox1.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            marksFilePath = folderBrowserDialog1.SelectedPath;
            textBox4.Text = marksFilePath;
            marksPathChosen = true;
        }

        private void startFileDialog2()
        {
            if (openFileDialog2.ShowDialog() == DialogResult.Cancel)
                return;
        }

        bool marksPathChosen = false;
        bool marksNameChosen = false;

        private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            fullMarksPath = openFileDialog2.FileName;

            if (areTheListsSimilar(fullMarksPath))
            {
                textBox2.Text = fullMarksPath;
                marksFileCreated = true;
                if (listFileLoaded & marksFileCreated)
                    button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("The list with names of students and the list with marks are different. Choose other file for marks or create new file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool areTheListsSimilar(string pathOfMakrsList)
        {
            string[] marksNameList = File.ReadAllLines(pathOfMakrsList);
            string[] studentsNameList = listOfStudents;
            return marksNameList.Length == studentsNameList.Length;
        }

        public string marksFilePath;
        public string fullMarksPath;

        public string marksPath
        {
            get
            {
                if (fullMarksPath != "" & fullMarksPath != null)
                    return fullMarksPath;
                else
                { MessageBox.Show("null fullMarksPath", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return "Error"; }
            }
        }

        private bool marksFileCreated;
        private bool listFileLoaded;

        private bool checkForBackslash (string str)
        {
            bool result = true;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\\')
                    return false;
            }
            return result;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (marksPathChosen & marksNameChosen)
            {
                if (textBox3.Text != "")
                    marksFileName = textBox3.Text;
                if (marksFileName != "" & marksFilePath != "")
                    fullMarksPath = marksFilePath + marksFileName;
                if (fullMarksPath.Substring(fullMarksPath.Length - 4, 4) == ".txt")
                {
                    if (checkForBackslash(marksFileName))
                    {
                        marksFileCreated = true;
                        string[] studentList = listOfStudents;
                        using (StreamWriter sw = File.CreateText(fullMarksPath))
                        {
                            for (int i = 0; i < studentList.Length; i++)
                                sw.WriteLine(studentList[i]);
                        }
                        writeMarksFilePath(fullMarksPath);
                        if (listFileLoaded & marksFileCreated)
                            button2.Enabled = true;
                    }
                    else
                        MessageBox.Show(@"File name can't contain simbol '\'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("File name must contain file format (.txt)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (marksPathChosen & !marksNameChosen)
                    MessageBox.Show("Incorrect file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!marksPathChosen & marksNameChosen)
                    MessageBox.Show("Select a folder where to create a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!marksPathChosen & !marksNameChosen)
                    MessageBox.Show("Select a folder where to create a file and choose a file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void writeMarksFilePath (string pathOfFile)
        {
            if (File.Exists(pathOfFile))
            {
                string userPath = "C:\\Users\\" + Environment.UserName;
                if (!(Directory.Exists(userPath + "\\Regimark")))
                    Directory.CreateDirectory(userPath + "\\Regimark");
                string folderPath = userPath + "\\Regimark\\";
                string newFilePath = folderPath + "marksPath.txt";
                File.WriteAllText(newFilePath, pathOfFile);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listFileLoaded & marksFileCreated & (fullMarksPath != "" & fullMarksPath != null))
            {
                classForm1 classForm = new classForm1();
                classForm.Show();
                this.Hide();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
                marksNameChosen = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox3.Text = getDateName();
            else
                textBox3.Text = "";
        }
    }
}