using TrackYourTripGRPC.SharedProtos.Protos;

namespace TrackYourTripGrpc.Sdk.Interfaces;

public interface IExpenseGrpcService
{
    Task<ExpenseDetail> GetExpenseAsync(int ExpenseId, CancellationToken cancellationToken);
    Task<IEnumerable<ExpenseDetail>> GetAllExpensesByTripAsync(int tripId, CancellationToken cancellationToken);

    Task<IEnumerable<ExpenseDetail>> GetAllExpensesByMemberAsync(int memberId, CancellationToken cancellationToken);
    Task<ExpenseDetail> CreateExpenseAsync(ExpenseDetail ExpenseDetail, CancellationToken cancellationToken);

    Task<ExpenseDetail> UpdateExpenseAsync(ExpenseDetail ExpenseDetail, CancellationToken cancellationToken);

    Task<bool> DeleteExpenseAsync(int ExpenseId, CancellationToken cancellationToken);
}
