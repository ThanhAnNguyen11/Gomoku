using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _1312014_Gomoku
{
    class Player : INotifyPropertyChanged
    {
        int row = board._row;
        int column = board._column;

        public int[,] arr;
        public int[,] arr_win;

        public Player()
        {
            arr = new int[row, column];
            value_default(arr);

            arr_win = new int[row, column];
            value_default(arr_win);
        }

        private void value_default(int[,] arr)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    arr[i, j] = 0;
                }
            }
        }


        public bool check_horizoltal(int a, int b)//Kiểm tra hàng ngang
        {
            arr_win[a, b] = 1;
            int count = 1;
            int x = a + 1;
            while (x < column && arr[a, b] == arr[x, b])
            {
                count++;
                arr_win[x, b] = 1;
                x++;
            }
            x = a - 1;
            while (x >= 0 && arr[a, b] == arr[x, b])
            {
                count++;
                arr_win[x, b] = 1;
                x--;
            }
            if (count >= 5)
            {
                return true;
            }
            else
            {
                value_default(arr_win);
                return false;
            }


        }

        public bool check_vertical(int a, int b)//Kiểm tra hàng dọc
        {
            arr_win[a, b] = 1;
            int count = 1;
            int y = b + 1;
            while (y < row && arr[a, b] == arr[a, y])
            {
                count++;
                arr_win[a, y] = 1;
                y++;
            }
            y = b - 1;
            while (y >= 0 && arr[a, b] == arr[a, y])
            {
                count++;
                arr_win[a, y] = 1;
                y--;
            }
            if (count >= 5)
            {
                return true;
            }
            else
            {
                value_default(arr_win);
                return false;
            }
        }

        public bool check_diagonally(int a, int b)//Kiểm tra theo đường chéo
        {
            arr_win[a, b] = 1;
            int count = 1;
            int x = a + 1;
            int y = b + 1;
            while (x < column && y < column && arr[a, b] == arr[x, y])
            {
                count = count + 1;
                arr_win[x, y] = 1;
                x++;
                y++;
            }
            x = a - 1;
            y = b - 1;
            while (x >= 0 && y >= 0 && arr[a, b] == arr[x, y])
            {
                count = count + 1;
                arr_win[x, y] = 1;
                x--;
                y--;
            }
            if (count >= 5)
            {
                return true;
            }
            else
            {
                value_default(arr_win);
                return false;
            }
        }
        public bool check_diagonally_extra(int a, int b)//Kiểm tra đường chéo phụ
        {
            arr_win[a, b] = 1;
            int count = 1;
            int x = a + 1;
            int y = b - 1;
            while (x < column && y >= 0 && arr[a, b] == arr[x, y])
            {
                count++;
                arr_win[x, y] = 1;
                x++;
                y--;
            }
            x = a - 1;
            y = b + 1;
            while (x >= 0 && y < column && arr[a, b] == arr[x, y])
            {
                count++;
                arr_win[x, y] = 1;
                x--;
                y++;
            }
            if (count >= 5)
            {
                return true;
            }
            else
            {
                value_default(arr_win);
                return false;
            }
        }

        public bool IsWin(int x, int y)
        {
            if (check_horizoltal(x, y) == true || check_vertical(x, y) == true || check_diagonally(x, y) == true || check_diagonally_extra(x, y))
                return true;
            return false;
        }


        public int[] Find_i(int x, int y, Player player1)
        {
            int x_start = x;
            int y_start = y;
            int[] arr_1 = new int[2];
            double[] arr1 = new double[10];
            double tren = y;
            double duoi = row - y;
            double trai = x;
            double phai = column - x;
            arr1[0] = tren;
            arr1[1] = duoi;
            arr1[2] = trai;
            arr1[3] = phai;

            double max = arr1[0];
            int i;
            for (i = 0; i < 8; i++)
            {
                if (arr1[i] > max)
                {
                    max = arr1[i];
                }
            }
            for (i = 0; i < 8; i++)
            {
                if (arr1[i] == max)
                {
                    break;
                }
            }
            int a, b;


            while (true)
            {
                if (i == 0)
                {
                    y--;
                    while (y > 0 && (player1.arr[x, y] == 1 || arr[x, y] == 1)) //sai
                    {
                        y--;
                    }
                    if (y == 0 && (player1.arr[x, y] == 1 || arr[x, y] == 1))
                    {
                        i = 1;
                        x = x_start;
                        y = y_start;
                    }
                    else
                    {
                        a = y;
                        b = x;
                        arr_1[0] = a;
                        arr_1[1] = b;
                        break;
                    }
                }
                if (i == 1)
                {
                    y++;
                    while (y < row - 1 && (player1.arr[x, y] == 1 || arr[x, y] == 1)) //sai
                    {
                        y++;
                    }
                    if (y == row - 1 && (player1.arr[x, y] == 1 || arr[x, y] == 1))
                    {
                        i = 2;
                        x = x_start;
                        y = y_start;
                    }
                    else
                    {
                        a = y;
                        b = x;
                        arr_1[0] = a;
                        arr_1[1] = b;
                        break;
                    }
                }
                if (i == 2)
                {
                    x--;

                    while (x > 0 && (player1.arr[x, y] == 1 || arr[x, y] == 1)) //sai
                    {
                        x--;
                    }
                    if (x == 0 && (player1.arr[x, y] == 1 || arr[x, y] == 1))
                    {
                        i = 3;
                        x = x_start;
                        y = y_start;

                    }
                    else
                    {
                        b = x;
                        a = y;
                        arr_1[0] = a;
                        arr_1[1] = b;
                        break;
                    }
                }
                if (i == 3)
                {
                    x++;
                    while (x < row - 1 && (player1.arr[x, y] == 1 || arr[x, y] == 1)) //sai
                    {
                        x++;
                    }
                    if (x == row - 1 && (player1.arr[x, y] == 1 || arr[x, y] == 1))
                    {
                        i = 0;
                        x = x_start;
                        y = y_start;
                    }
                    else
                    {
                        a = y;
                        b = x;
                        arr_1[0] = a;
                        arr_1[1] = b;
                        break;
                    }
                }

            }
            return arr_1;

        }
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, args);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
