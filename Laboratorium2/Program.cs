using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laboratorium2
{
    class Program
    {
        static void Main(string[] args)
        {
            Zadanie6();
            Zadanie7();
            Zadanie8();
        }

        //Zadanie 6:
        static void Zadanie6()
        {
            Console.WriteLine("\n ///Zadanie 6/// \n");
            AutoResetEvent are = new AutoResetEvent(false);
            FileStream fs = new FileStream("pliczek.txt", FileMode.Open);
            Byte[] data = new Byte[1024];
            IAsyncResult ar = fs.BeginRead(data, 0, data.Length, myAsyncCallBack, new object[] { fs, data, are });
            are.WaitOne();
        }
        static void myAsyncCallBack(IAsyncResult state)
        {
            object[] data = (object[])state.AsyncState;
            FileStream fs = (FileStream)data[0];
            byte[] buffer = (byte[])data[1];
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            AutoResetEvent are = (AutoResetEvent)data[2];
            fs.Close();
            fs.EndRead(state);
            are.Set();
        }

        //Zadanie 7:
        static void Zadanie7()
        {
            Console.WriteLine("\n ///Zadanie 7/// \n");
            FileStream fs = new FileStream("pliczek.txt", FileMode.Open);
            Byte[] data = new Byte[fs.Length];
            IAsyncResult ar = fs.BeginRead(data, 0, data.Length, null, null);
            int i = fs.EndRead(ar);
            fs.Close();
            Console.WriteLine(Encoding.ASCII.GetString(data,0,i));
        }

        //Zadanie 8:
        delegate int DelegateType(object args);
        static DelegateType delegateName;
        static DelegateType delegateName2;
        static DelegateType delegateName3;
        static DelegateType delegateName4;
        static void Zadanie8()
        {
            Console.WriteLine("\n ///Zadanie 8/// \n");
            delegateName = new DelegateType(silniaI);
            IAsyncResult ar = delegateName.BeginInvoke(10, null, null);
            delegateName2 = new DelegateType(silniaR);
            IAsyncResult ar2 = delegateName2.BeginInvoke(10, null, null);
            delegateName3 = new DelegateType(fiboI);
            IAsyncResult ar3 = delegateName3.BeginInvoke(5, null, null);
            delegateName4 = new DelegateType(fiboR);
            IAsyncResult ar4 = delegateName4.BeginInvoke(5, null, null);

            Console.WriteLine("SilniaI "+delegateName.EndInvoke(ar));
            Console.WriteLine("SilniaR "+delegateName2.EndInvoke(ar2));
            Console.WriteLine("FiboI "+delegateName3.EndInvoke(ar3));
            Console.WriteLine("FiboR "+delegateName4.EndInvoke(ar4));
        }
        static int silniaI(object args)
        {
            if ((int)args == 0)
                return 1;
            int k = 1;
            for (int i = 1; i <= (int)args; i++)
            {
                k *= i;
            }
            Console.WriteLine("Silnia iteracyjnie skonczyla działanie");
            return k;
        }
        static int silniaR(object args)
        {
            if ((int)args < 1)
                return 1;
            else
                return (int)args * silniaR((int)args - 1);
        }
        static int fiboI(object args)
        {
            int firstnumber = 0, secondnumber = 1, result = 0;

            if ((int)args == 0) return 0;
            if ((int)args == 1) return 1;

            for (int i = 2; i <= (int)args; i++)
            {
                result = firstnumber + secondnumber;
                firstnumber = secondnumber;
                secondnumber = result;
            }
            Console.WriteLine("Fibonnaci iteracyjnie skonczyl działanie");
            return result;
        }
        static int fiboR(object args)
        {
            if (((int)args == 1) || ((int)args == 2))
                return 1;
            else
                return fiboR((int)args - 1) + fiboR((int)args - 2);
        }
    }
}
