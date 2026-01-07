using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface IExpenseGrpcService
{
    Task<ExpenseDetail> GetExpenseAsync(int ExpenseId, CancellationToken cancellationToken);
    Task<IEnumerable<ExpenseDetail>> GetAllExpensesByTripIdAsync(int tripId, CancellationToken cancellationToken);

    Task<IEnumerable<ExpenseDetail>> GetAllExpensesByMemberIdAsync(int MemberId, CancellationToken cancellationToken);
    Task<ExpenseDetail> CreateExpenseAsync(ExpenseDetail ExpenseDetail, CancellationToken cancellationToken);

    Task<ExpenseDetail> UpdateExpenseAsync(ExpenseDetail ExpenseDetail, CancellationToken cancellationToken);

    Task<bool> DeleteExpenseAsync(int ExpenseId, CancellationToken cancellationToken);
}
