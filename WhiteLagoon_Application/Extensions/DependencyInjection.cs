using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Implementation;
using WhiteLagoon.Application.Services.Interface;

namespace WhiteLagoon.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IDashboardService, DashboardService>();
            //services.AddScoped<IVillaService, VillaService>();
            services.AddScoped<IVillaNumberService, VillaNumberService>();
            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
