using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp3
{
    public class COneRecord
    {
        Label idL;
        TextBox[] FIO;
        ComboBox[] HInfo;

         CStudent Student;
            
        Button ShowExams;
        Button DeleteRec;

        string pred;
        bool isActive=true;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }
       
        public COneRecord( CStudent student, int id)
        {
            Student = student;
            Student.IsActive = true;
            idL = new Label();
            idL.Text = id.ToString();
            idL.Font = new System.Drawing.Font("Times New Roman", 10);
            idL.Width = 50;
            idL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          
            pred = StringEquivalents.Specializations[(int)student.specialization];
            
            FIO = new TextBox[3];
            for (int i=0; i<3; i++)
            {
                FIO[i] = new TextBox();
                FIO[i].Width = 120;
                FIO[i].Multiline = false;
                FIO[i].Font = new System.Drawing.Font("Times New Roman", 10);
                FIO[i].Text = Student.FIO[i];
            }
            if (FIO[0].Text=="")
            {
                FIO[0].BackColor = System.Drawing.Color.Red;
            }
            if (FIO[1].Text == "")
            {
                FIO[1].BackColor = System.Drawing.Color.Red;
            }

            FIO[0].TextChanged += COneRecord_TextChanged;
            FIO[1].TextChanged += COneRecord_TextChanged6;
            FIO[2].TextChanged += COneRecord_TextChanged7;
            FIO[0].Leave += COneRecord_Leave1;
            FIO[1].Leave += COneRecord_Leave2;
            FIO[2].Leave += COneRecord_Leave;
            HInfo = new ComboBox[4];
            for (int i=0; i<4; i++)
            {
                HInfo[i] = new ComboBox();
                HInfo[i].Font = new System.Drawing.Font("Times New Roman", 10);
            }

            HInfo[0].Items.AddRange(StringEquivalents.EdForms);


            for (int i=1; i<=(StringEquivalents.Nsemestrs)/2; i++)
                HInfo[1].Items.Add(i);

            HInfo[2].Items.AddRange(StringEquivalents.Specializations);       
            
            HInfo[0].Width = 80;
            HInfo[1].Width = 50;
            HInfo[2].Width = 80;
            HInfo[3].Width = 50;
            

            HInfo[0].Text = StringEquivalents.EdForms[Student.ed];
            HInfo[1].Text = Student.course.ToString();
            HInfo[2].Text = StringEquivalents.Specializations[Student.specialization];
            HInfo[3].Text = Student.Group.ToString();
            Generate();

            HInfo[0].TextChanged += COneRecord_TextChanged5; 
            HInfo[1].TextChanged += COneRecord_TextChanged4; 
            HInfo[2].TextChanged += COneRecord_TextChanged2;
            HInfo[3].TextChanged += COneRecord_TextChanged3;
            

            ShowExams = new Button();
            if (student.FIO[0] == "" && student.FIO[1] == "" && student.FIO[2] == "")
                ShowExams.Text = "Fill Exams";
            else
                ShowExams.Text = "Show Exams";
            ShowExams.Width = 80;
            ShowExams.Padding = new Padding(4, 2, 0, 2);
            ShowExams.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ShowExams.Click += ShowExams_Click;

            DeleteRec = new Button();
            DeleteRec.Text = "Delete record";
            DeleteRec.Width = 80;
            DeleteRec.Padding = new Padding(4, 2, 0, 2);
            DeleteRec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            DeleteRec.Click += DeleteRec_Click;


            for (int i = 0; i < student.exams.Semesters.Length; i++)
                Student.exams.Semesters[i].SetFilling(i, student.course);
        }

        private void COneRecord_Leave2(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                ((TextBox)sender).BackColor = System.Drawing.Color.Red;
            }
            else
            {
                ((TextBox)sender).BackColor = System.Drawing.Color.White;
                
            }
        }

        private void COneRecord_Leave1(object sender, EventArgs e)
        {
           if (((TextBox)sender).Text=="")
           {
                ((TextBox)sender).BackColor = System.Drawing.Color.Red;

           }
           else
           {
                ((TextBox)sender).BackColor = System.Drawing.Color.White;
           }

        }

        private void COneRecord_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "-";
        }

        private void COneRecord_TextChanged7(object sender, EventArgs e)
        {
            Student.FIO[2] = FIO[2].Text;
            Form1.NeedSaving = true;

        }
        private void COneRecord_TextChanged6(object sender, EventArgs e)
        {
            if (((TextBox)sender).BackColor == System.Drawing.Color.Red)
            {
                ((TextBox)sender).BackColor = System.Drawing.Color.White;
            }
            Student.FIO[1] = FIO[1].Text;
            Form1.NeedSaving = true;

        }
        private void COneRecord_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).BackColor==System.Drawing.Color.Red)
            {
                ((TextBox)sender).BackColor = System.Drawing.Color.White;
            }
            Student.FIO[0] = FIO[0].Text;
            Form1.NeedSaving = true;
        }

        public void AddToPanel (Panel panel, int x, int y)
        {
            System.Drawing.Point loc = new System.Drawing.Point(x, y);
            idL.Location = loc;
            panel.Controls.Add(idL);

            loc.X += 50;
            for (int i = 0; i < 3; i++)
            {         
                FIO[i].Location = loc;
                panel.Controls.Add(FIO[i]);
                loc.X += 120;
            }

            HInfo[0].Location = loc;
            panel.Controls.Add(HInfo[0]);

            loc.X += 80;


            HInfo[1].Location = loc;
            panel.Controls.Add(HInfo[1]);

            loc.X += 50;


            HInfo[2].Location = loc;
            panel.Controls.Add(HInfo[2]);

            loc.X += 80;


            HInfo[3].Location = loc;
            panel.Controls.Add(HInfo[3]);

            loc.X += 50;
            ShowExams.Location = loc;
            panel.Controls.Add(ShowExams);

            loc.X += 80;
            DeleteRec.Location = loc;
            panel.Controls.Add(DeleteRec);

        }
        public void Dispose()
        {
            idL = null;
            for (int i = 0; i < 3; i++)
                FIO[i] = null;
            for (int i = 0; i < 4; i++)
                HInfo[i] = null;
            ShowExams = DeleteRec = null;
            pred = "";
            Student = null;
            isActive = false;
        }

        private void COneRecord_TextChanged5(object sender, EventArgs e)
        {
            int p = Array.IndexOf(StringEquivalents.EdForms, HInfo[0].Text);
            if (p== -1)
            {
                Student.ed = 0;
                HInfo[0].Text = "";
            }
            else
            {
                HInfo[0].Text = StringEquivalents.EdForms[p];
                Student.ed = (byte)p;
            }
            Form1.NeedSaving = true;
        }
        private void COneRecord_TextChanged4(object sender, EventArgs e)
        {
            byte k=0;
            if (byte.TryParse(HInfo[1].Text, out k) && (k>=1) &&(k<=StringEquivalents.Nsemestrs/2))
            {
                Student.course = k;
                for (int i = 0; i < StringEquivalents.Nsemestrs; i++)
                {
                    Student.exams.Semesters[i].SetFilling(i, k);
                }
            }
            else
            {
                HInfo[1].Text = "";
                Student.course = 1;
                for (int i = 0; i < StringEquivalents.Nsemestrs; i++)
                {
                    Student.exams.Semesters[i].SetFilling(i, 1);
                }
            }
            Form1.NeedSaving = true;
        }
        private void COneRecord_TextChanged3(object sender, EventArgs e)
        {
            int p = Array.IndexOf(StringEquivalents.Groupes, HInfo[3].Text);
            if (p==-1)
            {
                HInfo[3].Text = "";
                Student.Group = 1;
            }
            else
            {
                Student.Group = int.Parse(StringEquivalents.Groupes[p]);
            }
            Form1.NeedSaving = true;
        }
        private void ShowExams_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Fill Exams")
            {
                if (Array.IndexOf(StringEquivalents.Specializations, HInfo[2].Text)==-1 || HInfo[1].Text=="")
                    MessageBox.Show("There is not program of education", "Error", MessageBoxButtons.OK);
                else
                {
                    ShowEditMarks show = new ShowEditMarks(this);
                    if (!IsActive)
                        show.MakeUnaviable();
                    show.form.Show();
                    ((Button)sender).Text = "Show Exams";
                }
            }
            else
            {
                ShowEditMarks show = new ShowEditMarks(this);
                if (!IsActive)
                    show.MakeUnaviable();
                show.form.Show();
            }
        }
        private void DeleteRec_Click(object sender, EventArgs e)
        {
            if (isActive)
            {

                isActive = false;
                FIO[0].Enabled = FIO[1].Enabled = FIO[2].Enabled = false;
                for (int i = 0; i < 4; i++)
                    HInfo[i].Enabled = false;
                ShowExams.Enabled = false;
                Student.IsActive = false;
                ((Button)sender).Text = "Recover";
            }
            else
            {
                isActive = true;
                Student.IsActive = true;
                FIO[0].Enabled = FIO[1].Enabled = FIO[2].Enabled = true;
                for (int i = 0; i < 4; i++)
                    HInfo[i].Enabled = true;
                ShowExams.Enabled = true;
                ((Button)sender).Text = "Delete record";
            }
            Form1.NeedSaving = true;
        }
        
        //FIO
        private void COneRecord_TextChanged2(object sender, EventArgs e)
        {
            int pos;
            if (Array.IndexOf(StringEquivalents.Specializations, HInfo[2].Text)==-1 )
            {
                HInfo[2].Text = "";
                Student.specialization = 0;
                HInfo[3].Text = StringEquivalents.DefaultGroupes[Student.specialization].ToString();
                Generate();
                Form1.NeedSaving = true;
            }
                 
            else
            {
                if (pred != HInfo[2].Text)
                {
                    pos = Array.IndexOf(StringEquivalents.Specializations, HInfo[2].Text);
                    if (pos >= 0)
                    {
                        if (HInfo[1].Text != "")
                        {
                            byte t = byte.Parse(HInfo[1].Text);
                            Student.exams = new CExams((byte)pos, t);
                        }
                        else
                            Student.exams = new CExams((byte)pos, 1);
                        Student.specialization = (byte)(pos);
                    }
                    Form1.NeedSaving = true;
                }
                    
                if (ShowExams!=null && ShowExams.Text == "Show Exams" && pred != HInfo[2].Text)
                {
                    Form1.NeedSaving = true;
                    ShowExams.Text = "Fill Exams";
                    HInfo[3].Text = "";
                    //MessageBox.Show("Now =" + pred);
                }
                Generate();
                pred = HInfo[2].Text;
                HInfo[3].Text = StringEquivalents.DefaultGroupes[Student.specialization].ToString();
            }
            // MessageBox.Show(HInfo[2].Text + "Text pred =" + pred);
        }

        void Generate ()
        {
            if (HInfo[2].Text == "")
            {
                Student.Group = 1;
                HInfo[3].Items.Clear();
                HInfo[3].Text = "";
            }
            else
            {
                switch (HInfo[2].SelectedIndex)
                {
                    case 0:
                        HInfo[3].Items.Clear();
                        HInfo[3].Items.Add(StringEquivalents.Groupes[8]);
                        break;
                    case 1:
                        HInfo[3].Items.Clear();
                        HInfo[3].Items.Add(StringEquivalents.Groupes[0]);
                        break;
                    case 2:
                        HInfo[3].Items.Clear();
                        HInfo[3].Items.Add(StringEquivalents.Groupes[StringEquivalents.Groupes.Length - 1]);
                        break;
                    case 3:
                        HInfo[3].Items.Clear();
                        for (int i = 1; i < 6; i++)
                            HInfo[3].Items.Add(StringEquivalents.Groupes[i]);
                        break;
                    case 4:
                        HInfo[3].Items.Clear();
                        for (int i = 6; i < 8; i++)
                            HInfo[3].Items.Add(StringEquivalents.Groupes[i]);
                        break;
                    case 5:
                        HInfo[3].Items.Clear();
                        for (int i = 9; i < 11; i++)
                            HInfo[3].Items.Add(StringEquivalents.Groupes[i]);
                        break;
                }
                //HInfo[3].Text = "";
            }     
        }
        public string[] GetFIO ()
        {
            string[] res = new string[3];
            for (int i = 0; i < 3; i++)
                res[i] = FIO[i].Text;
            return res;
        }
        public byte GetFormOfEd()
        {
            int res=Array.IndexOf(StringEquivalents.EdForms, HInfo[0].Text);

            if (res!=-1)

                return (byte)res;
            else
                return 0 ;
        }
        public byte GetCourse()
        {
            byte res;
            if (Byte.TryParse(HInfo[1].Text, out res))
                return res;
            else
                return 1;
        }
        public byte GetSp()
        {
            int res=Array.IndexOf(StringEquivalents.Specializations, HInfo[2].Text);
            if (res!=-1)
                return (byte)res;
            else
                return 0;
                
        }
        public int GetGroup ()
        {
            byte res;
            if (Byte.TryParse(HInfo[3].Text, out res))
                return res;
            else
                return 1;
        }
        public CExams GetExams ()
        {
            return this.Student.exams;
        }
        public void MakeUnAviable ()
        {
            for (int i = 0; i < 3; i++)
                FIO[i].ReadOnly = true;
            for (int i = 0; i < 4; i++)
                HInfo[i].Enabled = false;
            DeleteRec.Visible = false;

        }
    }

    class ShowEditMarks
    {
        class MyGroupBox
        {
            GroupBox Main;
            Label[] Sub;
            ComboBox[] Results;
            EConditions condition;
            public MyGroupBox(COneRecord record, int index)
            {
                Main = new GroupBox();
                Main.Text = "Semestr " + (index + 1).ToString();
                
                condition = record.GetExams().Semesters[index].Condition;
                //MessageBox.Show("Comd" + (int)condition);
                System.Drawing.Point location;
                if (index < 4)
                    location = new System.Drawing.Point(5, 90 * (index + 1));
                else
                    location = new System.Drawing.Point(5 + 280, 90 * (index - 4 + 1));
                Main.Width = 270;
                Main.Height = 90;
                Main.Location = location;
                switch (condition)
                {
                    case EConditions.MustBeFilled:
                        Main.BackColor = System.Drawing.Color.FromArgb(127, 255, 255, 0);
                        break;
                    case EConditions.CanBeFilled:
                        Main.BackColor = System.Drawing.Color.FromArgb(127, 0, 0, 255);
                        break;
                    case EConditions.CanTBeFilled:
                        Main.BackColor = System.Drawing.Color.FromArgb(127, 128, 128, 128);
                        break;
                }
                System.Drawing.Point loc = new System.Drawing.Point(5, 25);
                System.Drawing.Point locm = new System.Drawing.Point(5, 50);
                Sub = new Label[StringEquivalents.NExams];
                Results = new ComboBox[StringEquivalents.NExams];
                for (int i = 0; i < StringEquivalents.NExams; i++)
                {
                    Sub[i] = new Label();
                    Results[i] = new ComboBox();
                    switch (condition)
                    {
                        case EConditions.MustBeFilled:
                        case EConditions.CanBeFilled:
                            Results[i].Enabled = true;
                            break;
                        case EConditions.CanTBeFilled:
                            Results[i].Enabled = false;
                            break;
                    }
                    Sub[i].Width = Results[i].Width = 40;
                    Sub[i].Text = StringEquivalents.SubjectNames[(int)(record.GetExams()[index, i].Subject)];
                    Results[i].Text = StringEquivalents.Marks[(int)record.GetExams()[index, i].Mark];
                    Sub[i].Font = Results[i].Font = new System.Drawing.Font("Times New Roman", 10);
                    Results[i].Items.AddRange(StringEquivalents.Marks);
                    Sub[i].Location = loc;
                    Results[i].Location = locm;
                    Results[i].TextChanged += ShowEditMarks_TextChanged;
                    Main.Controls.Add(Sub[i]);
                    Main.Controls.Add(Results[i]);
                    loc.X += 40;
                    locm.X += 40;
                }
            }
            private void ShowEditMarks_TextChanged(object sender, EventArgs e)
            {
                int p = Array.IndexOf(StringEquivalents.Marks, ((ComboBox)sender).Text);
                if (p == -1)
                    ((ComboBox)sender).Text = StringEquivalents.Marks[0];
                if (!Form1.NeedSaving)
                    Form1.NeedSaving = true;
            }
            public byte[] GetNewMarksArray()
            {
                byte[] res = new byte[StringEquivalents.NExams];
                for (int i=0; i<StringEquivalents.NExams; i++)
                {
                    int p =Array.IndexOf(StringEquivalents.Marks, Results[i].Text);
                    res[i] = (byte)p;
                }
                return res;
            }
            public GroupBox GetBox()
            {
                return Main;
            }
            public bool IsCorrect ()
            {
                bool ok=true;
                int i = 0;
                switch (condition)
                {
                    case EConditions.CanBeFilled:
                        ok=true;
                        break;
                    case EConditions.MustBeFilled:
                        while (i < StringEquivalents.NExams && ok)
                        {
                            ok = (Results[i].Text != "") && (Results[i].Text!="-");
                            i++;
                        }   
                        break;
                    case EConditions.CanTBeFilled:
                        while (i < StringEquivalents.NExams && ok)
                        {
                            ok = Results[i].Text == "-";
                            i++;
                        }
                        break;
                }
                return ok;
            }
            public void MakeUnAviable ()
            {
                for (int i = 0; i < StringEquivalents.NExams; i++)
                    Results[i].Enabled = false;
            }
        }

        static bool IsChanged = false;
        public Form form;
        Button OK;
        MyGroupBox[] semesters;
        Label[] info;
        TextBox[] cont;
        COneRecord record;
        public ShowEditMarks(COneRecord record)
        {
            this.record = record;
            form = new Form();
            form.Width = 610;
            form.Height = 560;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            InitCapt(record);
            InitGroupBoxes(record);
            OK = new Button();
            OK.Text = "OK";
            OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            OK.Font = new System.Drawing.Font("Times New Roman", 12);
            OK.Width = 70;
            OK.Height = 30;
            OK.Location = new System.Drawing.Point(5, 460);
            OK.Click += OK_Click;
            form.Controls.Add(OK);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (CheckMarks())
            {
                byte[][] marks = new byte[StringEquivalents.Nsemestrs][];
                for (int i = 0; i < StringEquivalents.Nsemestrs; i++)
                {
                    marks[i] = semesters[i].GetNewMarksArray();
                }
                record.GetExams().ChangeMarks(ref marks);
                form.Close();
            }
            else
            {
                MessageBox.Show("Incorrect marks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool CheckMarks ()
        {
            bool ok = true;
            int i = 0;
            while ((i < StringEquivalents.Nsemestrs) && ok)
            {
                ok = semesters[i].IsCorrect();
                i++;
            }   
            return ok;
        }

        void InitCapt(COneRecord record)
        {   
            form.Name = "Show/edit exams";
            info = new Label[4];
            cont = new TextBox[4];
            for (int i = 0; i < 4; i++)
            {
                info[i] = new Label();
                cont[i] = new TextBox();
                info[i].Font = cont[i].Font = new System.Drawing.Font("Times New Roman", 10);
                info[i].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                cont[i].TextAlign = HorizontalAlignment.Left;

            }
            info[0].Text = "FIO";
            info[1].Text = "Course";
            info[2].Text = "Specialization";
            info[3].Text = "Group";

            cont[0].Text = string.Join(" ", record.GetFIO());
            cont[1].Text = record.GetCourse().ToString();
            cont[2].Text = (StringEquivalents.Specializations[(int)record.GetSp()]);
            cont[3].Text = record.GetGroup().ToString();
            for (int i = 0; i < 4; i++)
                cont[i].ReadOnly = true;
            info[0].Width = cont[0].Width = 300;
            info[1].Width = cont[1].Width = 70;
            info[2].Width = cont[2].Width = 100;
            info[3].Width = cont[3].Width = 70;
            System.Drawing.Point loc = new System.Drawing.Point(5, 5);
            System.Drawing.Point loct = new System.Drawing.Point(5, 25);

            info[0].Location = loc;
            cont[0].Location = loct;
            loc.X += 300; loct.X += 300;

            info[1].Location = loc;
            cont[1].Location = loct;
            loc.X += 70; loct.X += 70;

            info[2].Location = loc;
            cont[2].Location = loct;
            loc.X += 100; loct.X += 100;

            info[3].Location = loc;
            cont[3].Location = loct;
            for (int i = 0; i < 4; i++)
            {
                form.Controls.Add(info[i]);
                form.Controls.Add(cont[i]);
            }
                
        }
        void InitGroupBoxes (COneRecord record)
        {
            semesters = new MyGroupBox[StringEquivalents.Nsemestrs];
            for (int i=0; i<StringEquivalents.Nsemestrs; i++)
            {
                InitGroupBox(record, i);
            }
        }
        void InitGroupBox (COneRecord record, int index)
        {
            semesters[index] = new MyGroupBox(record, index);
            form.Controls.Add(semesters[index].GetBox());
        }

        public void MakeUnaviable ()
        {
            for (int i = 0; i < StringEquivalents.Nsemestrs; i++)
                semesters[i].MakeUnAviable();
        }
       
    }
    class CVisualization
    {
       
        public int count;
        public List<COneRecord> list { get; set; }
        public List<CStudent> Source { get; set; }
        public Panel panel;
        public Button add;
        Label[] labels;
        void InitCaption()
        {
            labels = new Label[9];
            System.Drawing.Point pointL = new System.Drawing.Point(0, 0);
            for (int i = 0; i < 9; i++)
            {
                labels[i] = new Label();
                labels[i].Font = new System.Drawing.Font("Times New Roman", 12);
                labels[i].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            }

            labels[0].Text = "№";
            labels[0].Width = 50;
            panel.Controls.Add(labels[0]);
            pointL.X += 50;



            labels[1].Text = "Last Name";
            labels[2].Text = "First Name";
            labels[3].Text = "Pathronymic";
            labels[1].Width = labels[2].Width = labels[3].Width = 120;
            for (int i = 1; i < 4; i++)
            {
                labels[i].Location = pointL;
                panel.Controls.Add(labels[i]);
                pointL.X += 120;
            }

            labels[4].Text = "Form Of Education";
            labels[4].Location = pointL;
            panel.Controls.Add(labels[4]);
            pointL.X += 80;

            labels[5].Text = "Course";
            labels[5].Location = pointL;
            panel.Controls.Add(labels[5]);
            pointL.X += 50;
            labels[6].Text = "Specialization";
            labels[6].Location = pointL;
            panel.Controls.Add(labels[6]);
            pointL.X += 80;
            labels[7].Text = "Group";
            labels[7].Location = pointL;
            panel.Controls.Add(labels[7]);
            pointL.X += 50;
            labels[4].Width = labels[6].Width = labels[8].Width = 80;
            labels[5].Width = labels[7].Width = 50;

            labels[8].Text = "Marks";
            labels[8].Location = pointL;
            panel.Controls.Add(labels[8]);
            pointL.X += 80;
        }
        
        public CVisualization(List<CStudent> source, System.Drawing.Point point)
        {
            panel = new Panel();
            panel.Location = point;
            panel.Width = 880;
            panel.Height = 630;

            this.Source = source ;
          
            panel.MaximumSize = new System.Drawing.Size(880, 640);
            panel.AutoScroll = true;

            panel.AutoScroll = true;

            InitCaption();

            list = new List<COneRecord>();
            count = 0;
            add = new Button();
            add.Height = 30;
            add.Width = 120;
            add.Text = "Add student";
            add.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            add.Location = new System.Drawing.Point(920, 40);
            add.Location = new System.Drawing.Point(point.X + 880, 60);
            add.Click += Add_Click;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (count >= 20) 
               panel.VerticalScroll.Value = panel.VerticalScroll.Minimum;
            Add(true);
            Form1.NeedSaving = true;
        }

        public void Add (bool IsActive)
        {
            count++;
            CStudent Nst = new CStudent();
            Source.Add(Nst);
            COneRecord record = new COneRecord(Nst, count);
            record.IsActive = IsActive;
            list.Add(record);
            record.AddToPanel(this.panel, 0, 28*count);
        }
        public void Add(CStudent student, bool IsActive)
        {
            count++;
            COneRecord record = new COneRecord(student, count);
            record.IsActive = IsActive;
            list.Add(record);
            record.AddToPanel(this.panel, 0, 28*count);
        }
        public void Display( bool IsAviable)
        { 
            for (int i = 0; i < Source.Count; i++)
            {
                Add((Source).ElementAt(i), IsAviable);
            }          
        }
        public void DisplayOnForm (Form form)
        {
            form.Controls.Add(panel);
            form.Controls.Add(add);
        }
        public void Clean()
        {
            for (int i = 0; i < list.Count; i++)
                list.ElementAt(i).Dispose();
            list.Clear();
            this.panel.Controls.Clear();
            this.count = 0;
            InitCaption();
        }
        public void MakeUnAviable ()
        {
            for (int i = 0; i < list.Count; i++)
                (list.ElementAt(i)).MakeUnAviable();
            add.Visible = false;
        }
    }
}