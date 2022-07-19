using System;
using System.Collections.Generic;
using System.Text;

namespace YurtYonetim.Core.Utilities.Results.Abstract
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
