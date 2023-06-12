using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using TodoApp.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersInvestInfoController : ControllerBase
    {
        private UsersServiceSample usersServiceSample;
        public UsersInvestInfoController(UsersServiceSample usersServiceSample)
        {
            this.usersServiceSample = usersServiceSample;
        }
        // GET: api/<UsersInvestInfo>
        [HttpGet("getInfoUserAccounts")]
        public async Task<dynamic> GetInfoUserAccounts()
        {
            var accountResponse = await usersServiceSample.GetAccountsResponse();
            Console.Write(accountResponse.Accounts);
            return accountResponse.Accounts;
        }

        [HttpGet("getAllUserInformation")]
        public async Task<dynamic> GetAllInfoUserAccounts()
        {
            var accountResponse = await usersServiceSample.GetUserInfoDescriptionAsync();
            Console.Write(accountResponse);
            return accountResponse;
        }

        // GET api/<UsersInvestInfo>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersInvestInfo>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersInvestInfo>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersInvestInfo>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
