//Main Clas For Solve A Star 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
namespace Copy_the_N_puzzle
{
    class AstarAlogrithm
    {
        public string FileName;
        public SortedList<string, string> ARR_MOVES;
        public MatrixCLASS ARR2D;
        public int size;
        int[,] ARR;
        public int Num_Manhateen;
        public int Num_Hamming;
        PaiortyQueue OPenList;
        SortedDictionary<string, NODE> ClosedList;
        public  int[,] BESTARR;
        #region SolveOrNOT_Var
        private static Int64 swap_counter = 0;
        private static Int64 swap_counter2 = 0;
        #endregion
        public AstarAlogrithm()
        {
            ARR = new int[100, 100];
            ARR_MOVES = new SortedList<string, string>();
            ARR_MOVES.Add("UP", "DOWN");
            ARR_MOVES.Add("DOWN", "UP");
            ARR_MOVES.Add("LEFT", "RIGHT");
            ARR_MOVES.Add("RIGHT", "LEFT");
            Num_Manhateen = -1;
            Num_Hamming = -1;
            size = 0;
             ClosedList =  new SortedDictionary<string,NODE>();
            BESTARR = new int[3, 3];
        }
        public bool check_if_equl(int[,] arr, int[,] arr2, int s)
        {
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    if (arr[i, j] != arr2[i, j])
                        return false;
                }
            }
            return true;
        }
        public int[,] getACopy(int[,] COP)
        {
            int[,] newcopy = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    newcopy[i, j] = COP[i, j];
                }
            }
            return newcopy;
        }
        public string[] Get_Path(NODE FINAL)
        {
            List<string> PP = new List<string>();
            
            while (FINAL.Parent != null)
            {
                PP.Add(ARR_MOVES.Values[ARR_MOVES.IndexOfKey(FINAL.move)]);
                FINAL.EQUAL(FINAL.Parent);
            }
            return PP.ToArray();
        }
        public void  AccessCASe(int CASE , string FileName1 = "TEST.txt")
        {
            ARR2D = new MatrixCLASS(size);
            size = ARR2D.read_from_file(ARR, CASE, FileName1);
            BESTARR = getACopy(ARR);
        }
        public string GenerateAKEy(int[,] ARRMAIN, int size)
        {
            string temp = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp += ARRMAIN[i, j].ToString();
                }
            }
            return temp;
        }
        public string[] AStar_SOLVE_USing_HAMMing(int CASE, string FileName1="TEST.txt")
        {
            OPenList = new PaiortyQueue();
            size = ARR2D.read_from_file(ARR, CASE, FileName1);
            NODE X = new NODE(size);
            X.Parent = null;
            X.PUZZLE = getACopy(ARR);
            X.HeuristicValue = Hamming(ARR, size);
            X.move = "";
            X.PuzzleSIze = size;
            X.COSTSOFAR = 0;
            X.calac_Total_using_Hamming();
            OPenList.push(X);
            ClosedList = new SortedDictionary<string, NODE>();
            if (ChecK_Solved())
            {
                while (OPenList.opendlistsize() != 0)
                {

                    NODE CURR = new NODE(size);
                    NODE TEMPNODE = new NODE(size);
                    NODE T = new NODE(size);
                    CURR = OPenList.POP();
                    TEMPNODE.EQUAL(CURR);
                    T.EQUAL(CURR);
                   
                    if (CURR.HeuristicValue == 1)
                    {
                       return Get_Path(CURR);
                    }
                    try
                    {
                        ClosedList.Add(GenerateAKEy(CURR.PUZZLE, CURR.PuzzleSIze), CURR);
                    }
                    catch(Exception E)
                    {
                        MessageBox.Show("OUT OFF MEMORY" , "WRONG" , MessageBoxButtons.OK , MessageBoxIcon.Warning);
                    }
                    //Console.WriteLine("********************************************************");
                    foreach (KeyValuePair<string, string> it in ARR_MOVES)
                    {
                        bool change = false;
                        ARR2D.SETMAINARRANDTEMP(getACopy(CURR.PUZZLE));
                        CURR.PUZZLE = ARR2D.GetTempArr(it.Key, ref change);
                        Num_Hamming = Hamming(CURR.PUZZLE, size);
                        if (change)
                        {
                            string CurentKey = GenerateAKEy(CURR.PUZZLE, size);
                            if (ClosedList.ContainsKey(CurentKey))
                            {

                                NODE NewNode = new NODE(size);
                                ClosedList.TryGetValue(CurentKey, out NewNode);
                                if (Num_Hamming + CURR.COSTSOFAR + 1 < NewNode.TOTAL)
                                {
                                    ClosedList.Remove(CurentKey);
                                    NODE newnode = new NODE(size);
                                    newnode.HeuristicValue = Num_Hamming;
                                    newnode.PUZZLE = getACopy(CURR.PUZZLE);
                                    newnode.PuzzleSIze = size;
                                    newnode.move = it.Key;
                                    newnode.COSTSOFAR = TEMPNODE.COSTSOFAR + 1;
                                    newnode.calac_Total_using_Hamming();
                                    newnode.Parent = TEMPNODE;

                                    OPenList.push(newnode);
                                }
                            }
                            else
                            {
                                NODE newnode = new NODE(size);
                                newnode.HeuristicValue = Num_Hamming;
                                newnode.PUZZLE = getACopy(CURR.PUZZLE);
                                newnode.PuzzleSIze = size;
                                newnode.move = it.Key;
                                newnode.COSTSOFAR = TEMPNODE.COSTSOFAR + 1;
                                newnode.calac_Total_using_Hamming();
                                newnode.Parent = TEMPNODE;

                                OPenList.push(newnode);
                            }
                            change = false;
                            CURR.PUZZLE = ARR2D.GetTempArr(it.Value, ref change);
                        }
                    }

                }

            }
            return null;
        }
        public string[] AStar_SOLVE_using_Mnhaten(int CASE , string FileName1 ="TEST.txt" )
        {
            OPenList = new PaiortyQueue();
            size = ARR2D.read_from_file(ARR, CASE , FileName1);
            NODE X = new NODE(size);
            X.Parent = null;
            X.PUZZLE = getACopy(ARR);
            X.HeuristicValue = Manhateen(ARR, size);
            X.move = "";
            X.PuzzleSIze = size;
            X.COSTSOFAR = 0;
            X.calac_Total_using_Manhateen();
            OPenList.push(X);
            ClosedList = new SortedDictionary<string, NODE>();


            int bestlevel;
            bestlevel = int.MaxValue;
            if (ChecK_Solved())
            {
                while (OPenList.opendlistsize() != 0)
                {
                    NODE CURR = new NODE(size);
                    NODE TEMPNODE = new NODE(size);
                    NODE T = new NODE(size);
                    CURR = OPenList.POP();
                    TEMPNODE.EQUAL(CURR);
                    T.EQUAL(CURR);

                    if (CURR.HeuristicValue == 0)
                    {
                            return Get_Path(CURR);
                    }
                    try
                    {
                        ClosedList.Add(GenerateAKEy(CURR.PUZZLE, CURR.PuzzleSIze), CURR);
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show("OUT OFF MEMORY", "WRONG", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    //Console.WriteLine("********************************************************");
                    foreach (KeyValuePair<string, string> it in ARR_MOVES)
                    {
                        bool change = false;
                        ARR2D.SETMAINARRANDTEMP(getACopy(CURR.PUZZLE));
                        CURR.PUZZLE = ARR2D.GetTempArr(it.Key, ref change);
                        Num_Manhateen = Manhateen(CURR.PUZZLE, size);
                        if (change)
                        {
                            string CurentKey = GenerateAKEy(CURR.PUZZLE, size);
                            if (ClosedList.ContainsKey(CurentKey))
                            {

                                NODE NewNode = new NODE(size);
                                ClosedList.TryGetValue(CurentKey, out NewNode);
                                if (Num_Manhateen + CURR.COSTSOFAR + 1 < NewNode.TOTAL)
                                {
                                    ClosedList.Remove(CurentKey);
                                    NODE newnode = new NODE(size);
                                    newnode.HeuristicValue = Num_Manhateen;
                                    newnode.PUZZLE = getACopy(CURR.PUZZLE);
                                    newnode.PuzzleSIze = size;
                                    newnode.move = it.Key;
                                    newnode.COSTSOFAR = TEMPNODE.COSTSOFAR + 1;
                                    newnode.calac_Total_using_Manhateen();
                                    newnode.Parent = TEMPNODE;

                                    OPenList.push(newnode);
                                }
                            }
                            else
                            {
                                NODE newnode = new NODE(size);
                                newnode.HeuristicValue = Num_Manhateen;
                                newnode.PUZZLE = getACopy(CURR.PUZZLE);
                                newnode.PuzzleSIze = size;
                                newnode.move = it.Key;
                                newnode.COSTSOFAR = TEMPNODE.COSTSOFAR + 1;
                                newnode.calac_Total_using_Manhateen();
                                newnode.Parent = TEMPNODE;

                                OPenList.push(newnode);
                            }
                            change = false;
                            CURR.PUZZLE = ARR2D.GetTempArr(it.Value, ref change);
                        }
                    }

                }

            }
            return null;
        }
        public int Hamming(int[,] ARR2D, int n)
        {
            int H = 1, counter = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (H != ARR2D[i, j])
                    {
                        counter++;
                    }

                    H += 1;
                }
            }
            return counter;
        }
        public void disply(int[,] arr1, int s)
        {
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                    Console.Write(arr1[i, j] + " ");
                Console.WriteLine();
            }
        }
        static int Manhateen(int[,] array_2D, int size)
        {
            int n = size * size, index = 0;
            int sum = 0;
            int row, row2, col = 0, col2 = 0;
            int[] arr_1D = new int[(size * size)];
            int[] arr_sorted = new int[(size * size)];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (array_2D[i, j] != 0)
                    {
                        arr_1D[index] = array_2D[i, j];
                    }
                    if (index < (size * size) - 1)
                        arr_sorted[index] = index + 1;
                    index++;
                }
            }
            arr_sorted[(size * size) - 1] = 0;
            for (int i = 0; i < n; i++)
            {
                if (arr_1D[i] != 0)
                {
                    row = i / size;
                    col = Math.Abs((row * size - i));
                    int element_index_in_arr_sorted = arr_1D[i] - 1;
                    row2 = element_index_in_arr_sorted / size;
                    col2 = Math.Abs((row2 * size - element_index_in_arr_sorted));
                    sum += Math.Abs(row2 - row) + Math.Abs(col2 - col);
                }
            }
            return sum;
        }
        #region SolveOrNotFunctions
        static void merage_sort(int[] A, int low, int mid, int high)
        {
            long z = 0, s = 0;

            int t1 = mid - low + 1;
            int t2 = high - mid;

            int[] temp = new int[t1 + 1];
            int[] temp2 = new int[t2 + 1];

            if (t1 <= t2)
            {
                for (int i = 0; i < t2; i++)
                {
                    if (i < t1)
                        temp[i] = A[low + i];
                    if (i < t2)
                        temp2[i] = A[mid + 1 + i];
                }
            }
            else
            {
                for (int i = 0; i < t1; i++)
                {
                    if (i < t2)
                        temp2[i] = A[mid + 1 + i];
                    if (i < t1)
                        temp[i] = A[low + i];
                }
            }

            temp[t1] = 2147483647;
            temp2[t2] = 2147483647;


            for (int i = low; i < high + 1; i++)
            {
                if (temp[z] <= temp2[s])
                {
                    A[i] = temp[z]; z++;
                    swap_counter += s;
                }
                else { A[i] = temp2[s]; s++; }
            }
        }

        static void dividing_Array(int[] A, int low, int high)
        {
            if (low < high)
            {
                int mid = (low + high) / 2;
                dividing_Array(A, low, mid);
                dividing_Array(A, mid + 1, high);
                merage_sort(A, low, mid, high);
            }
        }

        static bool NumOfAdjacentSwaps(int[] A, int N)
        {
            dividing_Array(A, 0, N - 1);
            Int64 num_of_swap = swap_counter;
            swap_counter2 = swap_counter;
            swap_counter = 0;

            if (num_of_swap % 2 == 0) return true;

            else return false;

        }
        #endregion
        static bool solve_or_not(int[] Array_1D_to_check, int zero_row, int zero_col, int n)
        {
            swap_counter -= ((n * zero_row) + zero_col);
            if (n == 2)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i == 1) return true;
                    if (Array_1D_to_check[i] > Array_1D_to_check[i + 1]) return false;
                }
            }
            else
            {
                if ((n % 2 != 0 && NumOfAdjacentSwaps(Array_1D_to_check, n * n)) ||
                (n % 2 == 0 && ((zero_row % 2 != 0) == NumOfAdjacentSwaps(Array_1D_to_check, n * n))))
                    return true;

                else return false;

            }

            return false;
        }
        public bool ChecK_Solved()
        {
            int[] OneDArr = ARR2D.ConvertTo1DArray(ARR2D.MainArr, ARR2D.size);

            if (solve_or_not(OneDArr, ARR2D.Zero.y, ARR2D.Zero.x, ARR2D.size))
            {
                Console.WriteLine("Solved..");
                return true;
            }
            else
            {
                Console.WriteLine("Not Solved..");
                return false;
            }
        }
    }
}
