using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp3
{
    
    static class StringEquivalents
    {
        public const int NExams = 5;
        public const int Nsemestrs = 8;
        public static readonly string[] Groupes = { "1","2", "3",
                                        "4", "5", "6", "61",
                                        "62", "71", "9", "91", "10" };
        public static readonly int[] DefaultGroupes =
        {
            71, 1, 10, 2, 61, 9
        };
        public static readonly string[] EdForms =
        {
            "Budget", "Contract"
        };
        public static readonly string[] SubjectNames = {
            "MA", "Pr", "LA",
            "IITGA", "Web", "DM", "Alg",
            "AG", "LSP",
            "H", "TP", "Eng",
            "DS", "MS", "DifEq", "Ec"
        };
        public static readonly string[] Specializations =
        {
            "BI", "MMM", "AIJ", "AMI", "FIIT", "MSAIS"
        };
        public static readonly string[] Marks =
        {
            "-", "2", "3", "4", "5"
        };
    }
}