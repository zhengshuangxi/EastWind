using UnityEngine;
using System.Collections;

namespace Util.Operation
{
    public class WaitForEndOfOperation : IEnumerator
    {
        IOperation operation;

        public WaitForEndOfOperation(IOperation operation)
        {
            this.operation = operation;
        }

        public object Current
        {
            get
            {
                return null;
            }
        }

        public bool MoveNext()
        {
            return !operation.IsFinished();
        }

        public void Reset()
        {
        }
    }
}
