using Grpc.Core;
using System.Text;
using System.Threading;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace TodoApp.Services
{
    public class UsersServiceSample
    {
        private readonly UsersService.UsersServiceClient? _service;

        public UsersServiceSample(InvestApiClient investApi)
        {
            _service = investApi.Users;
        }

        public async Task<GetAccountsResponse> GetAccountsResponse()
        {
            try
            {
                return await _service?.GetAccountsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return await _service?.GetAccountsAsync();

            }
        }

        public async Task<string> GetUserInfoDescriptionAsync()
        {
            var accountsResponse = await _service.GetAccountsAsync();
            var infoResponse = await _service.GetInfoAsync();
            var userTariffResponse = await _service.GetUserTariffAsync();

            GetMarginAttributesResponse? marginAttributesResponse = null;
            try
            {
                marginAttributesResponse = await _service.GetMarginAttributesAsync();
            }
            catch (Exception)
            {
                // ignored
            }

            return new UserInfoFormatter(infoResponse, userTariffResponse, marginAttributesResponse,
                    accountsResponse.Accounts)
                .Format();
        }

        public string GetUserInfoDescription()
        {
            var accountsResponse = _service.GetAccounts();
            var infoResponse = _service.GetInfo();
            var marginAttributesResponse = _service.GetMarginAttributes();
            var userTariffResponse = _service.GetUserTariff();

            return new UserInfoFormatter(infoResponse, userTariffResponse, marginAttributesResponse,
                    accountsResponse.Accounts)
                .Format();
        }


        private class UserInfoFormatter
        {
            private readonly IReadOnlyCollection<Account> _accounts;
            private readonly GetMarginAttributesResponse? _marginAttributes;
            private readonly GetUserTariffResponse _tariff;
            private readonly GetInfoResponse _userInfo;

            public UserInfoFormatter(GetInfoResponse userInfo, GetUserTariffResponse tariff,
                GetMarginAttributesResponse? marginAttributes, IReadOnlyCollection<Account> accounts)
            {
                _userInfo = userInfo;
                _tariff = tariff;
                _marginAttributes = marginAttributes;
                _accounts = accounts;
            }

            public string Format()
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("Current account is ");
                if (!_userInfo.PremStatus) stringBuilder.Append("not");

                stringBuilder.Append(" premium and belongs to ");
                if (!_userInfo.QualStatus) stringBuilder.Append("non-");

                stringBuilder.AppendLine("qualified investor")
                    .AppendLine();

                stringBuilder.AppendFormat("There are {0} accounts:", _accounts.Count)
                    .AppendLine();

                foreach (var account in _accounts)
                    stringBuilder.AppendFormat("- [{0}] {1}", account.Id, account.Name)
                        .AppendLine()
                        .AppendFormat("  Status: {0}", account.Status)
                        .AppendLine()
                        .AppendFormat("  Type: {0}", account.Type)
                        .AppendLine();

                if (_marginAttributes != null)
                {
                    stringBuilder.AppendLine("Margin attributes:");
                    stringBuilder
                        .Append("- Liquid value of the portfolio ")
                        .AppendLine(_marginAttributes.LiquidPortfolio.Format())
                        .Append("- Starting margin ")
                        .AppendLine(_marginAttributes.StartingMargin.Format())
                        .Append("- Minimal margin ")
                        .AppendLine(_marginAttributes.MinimalMargin.Format())
                        .Append("- Funds sufficiency level ")
                        .Append(_marginAttributes.FundsSufficiencyLevel)
                        .AppendLine()
                        .Append("- Amount of missing funds ")
                        .Append(_marginAttributes.AmountOfMissingFunds)
                        .AppendLine()
                        .AppendLine();
                }

                stringBuilder.AppendLine().AppendLine("Tariff limits:");
                foreach (var limit in _tariff.UnaryLimits)
                    foreach (var method in limit.Methods)
                        stringBuilder.AppendFormat("- {0} rpm for method {1}", limit.LimitPerMinute, method)
                            .AppendLine();


                foreach (var limit in _tariff.StreamLimits)
                    foreach (var stream in limit.Streams)
                        stringBuilder.AppendFormat("- {0} connection(s) for stream {1}", limit.Limit, stream)
                            .AppendLine();

                return stringBuilder.ToString();
            }
        }
    }
}
