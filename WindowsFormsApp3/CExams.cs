using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    [Serializable]
    public class Cexam
    {
        byte subject;
        byte mark;
        public Cexam(byte subject, byte mark)
        {
            Mark = mark;
            Subject = subject;
        }
        public Cexam(string[] vs)
        {
            TryGetFromSTArray(vs);
        }
        public Cexam(string st) 
        {
            st = st.Trim();
            string[] res = st.Split(' ');
            TryGetFromSTArray(res);
        }
        public Cexam()
        {
            Subject = 0;
            Mark = 0;
        }

        public void TryGetFromSTArray(string[] vs)
        {
            if (vs.Length==2)
            {
                int pos1 = Array.IndexOf(StringEquivalents.SubjectNames, vs[0]), pos2 = Array.IndexOf(StringEquivalents.Marks, vs[1]);
                if (pos1 != -1 && pos2 != -1)
                {
                    subject = (byte)pos1;
                    mark = (byte)pos2;
                }
                else
                {
                    mark = 0;
                    subject = 0;
                }
            }
            else
            {
                mark = 0;
                subject = 0;
            }
        }
        public override string ToString()
        {
            return StringEquivalents.SubjectNames[(int)subject] + " " + StringEquivalents.Marks[(int)mark];
        }
       
        public byte Subject
        {
            get
            {
                return subject;
            }
            set
            {
                if (value >= 0 && value < StringEquivalents.SubjectNames.Length)
                    subject = value;
                else
                    subject = 0;
            }
        }
        public byte Mark
        {
            get
            {
                return mark;
            }
            set
            {
                if (value >= 0 && value < 5)
                    mark = value;
                else
                    mark = 0;
            }
        }

    }
    public enum EConditions {MustBeFilled, CanBeFilled, CanTBeFilled}
    [Serializable]
    public class CSemestr
    {
        Cexam[] cexams;
        [NonSerialized]
        public EConditions Condition; 
        public Cexam[] Exams
        {
            get
            {
                return cexams;
            }
            set
            {
                cexams = value;
            }
        }
        public CSemestr()
        {
            cexams = new Cexam[StringEquivalents.NExams];
            for (int i = 0; i < StringEquivalents.NExams; i++)
            {
                SetFilling(i, 1);
                cexams[i] = new Cexam(0, 0);
            }
                
        }
        public CSemestr(byte spec, int n, byte course)
        {
            cexams = new Cexam[StringEquivalents.NExams];
            SetFilling(n, course);
            for (int i = 0; i < StringEquivalents.NExams; i++)
            {
                cexams[i] = new Cexam((byte)((n + spec + i) % StringEquivalents.SubjectNames.Length), 0);
            }
        }
        public void SetFilling (int n, byte course)
        {
            switch(course)
            {
                case 1:
                    if (n ==0 || n==1)
                        Condition = EConditions.CanBeFilled;
                    else
                        Condition = EConditions.CanTBeFilled;
                    break;
                case 2:
                    if (n==0 || n==1)
                        Condition = EConditions.MustBeFilled;
                    else
                        if (n==2 || n==3)
                            Condition = EConditions.CanBeFilled;
                        else
                            Condition = EConditions.CanTBeFilled;
                    break;
                case 3:
                    if (n>=0 && n<=3)
                        Condition = EConditions.MustBeFilled;
                    else
                        if (n==4 || n==5) 
                        Condition = EConditions.CanBeFilled;
                        else
                            Condition = EConditions.CanTBeFilled;
                    break;
                case 4:
                    if (n >= 0 && n<=5)
                        Condition = EConditions.MustBeFilled;
                    else
                        Condition = EConditions.CanBeFilled;
                    break;
            }
        }

        public float GetAverageMark()
        {
            float res = 0;
            for (int i = 0; i < StringEquivalents.NExams; i++)
                res += this.cexams[i].Mark;
            return res / StringEquivalents.NExams;
        }

        public Cexam this[int i]
        {
            get
            {
                if (i >= 0 && i < StringEquivalents.NExams)
                    return cexams[i];
                else
                    return cexams[0];
            }
        }
        public void ChangeMarks (ref byte[] marks)
        {
            for (int i = 0; i < marks.Length; i++)
                cexams[i].Mark=marks[i];
        }
        public CSemestr Clone()
        {
            CSemestr res = new CSemestr();
            for (int i = 0; i < res.cexams.Length; i++) 
                res.cexams[i] = new Cexam(this.cexams[i].Subject, this.cexams[i].Mark);
            return res;
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < StringEquivalents.NExams; i++)
                res += cexams[i].ToString() + " ";
            return res;
        }
        public void Parse (string st)
        {
            //MessageBox.Show("In Parsing");
            st = st.Trim();
            string[] res = st.Split(' ');
            //MessageBox.Show("Res lenght = " + res.Length);
            if (res.Length==StringEquivalents.NExams*2)
            {
              //  MessageBox.Show("All right");
                for (int i = 0; i < StringEquivalents.NExams; i++)
                {
                    cexams[i].TryGetFromSTArray(new string[] { res[2*i], res[2*i + 1] });
                }
            }
        }
        public bool IsCorrect()
        {
            bool ok = true;
            int i = 0;
            switch (Condition)
            {
                case EConditions.CanBeFilled:
                    ok = true;
                    break;
                case EConditions.MustBeFilled:
                    while (i < StringEquivalents.NExams && ok)
                    {
                        ok = Exams[i].Mark != 0; ;
                        i++;
                    }   
                    break;
                case EConditions.CanTBeFilled:
                    while (i < StringEquivalents.NExams && ok)
                    {
                        ok = Exams[i].Mark == 0; ;
                        i++;
                    }
                    break;
            }
            return ok;
        }
    }


    [Serializable]
    public class CExams
    {
        CSemestr[] cexams;
        public CSemestr[] Semesters
        {
            get
            {
                return cexams;
            }
            set
            {
                cexams = value;
            }
        }
        public CExams()
        {
            cexams = new CSemestr[StringEquivalents.Nsemestrs];
            for (int i = 0; i < StringEquivalents.Nsemestrs; i++)
            {
                cexams[i] = new CSemestr(0, i, 1);
            }

        }
        public CExams(byte specialization, byte course)
        {
            cexams = new CSemestr[StringEquivalents.Nsemestrs];
            for (int i = 0; i < StringEquivalents.Nsemestrs; i++)
            {
                cexams[i] = new CSemestr(specialization, i, course);
            }
        }
        public CExams(string[] arr)
        {
            GetFromString(arr);
        }
        public CExams Clone()
        {
            CExams res = new CExams();
            for (int i = 0; i < res.cexams.Length; i++)
            {
                res.cexams[i] = this.cexams[i].Clone();

            }
            return res;
        }

        public Cexam this[int i, int j]
        {
            get
            {
                if (i >= 0 && i < StringEquivalents.Nsemestrs && j >= 0 && j < StringEquivalents.NExams)
                    return cexams[i][j];
                else
                    return cexams[0][0];
            }
        }
        public CSemestr this[int i]
        {
            get
            {
                if (i >= 1 && i <= StringEquivalents.Nsemestrs)
                    return cexams[i - 1];
                else
                    return cexams[0];
            }
        }
        public void ChangeMarks(ref byte[][] marks)
        {
            for (int i = 0; i < cexams.Length; i++)
            {
                this.cexams[i].ChangeMarks(ref marks[i]);
            }
        }

        public string GetSem(int s)
        {
            CSemestr sem = this[s];
            string res = "";
            for (int i = 0; i < StringEquivalents.NExams; i++)
            {
                res += sem[i].ToString() + " ";
            }
            return res;
        }
        public void GetFromString(string[] st)
        {
            //MessageBox.Show("In parsing Exams");
            if (st.Length == StringEquivalents.Nsemestrs)
            {
              //  MessageBox.Show("before for");
                for (int i = 0; i < StringEquivalents.Nsemestrs; i++)
                {
                //    MessageBox.Show("In for i = " + i.ToString());
                    cexams[i].Parse(st[i]);
                  //  MessageBox.Show("After for i = " + i.ToString());
                }
                    
            }
        }
        bool CheckMarks()
        {
            bool ok = true;
            for (int i = 0; i < StringEquivalents.Nsemestrs && ok; i++)
                ok = cexams[i].IsCorrect();
            return ok;
        }
    }
}