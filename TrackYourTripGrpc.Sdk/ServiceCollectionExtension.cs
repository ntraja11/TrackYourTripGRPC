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
            services.AddGrpcClient<Trip.TripClient>(client => { 
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

            services.AddScoped<ITripGrpcService, TripGrpcService>();

            return services;
        }
    }
}
