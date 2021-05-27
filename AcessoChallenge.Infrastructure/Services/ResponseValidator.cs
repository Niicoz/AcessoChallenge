using AcessoChallenge.Domain;
using AcessoChallenge.Domain.Interfaces;
using AcessoChallenge.Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcessoChallenge.Infrastructure.Services
{
    public class ResponseValidator : IResponseValidator
    {
        public async Task<ServiceResult<Account>> ResponseValidate(HttpResponseMessage response)
        {
            var statusCode = (int)response.StatusCode;
            var content = await response.Content.ReadAsStringAsync();

            var isValid = response.IsSuccessStatusCode;

            if (!isValid)
            {
                if (statusCode == StatusCodes.Status400BadRequest)
                {
                    return ServiceResult<Account>.Error(content);
                }

                if (statusCode == StatusCodes.Status404NotFound)
                {
                    return ServiceResult<Account>.Error("Invalid account number");
                }

                throw new Exception(content);
            }
            var account = JsonConvert.DeserializeObject<Account>(content);

            return ServiceResult<Account>.Success(account);
        }
    }
}