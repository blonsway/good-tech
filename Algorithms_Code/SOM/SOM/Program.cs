/*Author: Ravi Nagendra
 * 
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using Excel=Microsoft.Office.Interop.Excel;
using System.ComponentModel;

namespace WordListGenerator
{
    using VectorData = Dictionary<int, List<int>>;
    
    class WordList
    {   
        public static string currentFilePath = "";
        public static List<String> wordList = new List<String>();
        public static String[,] matrixList;
        public static List<String[,]> documentsMatrix= new List<String[,]>();
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        public static String filename = "finalTaxonomy.xlsx";
        public static VectorData finalVectorTable = new VectorData();
        public static bool InitializeExcel()
        {
            bool excelSheetPresent = true;
            MyApp = new Excel.Application();
            MyApp.Visible = false;
            currentFilePath = System.IO.Directory.GetCurrentDirectory() + "\\" + filename;            
            MyBook = MyApp.Workbooks.Open(currentFilePath);

            if (MyBook == null)
            {
                return false;
            }
            MySheet = (Excel.Worksheet)MyBook.Sheets[1];
            return excelSheetPresent;
        }
        public static void ReadExcel()
        {
            String appendFileContents = "";
            bool uniqueString = true;
            for (int index = 2; index <= 59; index++)
            {
               String currentString = (String)MySheet.get_Range("S" + index.ToString(), "S" + index.ToString()).Cells.Value;
               appendFileContents = appendFileContents+currentString;
            }

            String[] currentWordsList = appendFileContents.Split(' ');

            foreach (string st in currentWordsList)
            {
                for (int i = 0; i < wordList.Count; i++)
                {
                    if (wordList[i].CompareTo(st) == 0)
                    {
                        uniqueString = false;
                        break;
                    }
                }
                if (uniqueString)
                {
                    wordList.Add(st);
                }
                else
                {
                    //String is not unique so not adding
                }
                uniqueString = true;
            }                               
        }

        public static String FindElementPosition(string inputString,string rowElement,string columnElement)
        {
            int cooccurentCount=0;
            String[] currentWordsList = inputString.Split(' ');

            for (int i = 0; i < currentWordsList.Count();i++)
            {
                if (i==0)
                {
                    if (currentWordsList[i].CompareTo(rowElement) == 0)
                    {
                        if (currentWordsList[i + 1].CompareTo(columnElement) == 0)
                        {
                            cooccurentCount++;
                        }
                    }
                }
                else if(i==currentWordsList.Count()-1)
                {
                    if (currentWordsList[i].CompareTo(rowElement) == 0)
                    {
                        if (currentWordsList[i - 1].CompareTo(columnElement) == 0)
                        {
                            cooccurentCount++;
                        }
                    }
                }
                else
                {
                    if (currentWordsList[i].CompareTo(rowElement) == 0)
                    {
                        if (currentWordsList[i - 1].CompareTo(columnElement) == 0
                            || currentWordsList[i + 1].CompareTo(columnElement) == 0)
                        {
                            cooccurentCount++;
                        }
                    }
                }
            }

            return cooccurentCount.ToString();
        }

        public static void createMatrix()
        {
            for (int documentNumber = 2; documentNumber <= 59; documentNumber++)
            //for (int documentNumber = 2; documentNumber <= 3; documentNumber++)
            {
                matrixList = new string[wordList.Count(), wordList.Count()];
                for (int i = 0; i < wordList.Count; i++)
                {
                    String rowElement = wordList[i];
                    for (int j = 0; j < wordList.Count; j++)
                    {
                        String matrixRowColumnCount;
                        String columnElement = wordList[j];
                        String currentdocumentString = (String)MySheet.get_Range("S" + documentNumber.ToString(), "S" + documentNumber.ToString()).Cells.Value;

                       matrixRowColumnCount = FindElementPosition(currentdocumentString, rowElement, columnElement);
                       matrixList[i, j] = matrixRowColumnCount;
                    }
                }
                documentsMatrix[documentNumber-2] = matrixList;
            }           
        }

        public static void printMatrix()
        {
            for(int i=0;i<wordList.Count();i++)
            {
                for(int j=0;j<wordList.Count;j++)
                {
                    Console.Write("{0}", matrixList[i, j]);
                    Console.Write("\t");
                }
                Console.Write("\n");
            }
        }
        public static void convertMatrixToVector()
        {
            int matrixSize = matrixList.GetLength(0);
            for (int i=0; i<58; i++)
            {

                List<int> documentVectorContents = new List<int>();
                finalVectorTable[i] = new List<int>();
                String[,] currentMatrixList = documentsMatrix[i];
                
                for (int row=0; row<matrixSize;row++)
                {
                    for (int column=0; column<matrixSize; column++)
                    {
                       String intermediate = currentMatrixList[row,column];                       
                        documentVectorContents.Add(Convert.ToInt32(intermediate));
                    }
                }
                finalVectorTable[i] = documentVectorContents;
            }
        }
        static void Main(string[] args)
        {
            bool initExcel = false;
            initExcel = InitializeExcel();

            if (!initExcel)
            {
                Console.WriteLine("Excel sheet not present");
                return;
            }
            ReadExcel();

            for (int i = 0; i <58; i++)
            {
                documentsMatrix.Add(new String[,]{});
            }
                
            createMatrix();
            convertMatrixToVector();            
        }
    }
}