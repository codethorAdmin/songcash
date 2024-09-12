using Microsoft.Extensions.Options;
using Songcash.Configuration;
using Songcash.Model;
using MySql.Data.MySqlClient;
using Dapper;
using Songcash.Model.Dto;

namespace Songcash.Repository;

public class RequestRepository
{
    private string _connectionString;

    public RequestRepository(IOptions<DatabaseConfiguration> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    public async Task<CreatedRequestResultDto> CreateRequest(Request request)
    {
        var connection = new MySqlConnection(_connectionString);

        try
        {
            await connection.OpenAsync();

            var insertQuery = @"INSERT INTO Requests (user_id, request_start_date, effective_start_date, effective_end_date, status, step_in_flow, auto_estimated_income, auto_estimated_payment_to_recover, calculated_income, final_payment_to_recover, spotify_link)
                VALUES (@UserId, @RequestStartDate, @EffectiveStartDate, @EffectiveEndDate, @Status, @StepInFlow, @AutoEstimatedIncome, @AutoEstimatedPaymentToRecover, @CalculatedIncome, @FinalPaymentToRecover, @SpotifyLink, @UserName)";
            _ = await connection.ExecuteAsync(insertQuery,
                new
                {
                    UserId = request.UserId,
                    RequestStartDate = request.RequestStartDate,
                    EffectiveStartDate = request.EffectiveStartDate,
                    EffectiveEndDate = request.EffectiveEndDate,
                    Status = request.Status,
                    StepInFlow = request.StepInFlow,
                    AutoEstimatedIncome = request.AutoEstimatedIncome,
                    AutoEstimatedPaymentToRecover = request.AutoEstimatedPaymentToRecover,
                    CalculatedIncome = request.CalculatedIncome,
                    FinalPaymentToRecover = request.FinalPaymentToRecover,
                    SpotifyLink = request.SpotifyLink,
                });

            return new CreatedRequestResultDto
            {
                InsertedId = await connection.GetInsertedId(),
                Request = request
            };
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<Request> UpdateRequest(Request request)
    {
        throw new NotImplementedException();
    }
}

