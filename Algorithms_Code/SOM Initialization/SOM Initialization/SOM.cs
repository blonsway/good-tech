using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SOM_Initialization
{

    class SOM
    {
        static Dictionary<int, List<int>> _randomValuesMap = new Dictionary<int, List<int>>();
        static Dictionary<int, List<int>> _inputValuesMap = new Dictionary<int, List<int>>();

        public static Dictionary<int, List<int>> inputValuesMap
        {
            get { return SOM._inputValuesMap; }
            set { SOM._inputValuesMap = value; }
        }
        static int _maxRandom = 90000;
        static int _totalNumber = 100;
        static int _totalInputVectors = 60;

        public static int totalInputVectors
        {
            get { return SOM._totalInputVectors; }
            set { SOM._totalInputVectors = value; }
        }
        static int _somMatrixSize = 10;
        static int[,] _somMatrix = new int[10, 10];
        static String[,] _documentMatrix = new String[10, 10];
        

        public static String[,] documentMatrix
        {
            get { return SOM._documentMatrix; }
            set { SOM._documentMatrix = value; }
        }

        public static int somMatrixSize
        {
            get { return SOM._somMatrixSize; }
            set { SOM._somMatrixSize = value; }
        }
       

        public static int[,] somMatrix
        {
          get { return SOM._somMatrix; }
          set { SOM._somMatrix = value; }
        }


        public static int totalNumber
        {
            get { return SOM._totalNumber; }
            set { SOM._totalNumber = value; }
        }

        public static int maxRandom
        {
            get { return _maxRandom; }
            set { _maxRandom = value; }
        }

        public static Dictionary<int, List<int>> randomValuesMap
        {
            get { return SOM._randomValuesMap; }
            set { SOM._randomValuesMap = value; }
        }
        public static void generatedRandomVectorValues(Dictionary<int, List<int>> valueMap, int min, int max, int size)
        {
            int i, j;
            Random randomNumberGen = new Random();
            for ( i = 0; i < size; i++)
            {
                List<int>  randomNumberList = new List<int>();
                for ( j = 0; j < maxRandom; j++)
                {
                    
                    int randomNumber = randomNumberGen.Next(min, max);
                    randomNumberList.Add(randomNumber);
                    
                }

                valueMap.Add(i, randomNumberList);
            }
                
        }

        static void createSOMMatrix()
        {
            //store the positions
            int position = 0 ;
            for (int i = 0; i < somMatrixSize; i++)
            {
                for (int j = 0; j < somMatrixSize; j++)
                {
                    somMatrix[i, j] = position++;
                }
            }
        }

        static double computeEuclideanDistance(List<int> inputVector, List<int> SOMVector)
        {
            double squareDistance = 0;
            double squareRootDistance = 0;
            for (int i = 0; i < maxRandom; i++)
            {
                squareDistance += inputVector[i] * SOMVector[i];
            }
            squareRootDistance = Math.Sqrt(squareDistance);
            return squareRootDistance;
        }

        static void storeMinimumInDocumentMatrix(int documentNumber,double[,] distancesArray)
        {
            double minimum = distancesArray[0,0] ;
            int iPos = 0, jPos = 0;
            for (int i = 0; i < somMatrixSize; i++)
            {
                for (int j = 0; j < somMatrixSize; j++)
                {
                    double distanceValue = distancesArray[i, j];
                    if(distanceValue < minimum){
                        minimum = distanceValue;
                        iPos = i;
                        jPos = j;
                    }
                     
                }
            }
            if (documentMatrix[iPos, jPos] == null)
            {
                documentMatrix[iPos, jPos] = documentNumber +"";
            }
            else
            {
                documentMatrix[iPos, jPos] += ","+ documentNumber ;
            }

            Console.WriteLine("Shortest distance is " + minimum + " found at documentMatrix[" + iPos + "," + jPos + "]");
        }

        static void findTargetVector(int documentNumber, List<int> inputVector)
        {
            double[,] distancesArray = new double[10,10];
            
            for (int i = 0; i < somMatrixSize; i++)
            {
                for (int j = 0; j < somMatrixSize; j++)
                {
                    int somPosition = somMatrix[i,j];
                    distancesArray[i,j] =  computeEuclideanDistance(
                        inputVector,randomValuesMap[somPosition]);
                }
            }
            
            storeMinimumInDocumentMatrix(documentNumber, distancesArray);
            
        }

        static void displayDocumentMatrix()
        {
            for (int i = 0; i < somMatrixSize; i++)
            {
                for (int j = 0; j < somMatrixSize; j++)
                {
                    Console.Write("[" + i + "," + j + "] = "+documentMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        static void workOnSOMInitialization(Dictionary<int,List<int>> inputVectorMap)
        {
            generatedRandomVectorValues(randomValuesMap,1,10,totalNumber);
            createSOMMatrix();

            foreach (KeyValuePair<int, List<int>> keyValuePair in inputVectorMap)
            {
                int documentNumber = keyValuePair.Key;
                List<int> inputVector = keyValuePair.Value;
                findTargetVector(documentNumber, inputVector);
            }
            displayDocumentMatrix();
        }



        static void Main(string[] args)
        {
            generatedRandomVectorValues(inputValuesMap,1,10,totalInputVectors);
            workOnSOMInitialization(inputValuesMap);
            Console.ReadKey();
             /*List<int> valueList = randomValuesMap[7];
             foreach (int i in valueList)
             {
                 Console.Write(i+"\t");
             }
            Console.ReadKey();*/
        }
    }
}
