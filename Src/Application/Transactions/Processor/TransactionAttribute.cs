using System;

namespace SysgamingApi.Src.Application.Transactions.Processor;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TransactionAttribute : Attribute
{
}
