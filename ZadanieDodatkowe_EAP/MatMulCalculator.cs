using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieDodatkowe_EAP
{
    class MatMulCalculator
    {
        public void CalculateCompleted(object state)​



        public void Completion(int size, double[] mat, Exception ex, bool cancelled, AsyncOperation ao)​


        bool TaskCancelled(object taskID)

        void CalculateWorker(double[] mat1, double[] mat2, int size, AsyncOperation asyncOp)​

        public double getVal(double[] mat, int column, int row, int size)​

        public double[] MatMul(double[] mat1, double[] mat2, int size)​


        public virtual void MatMulAsync(double[] mat1, double[] mat2, int size, object taskId)​


        public void CancelAsync(object taskId)
        {

        }
    }
}
