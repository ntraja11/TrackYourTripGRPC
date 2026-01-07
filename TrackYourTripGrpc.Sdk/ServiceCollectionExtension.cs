using Microsoft.Extensions.DependencyInjection;
using TrackYourTripGrpc.Sdk.Interfaces;
using TrackYourTripGrpc.Sdk.Services;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGrpc.Sdk
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTrackYourTripGrpcSdk(this IServiceCollection services, string baseAddress)
        {
            services.AddGrpcClientWithHandler<Trip.TripClient>(baseAddress);
            services.AddGrpcClientWithHandler<Expense.ExpenseClient>(baseAddress);
            services.AddGrpcClientWithHandler<Auth.AuthClient>(baseAddress);
            services.AddGrpcClientWithHandler<Member.MemberClient>(baseAddress);

            services.AddScoped<IAuthGrpcService, AuthGrpcService>();
            services.AddScoped<IMemberGrpcService, MemberGrpcService>();
            services.AddScoped<IExpenseGrpcService, ExpenseGrpcService>();
            services.AddScoped<ITripGrpcService, TripGrpcService>();

            return services;
        }

        private static IHttpClientBuilder AddGrpcClientWithHandler<TClient>
            (this IServiceCollection services, string baseAddress) where TClient : class
        {
            return services.AddGrpcClient<TClient>(client => {
                client.Address = new Uri(baseAddress);
            })
           .ConfigurePrimaryHttpMessageHandler(() =>
           {
               return new SocketsHttpHandler
               {
                   SslOptions = new System.Net.Security.SslClientAuthenticationOptions
                   {
                       RemoteCertificateValidationCallback = (sender, cert, chain, errors) => true
                   }
               };

           });
        }
    }
}
