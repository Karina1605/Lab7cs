using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApp3
{
    class WorkWithFile
    {
        static string pred;
        public static string FileName;
        public static string Ext = "";

        public static void ChoseFileDialog ()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;
            openFile.Filter ="Текстовые файлы (*.txt)|*.txt|XML файлы (*.xml)|*.xml|BIN файлы (*.bin)|*.bin";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pred = FileName;
                FileName = openFile.FileName;
                Ext = Path.GetExtension(FileName);
            }
            else
            {
                FileName = "";
                Ext = ".txt";
            }
                
        }
        public static bool SaveFileDialog()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Текстовые файлы (*.txt)|*.txt|XML файлы (*.xml)|*.xml|BIN файлы (*.bin)|*.bin";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFile.FileName;
                Ext = Path.GetExtension(FileName);
                return true;
            }
            else
            {
                FileName = "";
                return false;
            }
        }
        
        public WorkWithFile(){ }
        public bool GetInfoFromFile (string ext, ref CStudents cs)
        {
            bool res = true;
            if (cs == null)
                cs = new CStudents();
            else
                cs.Collection.Clear();
            ext = ext.Trim();
            ext = ext.ToLower();
            switch (ext)
            {
                case ".bin":
                    WorkWithBinary work = new WorkWithBinary();
                    res = work.GetAllFromFile(ref cs);
                    break;
                case ".xml":
                    WorkWithXml xml = new WorkWithXml();
                    res = xml.GetAllFromFile(ref cs);
                    break;
                case ".txt":
                    WorkWithText text = new WorkWithText();
                    res = text.GetAllFromFile(ref cs);
                    break;
            }
            return res;
        }
        public bool PutInfoToFile (CStudents cs, string ext, bool Save)
        {
            cs.CleanList();
            if (!cs.IsAllCorrect())
            {
                MessageBox.Show("You did not filled all fields", "Uncorrect data", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Save = true;
                return false;
            }
            else
            {
                ext = ext.Trim();
                ext = ext.ToLower();
                switch (ext)
                {
                    case ".bin":
                        WorkWithBinary work = new WorkWithBinary();
                        work.PutAllToFile(cs);
                        break;
                    case ".xml":
                        WorkWithXml xml = new WorkWithXml();
                        xml.PutAllToFile(cs);
                        break;
                    case ".txt":
                    default:
                        WorkWithText text = new WorkWithText();
                        text.PutAllToFile(cs);
                        break;
                }
                Save = false;
                return true;
            }
            
        } 

        

        public class WorkWithBinary
        {
            BinaryFormatter formatter;
            public void PutAllToFile( CStudents c)
            {
                formatter = new BinaryFormatter();
                using (FileStream file = new FileStream(FileName, FileMode.OpenOrCreate))
                { 
                    formatter.Serialize(file, c);
                }
            }
            public bool GetAllFromFile(ref CStudents cs)
            {
                formatter = new BinaryFormatter();
                FileStream file = null;
                bool res = true ;
                try
                {
                    file = new FileStream(FileName, FileMode.Open);
                    cs = (CStudents)formatter.Deserialize(file);
                }
                catch
                {
                    res = false;
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
               return res;
            }
        }
        public class WorkWithXml
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CStudents));
            public void PutAllToFile(CStudents c)
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Create))
                {
                    serializer.Serialize(fs, c);
                }   
            }
            public bool GetAllFromFile(ref CStudents s)
            {
                bool res = true;
                XmlSerializer xml = new XmlSerializer(typeof(CStudents));
                FileStream fs = null;
                try
                {
                    fs = new FileStream(FileName, FileMode.Open);
                    s = (CStudents)xml.Deserialize(fs);
                }
                catch
                {
                    res = false;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
                return res;
            }
        }
        public class WorkWithText
        {
            public void PutAllToFile (CStudents cs)
            {
                string[] buff;
                using (StreamWriter file = new StreamWriter(FileName, false ))
                {
                    for (int i=0; i<cs.Collection.Count; i++)
                    {
                        buff = (cs.Collection.ElementAt(i)).ToStrArr();
                        for (int j = 0; j < 9; j++)
                            file.WriteLine(buff[j]);
                    }
                }
            }
            public bool GetAllFromFile (ref CStudents c)
            {
                string[] buff = new string[StringEquivalents.Nsemestrs];
                bool res = true;
                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(FileName, false);
                    c.AddFromFile(reader);
                }
                catch
                {
                    res = false;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
                return res;
            }
        }
    }
}