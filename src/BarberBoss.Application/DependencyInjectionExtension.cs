using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Invoicing.Delete;
using BarberBoss.Application.UseCases.Invoicing.GetAll;
using BarberBoss.Application.UseCases.Invoicing.GetById;
using BarberBoss.Application.UseCases.Invoicing.Register;
using BarberBoss.Application.UseCases.Invoicing.Reports.Excel;
using BarberBoss.Application.UseCases.Invoicing.Reports.Pdf;
using BarberBoss.Application.UseCases.Invoicing.Update;
using BarberBoss.Application.UseCases.Login.DoLogin;
using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Application.UseCases.Users.Delete;
using BarberBoss.Application.UseCases.Users.Profile;
using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Application.UseCases.Users.Update;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterInvoiceUseCase, RegisterInvoiceUseCase>();
        services.AddScoped<IGetAllInvoiceUseCase, GetAllInvoiceUseCase>();
        services.AddScoped<IGetInvoiceByIdUseCase, GetInvoiceByIdUseCase>();
        services.AddScoped<IDeleteInvoiceUseCase, DeleteInvoiceUseCase>();
        services.AddScoped<IUpdateInvoiceUseCase, UpdateInvoiceUseCase>();
        services.AddScoped<IGenerateInvoicingReportExcelUseCase, GenerateInvoicingReportExcelUseCase>();
        services.AddScoped<IGenerateInvoicingReportPdfUseCase, GenerateInvoicingReportPdfUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
    }
}
