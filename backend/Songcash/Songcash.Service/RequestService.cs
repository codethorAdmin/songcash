using Songcash.Dto;
using Songcash.Model;
using Songcash.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Songcash.Service
{
    public class RequestService
    {
        private readonly RequestRepository _requestRepository;

        public RequestService(RequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<int> CreateRequest(CreateRequestDto requestDto)
        {
            var completeRequest = new Request
            {
                AutoEstimatedIncome = requestDto.AutoEstimatedIncome,
                AutoEstimatedPaymentToRecover = requestDto.AutoEstimatedPaymentToRecover,
                UserId = 1,
                SpotifyLink = requestDto.SpotifyLink
            };

            return await _requestRepository.CreateRequest(completeRequest);
        }

        public async Task<Request> UpdateRequest(UpdateRequestDto requestDto)
        {
            var completeRequest = new Request
            {

            };

            return await _requestRepository.UpdateRequest(completeRequest);
        }
    }
}
