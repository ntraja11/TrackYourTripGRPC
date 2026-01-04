using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGRPCApi.Services
{
    public class ExpenseService : Expense.ExpenseBase
    {
        public TrackYourTripDbContext _dbContext { get; }
        public IMapper _mapper { get; }

        public ExpenseService(TrackYourTripDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public override async Task<CreateExpenseResponse> CreateExpense(CreateExpenseRequest request, ServerCallContext context)
        {
            var expenseEntity = new Models.ExpenseEntity
            {
                Title = request.Title,
                Description = request.Description,
                Amount = (decimal)request.Amount,
                TripId = request.TripId,
                MemberId = request.MemberId,
                ExpenseDate = DateTime.UtcNow.Date
            };
            _dbContext.Expenses.Add(expenseEntity);
            await _dbContext.SaveChangesAsync();
            var expenseDetail = _mapper.Map<ExpenseDetail>(expenseEntity);
            return new CreateExpenseResponse
            {
                Expense = expenseDetail
            };
        }

        public override async Task<GetExpenseResponse> GetExpense(GetExpenseRequest request, ServerCallContext context)
        {
            var expenseEntity = await _dbContext.Expenses.FindAsync(request.Id);
            if (expenseEntity is not null)
            {
                var expenseDetail = _mapper.Map<ExpenseDetail>(expenseEntity);
                return new GetExpenseResponse { Expense = expenseDetail };
            }
            throw new RpcException(new Status(StatusCode.NotFound, $"Expense with Id {request.Id} is not found."));
        }

        public override async Task<DeleteExpenseResponse> DeleteExpense(DeleteExpenseRequest request, ServerCallContext context)
        {
            var existingExpense = await _dbContext.Expenses.FindAsync(request.Id);
            if (existingExpense is not null)
            {
                _dbContext.Expenses.Remove(existingExpense);
                await _dbContext.SaveChangesAsync();
                return new DeleteExpenseResponse { Success = true };
            }
            throw new RpcException(new Status(StatusCode.NotFound, $"Expense with Id {request.Id} is not found."));
        }

        public override async Task<UpdateExpenseResponse> UpdateExpense(UpdateExpenseRequest request, ServerCallContext context)
        {
            var expenseEntity = await _dbContext.Expenses.FindAsync(request.Id);
            if (expenseEntity is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Expense with Id {request.Id} is not found."));
            }
            expenseEntity.Title = request.Title;
            expenseEntity.Description = request.Description;
            expenseEntity.Amount = (decimal)request.Amount;
            expenseEntity.TripId = request.TripId;
            expenseEntity.MemberId = request.MemberId;
            await _dbContext.SaveChangesAsync();

            var expenseDetail = _mapper.Map<ExpenseDetail>(expenseEntity);
            return new UpdateExpenseResponse
            {
                Expense = expenseDetail
            };
        }

        public override async Task<GetAllExpensesByTripIdResponse> GetAllExpensesByTripId(GetAllExpensesByTripIdRequest request, ServerCallContext context)
        {
            var expenses = await _dbContext.Expenses.Where(e => e.TripId == request.TripId).ToListAsync();
            var response = new GetAllExpensesByTripIdResponse();

            response.Expenses.AddRange(_mapper.Map<IEnumerable<ExpenseDetail>>(expenses));                        
            return response;
        }

        public override async Task<GetAllExpensesByMemberIdResponse> GetAllExpensesByMemberId(GetAllExpensesByMemberIdRequest request, ServerCallContext context)
        {
            var expenses = await _dbContext.Expenses.Where(e => e.MemberId == request.MemberId).ToListAsync();
            var response = new GetAllExpensesByMemberIdResponse();

            response.Expenses.AddRange(_mapper.Map<IEnumerable<ExpenseDetail>>(expenses));
            return response;

        }
    }
}
