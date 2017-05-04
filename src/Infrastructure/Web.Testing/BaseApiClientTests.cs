﻿namespace Skeleton.Web.Testing
{
    using System;
    using System.Threading.Tasks;
    using Integration.BaseApiClient;
    using Integration.BaseApiFluentClient;
    using Moq;
    using Xunit;

    public class BaseApiClientTests<TFixture, TApiClient> : IClassFixture<TFixture>
        where TFixture : BaseApiTestsFixture, new()
        where TApiClient : BaseFluentClient
    {
        protected readonly TFixture Fixture;
        protected readonly TApiClient ApiClient;
        protected dynamic AsyncApiClient => new FluentChainedTask<TApiClient>(Task.FromResult(ApiClient));

        public BaseApiClientTests(TFixture fixture)
        {
            Fixture = fixture;
            Fixture.MockLogger.ResetCalls();

            ApiClient = (TApiClient)Activator.CreateInstance(typeof(TApiClient),
                new ClientConfiguration
                {
                    BaseUrl = Fixture.Server.BaseAddress.ToString(),
                    TimeoutInMilliseconds = Fixture.TimeoutInMilliseconds
                },
                Fixture.Server.CreateHandler());
        }
    }
}