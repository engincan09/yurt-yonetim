﻿using System;
using System.Collections.Generic;
using System.Text;
using YurtYonetim.Core.Utilities.Results.Abstract;

namespace YurtYonetim.Core.Utilities.Results.Concrete
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string messages) : base(success, messages)
        {
            Data = data;
        }
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
