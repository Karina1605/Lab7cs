using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WindowsFormsApp3
{
    [Serializable]
    public class CFIO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Pathronymic { get; set; }
        
        public CFIO(string[] Init)
        {
            InitByArr(Init);
        }
        public CFIO (string st)
        {
            st = st.Trim();
            string[] parced = st.Split(' ');
            InitByArr(parced);
        }
        public CFIO()
        {
            Default();
        }
        void InitByArr (string[] st)
        {   
            if (st.Length != 3)
                Default();
            else
                for (int i = 0; i < 3; i++)
                {
                    this[i] = st[i];
                }
        }

        void Default ()
        {
            LastName = FirstName = "";
            Pathronymic = "-";
        }

        public string this[int i]
        {
            get
            {
                switch(i)
                {
                    case 0: return LastName;
                    case 1: return FirstName;
                    case 2: return Pathronymic;
                    default: return "";
                }
            }
            set
            {
                switch(i)
                {
                    case 0: LastName=value;
                        break;
                    case 1: FirstName=value;
                        break;
                    case 2: Pathronymic = value;
                        break;
                    default: Default();
                        break;
                }
                
            }
        }
        
        public override string ToString()
        {
            string res = LastName + " " + FirstName + " " + Pathronymic;
            return res;
        }
    }
}
