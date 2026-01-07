using Grpc.Core;
using System.Diagnostics;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Services;

public class ExpenseGrpcService : IExpenseGrpcService
{
    private readonly Expense.ExpenseClient _expenseClient;
    public ExpenseGrpcService(Expense.ExpenseClient expenseClient)
    {
        _expenseClient = expenseClient;
    }

    public async Task<ExpenseDetail> CreateExpenseAsync(ExpenseDetail ExpenseDetail, CancellationToken cancellationToken)
    {
        try
        {
            var request = new CreateExpenseRequest
            {
                Title = ExpenseDetail.Title,
                Description = ExpenseDetail.Description,
                Amount = ExpenseDetail.Amount,
                TripId = ExpenseDetail.TripId,
                MemberId = ExpenseDetail.MemberId
            };

            var response = await _expenseClient.CreateExpenseAsync(request, cancellationToken: cancellationToken);
            return response.Expense;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteExpenseAsync(int ExpenseId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteExpenseRequest { Id = ExpenseId };
            var response = await _expenseClient.DeleteExpenseAsync(request, cancellationToken: cancellationToken);
            return response.Success;

        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ExpenseDetail>> GetAllExpensesByMemberIdAsync(int MemberId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetAllExpensesByMemberIdRequest { MemberId = MemberId };
            var response = await _expenseClient.GetAllExpensesByMemberIdAsync(request, cancellationToken: cancellationToken);
            return response.Expenses;

        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ExpenseDetail>> GetAllExpensesByTripIdAsync(int tripId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetAllExpensesByTripIdRequest { TripId = tripId };
            var response = await _expenseClient.GetAllExpensesByTripIdAsync(request, cancellationToken: cancellationToken);
            return response.Expenses;

        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<ExpenseDetail> GetExpenseAsync(int ExpenseId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetExpenseRequest { Id = ExpenseId };
            var response = await _expenseClient.GetExpenseAsync(request, cancellationToken: cancellationToken);
            return response.Expense;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }

    public async Task<ExpenseDetail> UpdateExpenseAsync(ExpenseDetail ExpenseDetail, CancellationToken cancellationToken)
    {
        try
        {
            var request = new UpdateExpenseRequest
            {
                Id = ExpenseDetail.Id,
                Title = ExpenseDetail.Title,
                Description = ExpenseDetail.Description,
                Amount = ExpenseDetail.Amount,
                TripId = ExpenseDetail.TripId,
                MemberId = ExpenseDetail.MemberId
            };
            var response = await _expenseClient.UpdateExpenseAsync(request, cancellationToken: cancellationToken);
            return response.Expense;
        }
        catch (RpcException ex)
        {
            Debug.WriteLine($"gRPC ERROR: {ex.StatusCode} - {ex.Status.Detail}");
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UNEXPECTED ERROR: {ex.Message}");
            throw;
        }
    }
}
