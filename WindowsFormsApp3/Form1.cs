using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        CVisualization visualization;
        CStudents students;
        WorkWithFile work;
        Form HelpWindow;
        public static bool NeedSaving = false;
        bool IsOpened = false;

        public Form1()
        {
            InitializeComponent();
            students = new CStudents();
            visualization = new CVisualization(students.Collection, new Point(40, 40));
            work = new WorkWithFile();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpWindow = new Form();
            HelpWindow.FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpWindow.Width = 400;
            HelpWindow.Height = 600;
            TextBox Spec = new TextBox();
            Spec.Multiline = true;
            Spec.Height = 200;
            Spec.Width = 380;
            Spec.Location = new System.Drawing.Point(5, 10);
            Spec.ReadOnly = true;
            Spec.Text = File.ReadAllText(@"C:\Users\Пользователь\Desktop\Лабы c#\7\WindowsFormsApp3\WindowsFormsApp3\SpecAbbr.txt");
            HelpWindow.Controls.Add(Spec);
            TextBox Sub = new TextBox();
            Sub.Multiline = true;
            Sub.Height = 250;
            Sub.Width = 380;
            Sub.Location = new System.Drawing.Point(5, 230);
            Sub.ReadOnly = true;
            Sub.Text = File.ReadAllText(@"C:\Users\Пользователь\Desktop\Лабы c#\7\WindowsFormsApp3\WindowsFormsApp3\Abbr.txt");
            HelpWindow.Controls.Add(Sub);
            Button exit = new Button();
            exit.Text = "OK";
            exit.Font = new Font("Times New Roman", 10);
            exit.Click += Exit_Click;
            exit.TextAlign = ContentAlignment.MiddleCenter;
            HelpWindow.Controls.Add(Spec);
            HelpWindow.Controls.Add(Sub);
            HelpWindow.Controls.Add(exit);
            exit.Location = new Point(10, 500);
            HelpWindow.Show();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            HelpWindow.Close();
        }

        private void openExsistingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool close = true;
            if (NeedSaving)
            {
                DialogResult result = MessageBox.Show("Save Current File?", "Saving", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        if (work.PutInfoToFile(students, WorkWithFile.Ext, NeedSaving))
                            NeedSaving = false;
                        else
                            close = false;
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        close = false;
                        break;
                }
                
            }
            if (close || !NeedSaving)
            {
                WorkWithFile.ChoseFileDialog();
                if (WorkWithFile.FileName == "")
                {

                }
                else
                {
                    if (work.GetInfoFromFile(WorkWithFile.Ext, ref students))
                    {
                        visualization.Clean();
                        visualization.Source = students.Collection;
                        visualization.Display(true);
                        visualization.DisplayOnForm(this);
                        this.Text = WorkWithFile.FileName;
                        NeedSaving = false;
                        IsOpened = true;
                    }
                    else
                    {
                        MessageBox.Show("File was corrupted", "Incorrect file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        visualization.Clean();
                        visualization.Source = students.Collection;
                    }   
                }
            }

        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool close = true;
            if (NeedSaving)
            {
                DialogResult result = MessageBox.Show("Save Current File?", "Saving", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        work.PutInfoToFile(students, WorkWithFile.Ext, NeedSaving);
                        NeedSaving = false;
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        close = false;
                        break;
                }
            }
            if (close || !NeedSaving)
            {
                visualization.list.Clear();
                visualization.Source.Clear();
                WorkWithFile.FileName = "New List";
                this.Text = WorkWithFile.FileName;
                visualization.Clean();
                visualization.DisplayOnForm(this);
                work = new WorkWithFile();
                NeedSaving = true;
                IsOpened = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ext = Path.GetExtension(WorkWithFile.FileName);
            if (ext == String.Empty)
            {
                if (WorkWithFile.SaveFileDialog())
                {
                    ext = Path.GetExtension(WorkWithFile.FileName);
                    work.PutInfoToFile(students, ext, NeedSaving);
                }
            }
            else
                work.PutInfoToFile(students, ext, NeedSaving);
            NeedSaving = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WorkWithFile.SaveFileDialog())
            {
                string ext = Path.GetExtension(WorkWithFile.FileName);
                work.PutInfoToFile(students, ext, NeedSaving);
                NeedSaving = false;
            }
        }

        private void makeTheTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsOpened)
            {
                byte c=0;
                bool ok=true;
                do {
                    string course= Interaction.InputBox("Input The course");
                    ok = (course == "" || byte.TryParse(course, out c));
                    if (!ok)
                        c = 0;
                } while (!ok);
                if (c!=0)
                {
                    CStudents res =students.MainTAsk( c);
                    Form ResForm = new Form();
                    ResForm.Width = 830;
                    ResForm.Height = 600;
                    ResForm.MaximumSize = ResForm.MinimumSize = new Size(830, 600);
                    CVisualization resV = new CVisualization(res.Collection, new Point(40, 40));
                    resV.Display( false);
                    resV.MakeUnAviable();
                    resV.DisplayOnForm(ResForm);
                    ResForm.Show();
                } 
            }
            else
                MessageBox.Show("You did not chose a file");
        }
    }
}
