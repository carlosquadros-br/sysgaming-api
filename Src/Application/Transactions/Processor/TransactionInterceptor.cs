using System;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Application.AccountBalances.Command.DepositAccount;
using SysgamingApi.Src.Application.Transactions.Command.CreateTransaction;
using SysgamingApi.Src.Domain.Persitence;

namespace SysgamingApi.Src.Application.Transactions.Processor;


public class TransactionInterceptor : IInterceptor
{
    private readonly ICreateTransactionUseCase _createTransactionUseCase;

    public TransactionInterceptor(
    ICreateTransactionUseCase createTransactionUseCase)
    {
        _createTransactionUseCase = createTransactionUseCase;
    }

    public void Intercept(IInvocation invocation)
    {
        invocation.Proceed();
        var task = HandleOperation(invocation);
        task?.Wait();  // Ensure the task completes
    }

    private async Task HandleOperation(IInvocation invocation)
    {
        if (invocation.ReturnValue is null)
        {
            return;
        }
        var response = GetOperationResponse(invocation.ReturnValue);
        if (response is null || response is not OperationResponse operationResponse || !operationResponse.Sucess)
        {
            return;
        }
        await _createTransactionUseCase.Handle(mapperRequest(operationResponse));
    }

    private static object? GetOperationResponse(object originalResponse)
    {
        // Assuming the originalResponse is Task<something>, extract the result.
        if (originalResponse is Task task)
        {
            if (task.GetType().IsGenericType)
            {
                // Wait for the task to complete before accessing the result
                task.Wait();


                var resultType = task.GetType().GetGenericArguments()[0];
                var result = task.GetType().GetProperty("Result")?.GetValue(task);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return originalResponse;
    }

    private CreateTransactionRequest mapperRequest(
        OperationResponse operationResponse
    )
    {
        return new CreateTransactionRequest(
            operationResponse.Description,
            operationResponse.Type,
            operationResponse.Amount,
            operationResponse.CurrentBalanceUser,
            operationResponse.UserName,
            operationResponse.Currency
        );
    }


}
