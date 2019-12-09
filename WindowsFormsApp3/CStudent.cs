using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;


namespace WindowsFormsApp3
{
    [Serializable] 
    public class CStudent
    {
        
        CFIO fIO;
        public CFIO FIO
        {
            get
            {
                return fIO;
            }
            set
            {
                fIO = value;
            }
        }

        byte ef;
        public byte ed
        {
            get
            {
                return ef;
            }
            set
            {
                if (value >=StringEquivalents.EdForms.Length || value<0)
                    ef = 0;
                else
                    ef = value;
            }
        }
        
        byte c;      
        public byte course
        {
            get
            {
                return c;
            }
            
            set
            {
                if (value < 1 || value > (StringEquivalents.Nsemestrs/2))
                    c = 1;
                else
                    c = value;
            }
        }

        byte sp;        
        public byte specialization
        {
            get
            {
                return sp;
            }
            set
            {
                if (value >= 0 && value < StringEquivalents.Specializations.Length)
                    sp = value;
                else
                    sp = 0;
            }
        }

        int g;
        public int Group {
            get
            {
                return g;
            }
            set
            {
                if (!StringEquivalents.Groupes.Contains(value.ToString()))
                    g = 1;
                else
                {
                    int p = Array.IndexOf(StringEquivalents.Groupes, value.ToString());
                    g = Int32.Parse(StringEquivalents.Groupes[p]);
                }
            }
        }
        public bool IsActive { get; set; }

        CExams Exams;    
        public CExams exams
        {
            get
            {
                return Exams;
            }
            set
            {
                Exams = value;
            }
        }

        bool tryParseMainInfo(string st)
        {
            bool ok;
            st = st.Trim();
            string[] res = st.Split(' ');
            ok = res.Length == 7;
            if (ok)
            {
                for (int i = 0; i < 3; i++)
                    FIO[i] = res[i];
                int pos = Array.IndexOf(StringEquivalents.EdForms, res[3]);

                ok = (pos != -1);
                if (ok)
                {
                    ed= (byte)pos;
                    byte t;
                    ok = Byte.TryParse(res[4], out t);
                    if (ok)
                    {
                        course = t;
                        pos = Array.IndexOf(StringEquivalents.Specializations, res[5]);
                        ok = pos != -1;
                        if (ok)
                        {
                            specialization = (byte)pos;
                            pos = Array.IndexOf(StringEquivalents.Groupes, res[6]);
                            ok = pos != -1;
                            if (ok)
                                Group = Int32.Parse(StringEquivalents.Groupes[pos]);
                        }
                    }
                }
            }
            return ok;
        }
        public CStudent(string main, string[] st) 
        {
            fIO = new CFIO();
            Exams = new CExams();
            IsActive = true;
            getFromstr(main, st);
        }

        public void getFromstr(string maininfo, string[] exams)
        {
            if (tryParseMainInfo(maininfo))
            {
               // MessageBox.Show("OKOK, l ="+exams.Length);
                this.exams.GetFromString(exams);
            }
        }

        public CStudent()
        {
            FIO = new CFIO();
            Group = StringEquivalents.DefaultGroupes[sp];
            c = 1;
            IsActive = true;
            exams = new CExams();
        }
        public CStudent (StreamReader reader):this()
        {
            string main = ReadMain(reader);
            //MessageBox.Show("Right");
            string[] buff = new string[StringEquivalents.Nsemestrs];
            ReadExams(reader, buff);
            getFromstr(main, buff);
            
        }
        public bool IsNotFailing()
        {
            int countOfSemestrs = (c- 1) * 2;
            int i = 2;
            bool ok = true;
            float pred = exams[1].GetAverageMark(), curr;
            while (i <= countOfSemestrs && ok)
            {
                curr = exams[i].GetAverageMark();
                ok = (curr - pred) >= 0;
                pred = curr;
                i++;
            }
            return ok;
        }
        public bool IsCorrect()
        {
            if (FIO[0]!= "" && FIO[1]!= "")
                return true;
            else
                return false;
        }

        string ReadMain(StreamReader reader)
        {
            return reader.ReadLine();
        }
        bool ReadExams(StreamReader reader,  string[] buff)
        {
            bool ok = true;
            int i = 0;
            for (i = 0; (!reader.EndOfStream) && (i < StringEquivalents.Nsemestrs); i++)
                buff[i] = reader.ReadLine();
            ok = i == StringEquivalents.Nsemestrs;
            return ok;
        }
        public string[] ToStrArr()
        {
            string[] res = new string[9];
            res[0] = FIO[0] + " " + FIO[1]+ " " + FIO[2]+ " " + StringEquivalents.EdForms[ef]+ " "
                + course.ToString()+ " " + StringEquivalents.Specializations[sp]+ " " + g.ToString();
            for (int i = 1; i < 9; i++)
            {
                res[i] = exams[i].ToString();
            }
            return res;
        }
        public CStudent Clone ()
        {
            CStudent res = new CStudent();
            res.fIO = this.fIO;
            res.ef = this.ef;
            res.c = this.c;
            res.sp = this.sp;
            res.g = this.g;
            res.Exams = this.Exams.Clone();
            return res;
        }

    }


    [Serializable]
    [XmlRoot("Students")]
    public class CStudents
    {
        [XmlArray("Collection"), XmlArrayItem("OneStudent")]
        public List<CStudent> Collection;
        public CStudents()
        {
            Collection = new List<CStudent>();
        }

        public void CleanList( )
        {
            for (int i = 0; i < Collection.Count; i++)
            {
                if (!((Collection.ElementAt(i)).IsActive))
                {
                    Collection.RemoveAt(i);
                    i--;
                }
            }
                
        }
        public bool IsAllCorrect()
        {
            bool res = true;
            int i = 0;
            while (i < Collection.Count && res)
            {
                res = Collection.ElementAt(i).IsCorrect();
                i++;
            }
            return res;
        }
        public CStudents(StreamReader reader):this()
        {
            AddFromFile(reader);
        }

        public void AddFromFile (StreamReader reader)
        {
            Collection.Clear();
            while (!reader.EndOfStream)
            {
                //MessageBox.Show("OK");
                Collection.Add(new CStudent(reader));
            }
        }
        public CStudents MainTAsk(byte c)
        {
            CStudents res = new CStudents();
            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection.ElementAt(i).course==c && 
                    (Collection.ElementAt(i)).IsNotFailing()
                    && Collection.ElementAt(i).IsActive)
                    res.Collection.Add(Collection.ElementAt(i).Clone());
            }
            return res;
        }
    }
}