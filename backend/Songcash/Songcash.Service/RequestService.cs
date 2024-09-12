using Songcash.Dto;
using Songcash.Model;
using Songcash.Model.Dto;
using Songcash.Repository;

namespace Songcash.Service;

public class RequestService(RequestRepository requestRepository, UserRepository userRepository)
{
    private readonly RequestRepository _requestRepository = requestRepository;
    private readonly UserRepository _userRepository = userRepository;

    public async Task<CreatedRequestResultDto> CreateRequest(CreateRequestDto requestDto, string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        var request = new Request
        {
            AutoEstimatedIncome = requestDto.AutoEstimatedIncome,
            AutoEstimatedPaymentToRecover = requestDto.AutoEstimatedPaymentToRecover,
            UserId = user.Id,
            SpotifyLink = requestDto.SpotifyLink
        };

        return await _requestRepository.CreateRequest(request);
    }

    public async Task<Request> UpdateRequest(UpdateRequestDto requestDto)
    {
        var completeRequest = new Request
        {

        };

        return await _requestRepository.UpdateRequest(completeRequest);
    }
}

